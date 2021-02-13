/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Libs.EmailSender
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interface para envios de email
    /// </summary>
    public interface IEmailSender
    {
        /// <summary>
        /// Envio de um email para um destinatário
        /// </summary>
        /// <param name="to">Destinatário</param>
        /// <param name="subject">Assunto</param>
        /// <param name="body">Corpo html do email</param>
        /// <returns>Resultado da tarefa</returns>
        Task<bool> SendAsync(
            string to,
            string subject,
            string body);
    }
}
