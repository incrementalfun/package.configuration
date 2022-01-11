using System;
using Microsoft.Extensions.Configuration;

namespace Incremental.Common.Configuration
{
    /// <summary>
    ///     Common set of configuration settings.
    /// </summary>
    public static class ConfigurationBuilderExtensions
    {
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