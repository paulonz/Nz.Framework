/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Core.DatabaseContext.Impl.Auth
{
    /// <summary>
    /// Configurações de banco de dados (local, para ser utilizado na criação de migrations)
    /// </summary>
    internal class DbContextSettingsLocal : IDbContextSettings
    {
        /// <summary>
        /// String de conexão com o banco de dados principal
        /// </summary>
        public string DefaultConnectionString => EnvironmentVariable.ForMigrationsConnectionString;
    }
}
