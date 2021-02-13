/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Core.Business.Impl.Auth
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Nz.Common.GeneralSettings;
    using Nz.Core.Business.Impl.Default;
    using Nz.Core.Model.Impl.Auth;
    using Nz.Core.UnitOfWork;
    using Nz.Libs.EmailSender;
    using Nz.Libs.Encryption;
    using Nz.Libs.MessageTemplate;

    /// <summary>
    /// Classe para Usuários
    /// </summary>
    public class UserBusiness : CRUDBusiness<User>, IUserBusiness
    {
        /// <summary>
        /// Criptografia
        /// </summary>
        private readonly IEncryption _encryption;

        /// <summary>
        /// Implementação de IUnitOfWork
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Configurações gerais
        /// </summary>
        private readonly IGeneralSettings _general;

        /// <summary>
        /// Envio de email
        /// </summary>
        private readonly IEmailSender _emailSender;

        /// <summary>
        /// Template de mensagens
        /// </summary>
        private readonly IMessageTemplate _messageTemplate;

        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Construtor padrão
        /// </summary><param name="encryption">Criptografia de dados</param>
        /// <param name="unitOfWork">Implementação de IUnitOfWork</param>
        /// <param name="emailSender">Envio de emails</param>
        /// <param name="messageTemplate">Templates para email</param>
        /// <param name="generalSettings">Configurações gerais</param>
        /// <param name="logger">Logger</param>
        public UserBusiness(
            IEncryption encryption,
            IUnitOfWork unitOfWork,
            IEmailSender emailSender,
            IMessageTemplate messageTemplate,
            IGeneralSettings generalSettings,
            ILogger<CRUDBusiness<User>> logger)
            : base(unitOfWork, logger)
        {
            _encryption = encryption;
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
            _messageTemplate = messageTemplate;
            _general = generalSettings;
            _logger = logger;
        }

        /// <summary>
        /// Cria um novo objeto no repositório
        /// </summary>
        /// <param name="model">Novo objeto</param>
        public override async Task<User> CreateAsync(
            User model)
        {
            try
            {
                if (model != null)
                {
                    if (await EmailIsUniqueAsync(model.Email))
                    {
                        model.Password = _encryption.Encrypt(model.Password);

                        User user = await base.CreateAsync(model).ConfigureAwait(false);

                        if (user != null)
                        {
                            string message = _messageTemplate.GetTemplate(
                                    MessageTemplateType.EmailNewUserRegister,
                                    new
                                    {
                                        user.FirstName,
                                        ConfirmationToken = _encryption.Encrypt(user.Email),
                                        _general.BaseUri
                                    });

                            string subject = _messageTemplate.GetSubject(
                                                    MessageTemplateType.EmailNewUserRegister,
                                                    new
                                                    {
                                                        user.FirstName
                                                    });

                            await _emailSender.SendAsync(
                                        user.Email,
                                        subject,
                                        message).ConfigureAwait(false);

                            return user;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return null;
        }

        /// <summary>
        /// Verifica se já existe um usuário com o email informado
        /// </summary>
        /// <param name="email">Email de usuário</param>
        /// <returns>Existe?</returns>
        private async Task<bool> EmailIsUniqueAsync(
            string email)
        {
            User user = await _unitOfWork.ReadFirstAsync<User>($"{nameof(User.Email)} == \"{email}\"").ConfigureAwait(false);

            return user == null;
        }
    }
}
