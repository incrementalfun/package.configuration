using System;
using System.IO;
using System.Reflection;
using Amazon;
using Amazon.CloudWatchLogs;
using Amazon.Runtime;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Exceptions.Core;
using Serilog.Exceptions.EntityFrameworkCore.Destructurers;
using Serilog.Formatting.Compact;
using Serilog.Sinks.AwsCloudWatch;
using Serilog.Sinks.AwsCloudWatch.LogStreamNameProvider;

namespace Incremental.Common.Configuration
{
    /// <summary>
    /// Common set of configuration settings.
    /// </summary>
    public static class CommonConfigurationExtensions
    {
        /// <summary>
        /// Builds an IConfiguration.
        /// </summary>
        /// <param name="directory">Directory of the project.</param>
        /// <returns></returns>
        public static IConfiguration BuildConfiguration(string? directory)
        {
            return new ConfigurationBuilder()
                .SetBasePath(directory ?? Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
                .AddJsonFile($"appsettings.Local.json", true)
                .AddEnvironmentVariables()
                .Build();
        }
    }
}