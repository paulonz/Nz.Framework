/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Libs.MessageTemplate.Impl.MessageResource
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common.Helpers;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Templates de mensagem
    /// </summary>
    public class MessageTemplate : IMessageTemplate
    {
        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Helpers para enum
        /// </summary>
        private readonly IEnumHelpers _enumHelpers;

        /// <summary>
        /// Construtor padrão
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="enumHelpers">Helpers para enum</param>
        public MessageTemplate(
            ILogger<MessageTemplate> logger,
            IEnumHelpers enumHelpers)
        {
            _logger = logger;
            _enumHelpers = enumHelpers;
        }

        /// <summary>
        /// Geração do template para um tipo de mensagem
        /// </summary>
        /// <param name="messageTemplateType">Tipo de template para mensagem</param>
        /// <param name="data">Dados para substituição</param>
        /// <returns>String html</returns>
        public string GetTemplate(
            MessageTemplateType messageTemplateType,
            dynamic data)
        {
            try
            {
                string messageTemplate = _enumHelpers.GetDisplay(messageTemplateType, Messages.ResourceManager);
                if (!string.IsNullOrEmpty(messageTemplate))
                {
                    return ReplaceData(messageTemplate, data);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return null;
        }

        /// <summary>
        /// Geração do template para um tipo de mensagem
        /// </summary>
        /// <param name="messageTemplateType">Tipo de template para mensagem</param>
        /// <param name="data">Dados para substituição</param>
        /// <returns>String html</returns>
        public string GetSubject(
            MessageTemplateType messageTemplateType,
            dynamic data)
        {
            try
            {
                string messageTemplate = _enumHelpers.GetDisplay(messageTemplateType, Subjects.ResourceManager);
                if (!string.IsNullOrEmpty(messageTemplate))
                {
                    return ReplaceData(messageTemplate, data);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return null;
        }

        /// <summary>
        /// Substitui as entradas de messageTemplate pelos dados recebidos em data
        /// </summary>
        /// <param name="messageTemplate">Template da mensagem</param>
        /// <param name="data">Dados para substituição</param>
        /// <returns>Mensagem formatada</returns>
        private string ReplaceData(
            string messageTemplate,
            dynamic data)
        {
            try
            {
                Dictionary<string, string> values = ExtractData(data);

                if (values != null && values.Any())
                {
                    foreach (KeyValuePair<string, string> item in values)
                    {
                        messageTemplate = messageTemplate.Replace($"{{{item.Key}}}", item.Value);
                    }
                }

                return messageTemplate;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return null;
        }

        /// <summary>
        /// Converte um objeto dynamic em uma lista de chave/valor
        /// </summary>
        /// <param name="data">Dados dynamic</param>
        /// <returns>Lista de chave/valor</returns>
        private Dictionary<string, string> ExtractData(
            dynamic data)
        {
            try
            {
                JObject jObject = (JObject)JToken.FromObject(data);

                if (jObject != null)
                {
                    Dictionary<string, string> result = jObject.ToObject<Dictionary<string, string>>();

                    if (result != null && result.Any())
                    {
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return null;
        }
    }
}
