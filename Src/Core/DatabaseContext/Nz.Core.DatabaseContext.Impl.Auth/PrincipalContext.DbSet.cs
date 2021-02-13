/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Core.DatabaseContext.Impl.Auth
{
    using Microsoft.EntityFrameworkCore;
    using Nz.Core.DatabaseContext;
    using Nz.Core.Model.Impl.Auth;

    /// <summary>
    /// Contexto principal
    /// </summary>
    public partial class PrincipalContext : DbContext, IDbContext
    {
        /// <summary>
        /// DbSet User
        /// </summary>
        public virtual DbSet<User> User { get; set; }

        /// <summary>
        /// DbSet de regras de um usuário
        /// </summary>
        public virtual DbSet<UserRole> UserRole { get; set; }
    }
}
