using System;
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
        internal static IConfiguration BuildConfiguration(string directory)
        {
            return new ConfigurationBuilder()
                .SetBasePath(directory)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown"}.json", true)
                .AddJsonFile("appsettings.Local.json", true)
                .AddEnvironmentVariables()
                .Build();
        }

        /// <summary>
        ///     Adds common configuration sources to the <see cref="IConfigurationBuilder" />.
        /// </summary>
        /// <param name="builder">
        ///     <see cref="IConfigurationBuilder" />
        /// </param>
        /// <param name="configurations"></param>
        /// <returns>An <see cref="IConfigurationBuilder" /></returns>
        public static IConfigurationBuilder AddCommonConfiguration(this IConfigurationBuilder builder, Action<IConfigurationBuilder>? configurations = default)
        {
            configurations?.Invoke(builder);
            
            builder.AddJsonFile("appsettings.Local.json", true);
            builder.AddEnvironmentVariables();
            
            return builder;
        }
    }
}