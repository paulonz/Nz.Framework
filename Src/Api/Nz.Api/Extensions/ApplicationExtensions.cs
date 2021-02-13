/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Api.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Localization;
    using Microsoft.Extensions.DependencyInjection;
    using Nz.Common.Helpers;

    /// <summary>
    /// Configurações da aplicação
    /// </summary>
    public static class ApplicationExtensions
    {
        /// <summary>
        /// Extensão para configurar compressão de response
        /// </summary>
        /// <param name="app">Applicação</param>
        /// <returns>Applicação</returns>
        public static IApplicationBuilder ConfigureDefaultCompression(
            this IApplicationBuilder app)
        {
            app
                .UseResponseCompression();

            return app;
        }

        /// <summary>
        /// Extensão que configura Localization em requests
        /// </summary>
        /// <param name="app">Aplicação</param>
        public static IApplicationBuilder ConfigureDefaultRequestLocalization(
            this IApplicationBuilder app)
        {
            app
                .UseRequestLocalization(BuildDefaultLocalizationOptions());

            return app;
        }

        /// <summary>
        /// Extensão que configura o serviço de rotas da api
        /// </summary>
        /// <param name="app">Aplicação</param>
        public static IApplicationBuilder ConfigureDefaultRoutes(
            this IApplicationBuilder app)
        {
            app
                .UseHttpsRedirection()
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization()
                .UseEndpoints(config =>
                {
                    config.MapControllers();
                })
                .UseStaticFiles();

            return app;
        }

        /// <summary>
        /// Gera as configurações de Localização (i18n)
        /// </summary>
        /// <returns>Configuração de localização</returns>
        public static RequestLocalizationOptions BuildDefaultLocalizationOptions()
        {
            List<CultureInfo> supportedCultures = new List<CultureInfo>
            {
                new CultureInfo("en"),
                new CultureInfo("pt")
            };

            RequestLocalizationOptions options = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            };

            return options;
        }

        /// <summary>
        /// Extensão que configura o Handler para tratamento de erros
        /// </summary>
        /// <param name="app">Applicação</param>
        public static IApplicationBuilder ConfigureDefaultExceptionHandler(
            this IApplicationBuilder app)
        {
            IParserHelper parserHelper = app.ApplicationServices.GetRequiredService<IParserHelper>();

            app
                .UseMiddleware<Middleware.UnhandledExceptionMiddleware>()
                .UseExceptionHandler(config =>
                {
                    config.Run(async context =>
                    {
                        IExceptionHandlerFeature error = context.Features.Get<IExceptionHandlerFeature>();
                        if (error != null)
                        {
                            context.Response.StatusCode = error.Error switch
                            {
                                UnauthorizedAccessException unauthorizedAccessException => StatusCodes.Status401Unauthorized,
                                _ => StatusCodes.Status400BadRequest,
                            };

                            if (context.Response.StatusCode != StatusCodes.Status401Unauthorized)
                            {
                                context.Response.ContentType = "application/json";

                                ViewModel.ApiErrorResponse apiError = new ViewModel.ApiErrorResponse();
                                apiError.Errors.Add(new ViewModel.ApiErrorDetailResponse());

                                await context.Response.WriteAsync(parserHelper.ToJson(apiError)).ConfigureAwait(false);
                            }
                        }
                    });
                })
                .UseHsts();

            return app;
        }

        /// <summary>
        /// Configurações dos Midlleware da aplicação
        /// </summary>
        /// <param name="app">Aplicação</param>
        /// <returns>Aplicação</returns>
        public static IApplicationBuilder ConfigureDefaultMiddlewares(
            this IApplicationBuilder app)
        {
            app
                .UseMiddleware<Middleware.HttpRequestBodyMiddleware>();

            return app;
        }

        /// <summary>
        /// Extensão para configurar a interface web do Swagger
        /// </summary>
        /// <param name="app">Applicação</param>
        /// <param name="applicationName">Nome da aplicação atual</param>
        public static void ConfigureDefaultSwagger(
            this IApplicationBuilder app,
            string applicationName)
        {
            app
                .UseSwagger()
                .UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint($"/swagger/{applicationName}/swagger.json", applicationName);
                    options.RoutePrefix = string.Empty;
                });
        }
    }
}
