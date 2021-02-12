/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Libs.MessageTemplate
{
    /// <summary>
    /// Interface para templates de mensagens
    /// </summary>
    public interface IMessageTemplate
    {
        /// <summary>
        /// Geração do template para um tipo de mensagem
        /// </summary>
        /// <param name="messageTemplateType">Tipo de template para mensagem</param>
        /// <param name="data">Dados para substituição</param>
        /// <returns>String html</returns>
        string GetTemplate(
            MessageTemplateType messageTemplateType,
            dynamic data);

        /// <summary>
        /// Geração do título para um tipo de mensagem
        /// </summary>
        /// <param name="messageTemplateType">Tipo de template para mensagem</param>
        /// <param name="data">Dados para substituição</param>
        /// <returns>Título</returns>
        string GetSubject(
            MessageTemplateType messageTemplateType,
            dynamic data);
    }
}
