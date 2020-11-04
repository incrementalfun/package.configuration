using System;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Exceptions;

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
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .AddJsonFile($"appsettings.Local.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
        }
        
        /// <summary>
        /// Creates a logger using Serilog.
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static ILogger LoadLogger(IConfiguration configuration)
        {
            var loggerConf = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .WriteTo.Console();
            
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
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