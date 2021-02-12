/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Api.Announcement.Extensions
{
    using Microsoft.Extensions.DependencyInjection;
    using Nz.Core.Business.Impl.Announcement;
    using Nz.Core.DatabaseContext;
    using Nz.Core.DatabaseContext.Impl.Announcement;
    using Nz.Core.Service.Impl.Announcement;

    /// <summary>
    /// Extensões para configuração de dependências
    /// </summary>
    internal static class DependencyExtensions
    {
        /// <summary>
        /// Extensão para configurar dependencias
        /// </summary>
        /// <param name="services">Serviços</param>
        /// <returns>Serviços</returns>
        internal static IServiceCollection ConfigureLocalDependenciesService(
            this IServiceCollection services)
        {
            // Camada Service
            services.AddScoped<IMyAnnouncementService, MyAnnouncementService>();
            services.AddScoped<IManageAnnouncementsService, ManageAnnouncementsService>();

            // Camada Business
            services.AddScoped<IMyAnnouncementBusiness, MyAnnouncementBusiness>();
            services.AddScoped<IManageAnnouncementsBusiness, ManageAnnouncementsBusiness>();

            // Camada Data
            services.AddScoped<IDbContextSettings, DbContextSettings>();
            services.AddScoped<IDbContext, PrincipalContext>();


            return services;
        }
    }
}
