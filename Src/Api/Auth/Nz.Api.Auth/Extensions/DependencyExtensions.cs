/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Api.Auth.Extensions
{
    using Microsoft.Extensions.DependencyInjection;
    using Nz.Core.Business.Impl.Auth;
    using Nz.Core.DatabaseContext;
    using Nz.Core.DatabaseContext.Impl.Auth;
    using Nz.Core.Service.Impl.Auth;

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
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IManageUsersService, ManageUsersService>();
            services.AddScoped<IMeService, MeService>();
            services.AddScoped<IUserService, UserService>();

            // Camada Business
            services.AddScoped<IAuthBusiness, AuthBusiness>();
            services.AddScoped<IMeBusiness, MeBusiness>();
            services.AddScoped<IUserBusiness, UserBusiness>();

            // Camada Data
            services.AddScoped<IDbContextSettings, DbContextSettings>();
            services.AddScoped<IDbContext, PrincipalContext>();


            return services;
        }
    }
}
