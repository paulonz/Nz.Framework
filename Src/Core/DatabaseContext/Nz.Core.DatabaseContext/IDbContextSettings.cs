/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Core.DatabaseContext
{
    /// <summary>
    /// Configurações de banco de dados
    /// </summary>
    public interface IDbContextSettings
    {
        /// <summary>
        /// String de conexão com o banco de dados principal
        /// </summary>
        string DefaultConnectionString { get; }
    }
}
