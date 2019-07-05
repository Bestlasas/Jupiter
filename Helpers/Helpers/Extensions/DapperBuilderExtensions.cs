// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using Jupiter.Helpers.Interfaces;
using Jupiter.Helpers.Models.Utils;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods to add EF database support to IdentityServer.
    /// </summary>
    public static class DapperBuilderExtensions
    {

        /// <summary>
        /// Configures Dapper implementation.
        /// </summary>
        /// <param name="services">The services</param>
        /// <param name="configuration">Represents a section of application configuration values.</param>
        /// <returns></returns>
        public static IServiceCollection AddConfigurationDapperStorage<T>(this IServiceCollection services, IConfigurationSection configuration)
            where T : class, IDapperAdapter
        {
            if (configuration.Key.Equals("DapperIdentity"))
            {
                services.Configure<ConnectionProviderOptions>(configuration);
            }
            else if (configuration.Key.Equals("ConnectionStrings"))
            {
                var defaultConnection = configuration.GetValue<string>("DefaultConnection");
                if (!string.IsNullOrEmpty(defaultConnection))
                {
                    services.Configure<ConnectionProviderOptions>(x =>
                    {
                        x.ConnectionString = defaultConnection;
                    });
                }
                else
                {
                    var children = configuration.GetChildren();
                    if (children.Any())
                    {
                        services.Configure<ConnectionProviderOptions>(x =>
                        {
                            x.ConnectionString = configuration.GetChildren().First().Value;
                        });
                    }
                    else
                    {
                        throw new Exception("There's no DapperIdentity nor ConnectionStrings section with a connection string configured. Please provide one of them.");
                    }
                }
            }
            else
            {
                throw new Exception("There's no DapperIdentity nor ConnectionStrings section with a connection string configured. Please provide one of them.");
            }

            services.AddScoped<IDapperAdapter, T>();
            return services;
        }

       
    }
}
