/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Api
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Base para Program
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Nome da aplicação
        /// </summary>
        private string ApplicationName { get; set; }

        /// <summary>
        /// Construtor padrão
        /// </summary>
        /// <param name="applicationName">Nome da aplicação</param>
        public Program(string applicationName)
        {
            ApplicationName = applicationName;
        }

        /// <summary>
        /// Método inicial da aplicação
        /// </summary>
        /// <param name="args">Parametros de inicialização</param>
        /// <param name="startupFactory">Startup da aplicação</param>
        protected async Task DoAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)] TStartup>(
            string[] args,
            Func<WebHostBuilderContext, TStartup> startupFactory)
            where TStartup : class
        {
            ILogger<Program> logger = CreateLogger<Program>();

            try
            {
                logger.LogInformation($"Application {ApplicationName} starting");

                await CreateHostBuilder(args, startupFactory)
                       .Build()
                       .RunAsync()
                       .ConfigureAwait(false);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);

                throw;
            }

            logger.LogInformation($"Application {ApplicationName} stopped");
        }

        /// <summary>
        /// Criação do host para a aplicação
        /// </summary>
        /// <param name="args">Parametros de inicialização</param>
        /// <param name="startupFactory">Startup da aplicação</param>
        /// <returns>Host para aplicação</returns>
        private IHostBuilder CreateHostBuilder<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)] TStartup>(
            string[] args,
            Func<WebHostBuilderContext, TStartup> startupFactory)
            where TStartup : class
        {
            return Host.CreateDefaultBuilder(args)
                        .ConfigureLogging(CreateLoggerSettings())
                        .ConfigureWebHostDefaults(builder =>
                        {
                            builder.UseStartup(startupFactory);
                        });

        }

        /// <summary>
        /// Configurações de Logger
        /// </summary>
        /// <returns>Configurações de Logger</returns>
        private Action<ILoggingBuilder> CreateLoggerSettings()
        {
            return new Action<ILoggingBuilder>(logging =>
            {
                logging.ClearProviders();
                logging.AddEventLog(options =>
                {
                    options.SourceName = ApplicationName;
                });
                logging.AddSimpleConsole(options =>
                {
                    options.IncludeScopes = true;
                    options.UseUtcTimestamp = true;
                });
                logging.AddDebug();
            });
        }

        /// <summary>
        /// Logger para Startup
        /// </summary>
        /// <returns>Log para Startup</returns>
        public ILogger<Y> CreateLogger<Y>()
            where Y : class
        {
            return LoggerFactory
                        .Create(CreateLoggerSettings())
                        .CreateLogger<Y>();
        }

        /// <summary>
        /// FAKE
        /// </summary>
        public static void Main() { }
    }
}
