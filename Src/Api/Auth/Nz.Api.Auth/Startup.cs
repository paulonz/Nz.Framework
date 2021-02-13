/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Api.Auth
{
    using System;
    using Extensions;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Nz.Api.Extensions;

    /// <summary>
    /// Configurações para incialização da aplicação
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Lista key/value de configurações
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="logger">Logger</param>
        public Startup(
            ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Executa a configuração de serviços da aplicação
        /// </summary>
        /// <param name="services">Coleção de serviços</param>
        public void ConfigureServices(
            IServiceCollection services)
        {
            using (_logger.BeginScope($"{GetType().FullName}.{nameof(ConfigureServices)}"))
            {
                try
                {
                    _logger.LogInformation($"Starting");

                    services
                        .ConfigureDefaultCompressionService()
                        .ConfigureDefaultDependenciesService()
                        .ConfigureLocalDependenciesService()
                        .ConfigureDefaultDatabaseMigrationsService()
                        .ConfigureDefaultMvcService()
                        .ConfigureDefaultAuthService()
                        .ConfigureDefaultCorsService()
                        .ConfigureDefaultApiVersioning()
                        .ConfigureDefaultSwaggerService(Program.ApplicationName);

                    _logger.LogInformation($"Done");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);

                    throw;
                }
            }
        }

        /// <summary>
        /// Configuração de aplicação e ambiente
        /// </summary>
        /// <param name="app">Aplicação</param>
        public void Configure(
            IApplicationBuilder app)
        {
            using (_logger.BeginScope($"{GetType().FullName}.{nameof(Configure)}"))
            {
                try
                {
                    _logger.LogInformation($"Starting");

                    app
                        .ConfigureDefaultMiddlewares()
                        .ConfigureDefaultCompression()
                        .ConfigureDefaultExceptionHandler()
                        .ConfigureDefaultRequestLocalization()
                        .ConfigureDefaultRoutes()
                        .ConfigureDefaultSwagger(Program.ApplicationName);

                    _logger.LogInformation($"Done");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);

                    throw;
                }
            }
        }
    }
}
