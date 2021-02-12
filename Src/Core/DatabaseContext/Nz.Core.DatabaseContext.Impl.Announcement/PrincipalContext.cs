/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Core.DatabaseContext.Impl.Announcement
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Nz.Core.DatabaseContext;

    /// <summary>
    /// Contexto principal
    /// </summary>
    public partial class PrincipalContext : DbContext, IDbContext
    {
        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Configurações de banco de dados
        /// </summary>
        private readonly IDbContextSettings _database;

#if DEBUG
        /// <summary>
        /// Construtor para migrations
        /// </summary>
        public PrincipalContext()
        {
            _database = new DbContextSettingsLocal();
            _logger = new LoggerLocal();
        }
#endif

        /// <summary>
        /// Construtor padrão
        /// </summary>
        /// <param name="database">Configurações de banco de dados</param>
        /// <param name="logger">Logger</param>
        public PrincipalContext(
            IDbContextSettings database,
            ILogger<PrincipalContext> logger)
        {
            _database = database;
            _logger = logger;
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.LazyLoadingEnabled = false;
        }

        public DbContext CurrentDbContext => this;

        /// <summary>
        /// Override das configurações
        /// </summary>
        /// <param name="optionsBuilder">Configurações</param>
        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
            try
            {
                if (optionsBuilder != null && !optionsBuilder.IsConfigured)
                {
                    optionsBuilder
                        .UseNpgsql(_database.DefaultConnectionString)
                        .UseSnakeCaseNamingConvention();
                    base.OnConfiguring(optionsBuilder);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        /// <summary>
        /// Definições para o momento de criação dos modelos no banco
        /// </summary>
        /// <param name="modelBuilder">Construtor de modelos</param>
        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            try
            {
                if (modelBuilder != null)
                {
                    modelBuilder.UseIdentityColumns();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}
