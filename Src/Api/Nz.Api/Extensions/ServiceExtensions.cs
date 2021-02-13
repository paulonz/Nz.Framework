/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Api.Extensions
{
    using System;
    using System.IO;
    using System.Linq;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.ResponseCompression;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;
    using Newtonsoft.Json.Serialization;
    using Nz.Common.GeneralSettings;
    using Nz.Core.Service;
    using Nz.Libs.Jwt.Settings;

    /// <summary>
    /// Configurações dos serviços
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// Extensão para configurar compressão de response
        /// </summary>
        /// <param name="services">Serviços</param>
        /// <returns>Serviços</returns>
        public static IServiceCollection ConfigureDefaultCompressionService(
            this IServiceCollection services)
        {
            services
                .AddResponseCompression(options =>
                {
                    options.Providers.Add<BrotliCompressionProvider>();
                    options.Providers.Add<GzipCompressionProvider>();
                    options.MimeTypes =
                        ResponseCompressionDefaults.MimeTypes.Concat(
                            new[] { "image/svg+xml" });
                });

            return services;
        }

        /// <summary>
        /// Extensão para configurar MVC
        /// </summary>
        /// <param name="services">Auto referencia para a coleção de seviços</param>
        public static IServiceCollection ConfigureDefaultMvcService(
            this IServiceCollection services)
        {
            services
                .AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore;
                });

            return services;
        }

        /// <summary>
        /// Extensão para configurar Cors
        /// </summary>
        /// <param name="services">Auto referencia para a coleção de seviços</param>
        public static IServiceCollection ConfigureDefaultCorsService(
            this IServiceCollection services)
        {
            services
                .AddCors(options =>
                {
                    options.AddPolicy("CorsPolicy",
                        builder => builder.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials());
                });

            return services;
        }

        /// <summary>
        /// Extensão para configurar autenticação e autorização
        /// </summary>
        /// <param name="services">Auto referencia para a coleção de seviços</param>
        /// <returns></returns>
        public static IServiceCollection ConfigureDefaultAuthService(
            this IServiceCollection services)
        {
            ServiceProvider serviceProvider = services.BuildServiceProvider();

            IJwtSettings jwt = serviceProvider.GetRequiredService<IJwtSettings>();
            IGeneralSettings general = serviceProvider.GetRequiredService<IGeneralSettings>();

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.SaveToken = true;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(general.DefaultEncoding.GetBytes(jwt.IssuerSigningKey)),
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            RequireExpirationTime = true,
                            ValidateLifetime = true,
                            ClockSkew = TimeSpan.Zero,
                            ValidIssuer = jwt.ValidIssuer,
                            ValidAudience = jwt.ValidAudience
                        };
                    });

            return services;
        }

        /// <summary>
        /// Extensão para configurar Swagger
        /// </summary>
        /// <param name="services">Auto referencia para a coleção de seviços</param>
        /// <param name="applicationName">Nome da aplicação atual</param>
        public static void ConfigureDefaultSwaggerService(
            this IServiceCollection services,
            string applicationName)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(applicationName, new Microsoft.OpenApi.Models.OpenApiInfo { Title = applicationName, Version = applicationName });

                string[] xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml");
                foreach (string item in xmlFiles)
                {
                    options.IncludeXmlComments(item, true);
                }

                options.CustomSchemaIds(i => i.FullName);
            });
        }

        /// <summary>
        /// Aplica as migrations pendentes para a aplicação
        /// </summary>
        /// <param name="services">Serviços</param>
        /// <returns>Serviços</returns>
        public static IServiceCollection ConfigureDefaultDatabaseMigrationsService(
            this IServiceCollection services)
        {
            ServiceProvider serviceProvider = services.BuildServiceProvider();
            IApplicationHealthService applicationHealthService = serviceProvider.GetRequiredService<IApplicationHealthService>();

            applicationHealthService.ApplyDatabaseMigrationsAsync();

            return services;
        }

        /// <summary>
        /// Configurações para versionamento das urls
        /// </summary>
        /// <param name="services">Serviços</param>
        /// <returns>Serviços</returns>
        public static IServiceCollection ConfigureDefaultApiVersioning(
            this IServiceCollection services)
        {
            services
                .AddApiVersioning(config =>
                {
                    config.DefaultApiVersion = new ApiVersion(1, 0);
                    config.AssumeDefaultVersionWhenUnspecified = true;
                    config.ReportApiVersions = true;
                });

            return services;
        }
    }
}
