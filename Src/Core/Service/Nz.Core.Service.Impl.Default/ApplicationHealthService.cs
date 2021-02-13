/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Core.Service.Impl.Default
{
    using System.Threading.Tasks;
    using Nz.Core.Business;

    /// <summary>
    /// Serviço para saúde da aplicação
    /// </summary>
    public class ApplicationHealthService : IApplicationHealthService
    {
        /// <summary>
        /// Negócios
        /// </summary>
        private readonly IApplicationHealthBusiness _business;

        public ApplicationHealthService(
            IApplicationHealthBusiness business)
        {
            _business = business;
        }

        /// <summary>
        /// Aplica as migrações pendentes para o banco de dados
        /// </summary>
        /// <returns>Ok</returns>
        public async Task ApplyDatabaseMigrationsAsync()
        {
            await _business.ApplyDatabaseMigrationsAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Checagem da saúde da aplicação
        /// </summary>
        /// <returns>Ok</returns>
        public async Task<bool> HealthCheckAsync()
        {
            return await _business.HealthCheckAsync().ConfigureAwait(false);
        }
    }
}
