/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Core.DatabaseContext
{
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Representação de um contexto de banco de dados
    /// </summary>
    public interface IDbContext
    {
        /// <summary>
        /// Implementação do contexto de banco de dados
        /// </summary>
        DbContext CurrentDbContext { get; }
    }
}
