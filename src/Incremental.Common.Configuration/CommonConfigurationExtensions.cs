using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Incremental.Common.Configuration
{
    /// <summary>
    ///     Common set of configuration settings.
    /// </summary>
    public static class CommonConfigurationExtensions
    {
        /// <summary>
        ///     Builds an IConfiguration.
        /// </summary>
        /// <param name="directory">Directory of the project.</param>
        /// <returns>An <see cref="IConfiguration" />.</returns>
        public static IConfiguration BuildConfiguration(string? directory)
        {
            return new ConfigurationBuilder()
                .SetBasePath(directory ?? Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
                .AddJsonFile("appsettings.Local.json", true)
                .AddEnvironmentVariables()
                .Build();
        }
    }
}