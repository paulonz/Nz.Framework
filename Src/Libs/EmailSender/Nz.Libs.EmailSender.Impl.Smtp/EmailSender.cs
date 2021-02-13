/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Libs.EmailSender.Impl.Smtp
{
    using System;
    using System.Net;
    using System.Net.Mail;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Envios de email
    /// </summary>
    public class EmailSender : IEmailSender
    {
        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Configurações para envio de email Smtp
        /// </summary>
        private readonly IEmailSenderSettings _emailSenderSettings;

        /// <summary>
        /// Construtor padrão
        /// </summary>
        /// <param name="emailSenderSettings">Configurações para envio de email Smtp</param>
        /// <param name="logger">Logger</param>
        public EmailSender(
            IEmailSenderSettings emailSenderSettings,
            ILogger<EmailSender> logger)
        {
            try
            {
                _emailSenderSettings = emailSenderSettings;
                _logger = logger;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        /// <summary>
        /// Envio de um email para um destinatário
        /// </summary>
        /// <param name="to">Destinatário</param>
        /// <param name="subject">Assunto</param>
        /// <param name="body">Corpo html do email</param>
        /// <returns>Resultado da tarefa</returns>
        public async Task<bool> SendAsync(
            string to,
            string subject,
            string body)
        {
            try
            {
                if (IsValid(to, subject, body))
                {
                    MailMessage mailMessage = new MailMessage()
                    {
                        From = new MailAddress(_emailSenderSettings.FromEmail, _emailSenderSettings.FromName),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true,
                        Priority = MailPriority.High
                    };

                    mailMessage.To.Add(new MailAddress(to));

                    using SmtpClient smtpClient = new SmtpClient(_emailSenderSettings.SmtpHost, _emailSenderSettings.SmtpPort)
                    {
                        EnableSsl = true,
                        Credentials = new NetworkCredential(_emailSenderSettings.SmtpUser, _emailSenderSettings.SmtpPassword)
                    };

                    await smtpClient.SendMailAsync(mailMessage);

                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return false;
        }

        /// <summary>
        /// Validação dos parametros para envio de um email
        /// </summary>
        /// <param name="to">Destinatário</param>
        /// <param name="subject">Assunto</param>
        /// <param name="body">Corpo html do email</param>
        /// <returns>Parametros ok?</returns>
        private bool IsValid(
            string to,
            string subject,
            string body)
        {
            bool isValid = true;

            if (string.IsNullOrEmpty(to))
            {
                _logger.LogError($"Trying to send an email without '{nameof(to)}' parameter");
                isValid = false;
            }
            else
            {
                try
                {
                    MailAddress email = new MailAddress(to);

                    if (email.Address != to)
                    {
                        _logger.LogError($"Trying to send an email with invalid '{nameof(to)}' parameter: '{to}'");
                        isValid = false;
                    }
                }
                catch
                {
                    _logger.LogError($"Trying to send an email with invalid '{nameof(to)}' parameter: '{to}'");
                    isValid = false;
                }
            }

            if (string.IsNullOrEmpty(subject))
            {
                _logger.LogError($"Trying to send an email without '{nameof(subject)}' parameter");
                isValid = false;
            }

            if (string.IsNullOrEmpty(body))
            {
                _logger.LogError($"Trying to send an email without '{nameof(body)}' parameter");
                isValid = false;
            }

            return isValid;
        }
    }
}
