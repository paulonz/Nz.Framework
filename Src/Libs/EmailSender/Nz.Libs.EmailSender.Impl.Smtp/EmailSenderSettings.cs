/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Libs.EmailSender.Impl.Smtp
{
    using System;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Configurações relacionados ao envio de email
    /// </summary>
    public class EmailSenderSettings : IEmailSenderSettings
    {
        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Construtor padrão
        /// </summary>
        /// <param name="logger">Logger</param>
        public EmailSenderSettings(
            ILogger<EmailSenderSettings> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Email do remetente
        /// </summary>
        public string FromEmail
        {
            get
            {
                try
                {
                    return Environment.GetEnvironmentVariable(EnvironmentVariable.FromEmail);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }

                return null;
            }
        }

        /// <summary>
        /// Nome do remetente
        /// </summary>
        public string FromName
        {
            get
            {
                try
                {
                    return Environment.GetEnvironmentVariable(EnvironmentVariable.FromName);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }

                return null;
            }
        }

        /// <summary>
        /// Servidor Smtp
        /// </summary>
        public string SmtpHost
        {
            get
            {
                try
                {
                    return Environment.GetEnvironmentVariable(EnvironmentVariable.SmtpHost);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }

                return null;
            }
        }

        /// <summary>
        /// Porta do servidor Smtp
        /// </summary>
        public int SmtpPort
        {
            get
            {
                try
                {
                    string port = Environment.GetEnvironmentVariable(EnvironmentVariable.SmtpPort);

                    if (int.TryParse(port, out int result))
                    {
                        return result;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }

                return 587;
            }
        }

        /// <summary>
        /// Usuário Smtp
        /// </summary>
        public string SmtpUser
        {
            get
            {
                try
                {
                    return Environment.GetEnvironmentVariable(EnvironmentVariable.SmtpUser);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }

                return null;
            }
        }

        /// <summary>
        /// Senha do usuário Smtp
        /// </summary>
        public string SmtpPassword
        {
            get
            {
                try
                {
                    return Environment.GetEnvironmentVariable(EnvironmentVariable.SmtpPassword);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }

                return null;
            }
        }
    }
}
