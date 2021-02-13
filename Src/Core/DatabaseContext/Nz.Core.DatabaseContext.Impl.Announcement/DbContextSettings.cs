/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Core.DatabaseContext.Impl.Announcement
{
    using System;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Configurações de banco de dados
    /// </summary>
    public class DbContextSettings : IDbContextSettings
    {
        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Construtor padrão
        /// </summary>
        /// <param name="logger">Logger</param>
        public DbContextSettings(
            ILogger<DbContextSettings> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// String de conexão com o banco de dados principal
        /// </summary>
        public string DefaultConnectionString
        {
            get
            {
                try
                {
                    return Environment.GetEnvironmentVariable(EnvironmentVariable.ConnectionString);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }

                return null;
            }
        }
    }
}
