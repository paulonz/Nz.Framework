/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Api.Extensions
{
    using Microsoft.Extensions.DependencyInjection;
    using Nz.Common.GeneralSettings;
    using Nz.Common.GeneralSettings.Impl.Default;
    using Nz.Common.Helpers;
    using Nz.Common.Helpers.Impl.Default;
    using Nz.Core.Business;
    using Nz.Core.Business.Impl.Default;
    using Nz.Core.Model.Impl.Jwt;
    using Nz.Core.Service;
    using Nz.Core.Service.Impl.Default;
    using Nz.Core.UnitOfWork;
    using Nz.Core.UnitOfWork.Impl.Postgresql;
    using Nz.Libs.EmailSender;
    using Nz.Libs.EmailSender.Impl.Smtp;
    using Nz.Libs.EmailSender.Impl.Smtp.Settings.Default;
    using Nz.Libs.Encryption;
    using Nz.Libs.Encryption.Impl.HashAlgorithm;
    using Nz.Libs.Jwt.Settings;
    using Nz.Libs.Jwt.Settings.Impl.Default;
    using Nz.Libs.MessageTemplate;
    using Nz.Libs.MessageTemplate.Impl.MessageResource;
    using Nz.Libs.RestPagination;

    /// <summary>
    /// Extensões para configuração de dependências
    /// </summary>
    public static class DependencyExtensions
    {
        /// <summary>
        /// Extensão para configurar dependencias
        /// </summary>
        /// <param name="services">Serviços</param>
        /// <returns>Serviços</returns>
        public static IServiceCollection ConfigureDefaultDependenciesService(
            this IServiceCollection services)
        {
            /*
                Objetos Transient são sempre diferentes; uma nova instância é fornecida para todos os controladores e todos os serviços.
                Objetos Scoped são os mesmos em uma solicitação, mas diferentes entre solicitações diferentes.
                Objetos Singleton são os mesmos para todos os objects e todos os pedidos.
             */

            // Geral
            services.AddHttpContextAccessor();
            services.AddScoped<EnablePagingAttribute>();

            // Helpers
            services.AddSingleton<IEnumHelpers, EnumHelpers>();
            services.AddSingleton<IParserHelper, ParserHelper>();
            services.AddSingleton<IResourceHelper, ResourceHelper>();

            // Settings
            services.AddSingleton<IGeneralSettings, GeneralSettings>();
            services.AddSingleton<IJwtSettings, JwtSettings>();
            services.AddSingleton<IEncryptionSettings, EncryptionSettings>();
            services.AddSingleton<IEmailSenderSettings, EmailSenderSettings>();

            // Segurança
            services.AddSingleton<IEncryption, Encryption>();

            // Mensagem
            services.AddSingleton<IEmailSender, EmailSender>();
            services.AddSingleton<IMessageTemplate, MessageTemplate>();

            // Camada Service
            services.AddScoped<IApplicationHealthService, ApplicationHealthService>();

            // Camada Business
            services.AddScoped<IApplicationHealthBusiness, ApplicationHealthBusiness>();

            // Camada Data
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Camada Model
            services.AddScoped<Core.Model.IAuthUser, AuthUser>();

            return services;
        }
    }
}
