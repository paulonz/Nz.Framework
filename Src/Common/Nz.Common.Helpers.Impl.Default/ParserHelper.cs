/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Common.Helpers.Impl.Default
{
    using System;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    /// <summary>
    /// Helper para converção de objetos
    /// </summary>
    public class ParserHelper : IParserHelper
    {
        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Construtor padrão
        /// </summary>
        /// <param name="logger">Logger</param>
        public ParserHelper(
            ILogger<ParserHelper> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Converte um objeto para uma string Json
        /// </summary>
        /// <typeparam name="T">Tipo do objeto</typeparam>
        /// <param name="model">Objeto para conversão</param>
        /// <returns>String Json</returns>
        public string ToJson<T>(
            T model) where T : class
        {
            try
            {
                return JsonConvert.SerializeObject(
                    model,
                    Formatting.None,
                    GetJsonSerializerSettings());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return null;
        }

        /// <summary>
        /// Converte uma string Json em um objeto
        /// </summary>
        /// <typeparam name="T">Tipo do objeto</typeparam>
        /// <param name="json">String para conversão</param>
        /// <returns>Objeto convertido</returns>
        public T FromJson<T>(
            string json) where T : class
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(
                    json,
                    GetJsonSerializerSettings());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return null;
        }

        /// <summary>
        /// Converte de um objeto para outro
        /// </summary>
        /// <typeparam name="Target">Tipo do objeto de origem</typeparam>
        /// <typeparam name="From">Tipo do objeto de destino</typeparam>
        /// <param name="model">Objeto que será convertido</param>
        /// <returns>Objeto convertido</returns>
        public Target To<Target, From>(From model)
            where Target : class
            where From : class
        {
            try
            {
                string json = ToJson(model);

                return FromJson<Target>(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return null;
        }

        /// <summary>
        /// Configurações para serialização Json
        /// </summary>
        /// <returns>Configurações para serialização Json</returns>
        private static JsonSerializerSettings GetJsonSerializerSettings()
        {
            return new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
        }
    }
}
