/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Libs.EmailSender
{
    /// <summary>
    /// Configurações relacionados ao envio de emails
    /// </summary>
    public interface IEmailSenderSettings
    {
        /// <summary>
        /// Email do remetente
        /// </summary>
        string FromEmail { get; }

        /// <summary>
        /// Nome do remetente
        /// </summary>
        string FromName { get; }

        /// <summary>
        /// Servidor Smtp
        /// </summary>
        string SmtpHost { get; }

        /// <summary>
        /// Porta do servidor Smtp
        /// </summary>
        int SmtpPort { get; }

        /// <summary>
        /// Usuário Smtp
        /// </summary>
        string SmtpUser { get; }

        /// <summary>
        /// Senha do usuário Smtp
        /// </summary>
        string SmtpPassword { get; }
    }
}
