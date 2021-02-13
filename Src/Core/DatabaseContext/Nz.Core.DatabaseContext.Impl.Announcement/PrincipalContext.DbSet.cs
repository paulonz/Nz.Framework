/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Core.DatabaseContext.Impl.Announcement
{
    using Microsoft.EntityFrameworkCore;
    using Nz.Core.DatabaseContext;
    using Nz.Core.Model.Impl.Announcement;

    /// <summary>
    /// Contexto principal
    /// </summary>
    public partial class PrincipalContext : DbContext, IDbContext
    {
        /// <summary>
        /// DbSet Announcement
        /// </summary>
        public virtual DbSet<Announcement> Announcement { get; set; }
    }
}
