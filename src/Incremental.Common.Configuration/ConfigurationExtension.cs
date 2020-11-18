using System;
using Amazon.CloudWatchLogs;
using Amazon.Runtime;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.AwsCloudWatch;
using Serilog.Sinks.AwsCloudWatch.LogStreamNameProvider;

namespace Incremental.Common.Configuration
{
    /// <summary>
    /// Common set of configuration settings.
    /// </summary>
    public static class ConfigurationExtension
    {
        /// <summary>
        /// Builds an IConfiguration.
        /// </summary>
        /// <param name="directory">Directory of the project.</param>
        /// <returns></returns>
        public static IConfiguration LoadConfiguration(string directory)
        {
            return new ConfigurationBuilder()
                .SetBasePath(directory)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
                .AddJsonFile($"appsettings.Local.json", true)
                .AddEnvironmentVariables()
                .Build();
        }

        /// <summary>
        /// Creates a logger using Serilog.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="service"></param>
        /// <returns></returns>
        public static ILogger LoadLogger(IConfiguration configuration, string service)
        {
            var environment = $"{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}";
            
            var cloudWatchSink = new CloudWatchSinkOptions
            {
                LogGroupName = "incremental",
                LogStreamNameProvider = new ConfigurableLogStreamNameProvider($"{service}#{environment}", false, false),
                CreateLogGroup = true,
                LogGroupRetentionPolicy = LogGroupRetentionPolicy.OneWeek
            };
            
            var client = new AmazonCloudWatchLogsClient(new BasicAWSCredentials(configuration["AWS_ACCESS_KEY"], configuration["AWS_SECRET_KEY"]));

            var loggerConf = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .WriteTo.AmazonCloudWatch(cloudWatchSink, client);

            if (environment == "Development")
            {
                loggerConf.WriteTo.Console();
                loggerConf.MinimumLevel.Debug();
            }
            else
            {
                loggerConf.MinimumLevel.Warning();
            }

            return loggerConf.CreateLogger();
        }
    }
}