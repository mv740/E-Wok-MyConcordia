﻿using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace MyConcordiaID.Extensions
{
    /// <summary>
    /// https://github.com/aspnet-contrib/AspNet.Security.OpenIdConnect.Samples/blob/master/samples/Mvc/Mvc.Server/Extensions/AppBuilderExtensions.cs
    /// </summary>
    public static class AppBuilderExtensions
    {
        public static IApplicationBuilder UseWhen(this IApplicationBuilder app,
            Func<HttpContext, bool> condition, Action<IApplicationBuilder> configuration)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var builder = app.New();
            configuration(builder);

            return app.Use(next =>
            {
                builder.Run(next);

                var branch = builder.Build();

                return context =>
                {
                    if (condition(context))
                    {
                        return branch(context);
                    }

                    return next(context);
                };
            });
        }
    }
}