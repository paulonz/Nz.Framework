/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Core.Business.Impl.Default
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Nz.Core.UnitOfWork;

    /// <summary>
    /// Classe padrão de negócios
    /// </summary>
    /// <typeparam name="T">Tipo da model para manipulação</typeparam>
    public class ApplicationHealthBusiness : IApplicationHealthBusiness
    {
        /// <summary>
        /// Implementação de IUnitOfWork
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Construtor padrão
        /// </summary>
        /// <param name="unitOfWork">Implementação de IUnitOfWork</param>
        public ApplicationHealthBusiness(
            IUnitOfWork unitOfWork,
            ILogger<ApplicationHealthBusiness> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        /// <summary>
        /// Aplica as migrações pendentes para o banco de dados
        /// </summary>
        /// <returns>Ok</returns>
        public async Task ApplyDatabaseMigrationsAsync()
        {
            try
            {
                await _unitOfWork.ApplyMigrationsAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        /// <summary>
        /// Checagem da saúde da aplicação
        /// </summary>
        /// <returns>Ok</returns>
        public async Task<bool> HealthCheckAsync()
        {
            return await _unitOfWork.HealthCheckAsync().ConfigureAwait(false);
        }
    }
}
