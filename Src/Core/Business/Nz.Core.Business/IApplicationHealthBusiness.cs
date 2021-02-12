/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Core.Business
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interface para saúde da aplicação
    /// </summary>
    public interface IApplicationHealthBusiness
    {
        /// <summary>
        /// Aplica as migrations pendentes para o banco de dados
        /// </summary>
        /// <returns>Ok</returns>
        Task ApplyDatabaseMigrationsAsync();

        /// <summary>
        /// Checagem da saúde da aplicação
        /// </summary>
        /// <returns>Ok</returns>
        Task<bool> HealthCheckAsync();
    }
}
