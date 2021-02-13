/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Common.GeneralSettings.Impl.Default
{
    using System;
    using System.Text;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Configurações gerais da aplicação
    /// </summary>
    public class GeneralSettings : IGeneralSettings
    {
        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Data e hora atual (UTC)
        /// </summary>
        public DateTime CurrentDateTime => DateTime.UtcNow;

        /// <summary>
        /// Encoding padrão
        /// </summary>
        public Encoding DefaultEncoding => Encoding.UTF8;

        /// <summary>
        /// Construtor padrão
        /// </summary>
        /// <param name="logger">Logger</param>
        public GeneralSettings(
            ILogger<GeneralSettings> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Uri base da aplicação
        /// </summary>
        public Uri BaseUri
        {
            get
            {
                try
                {
                    string uri = Environment.GetEnvironmentVariable(EnvironmentVariable.BaseUri);

                    if (!string.IsNullOrEmpty(uri))
                    {
                        return new Uri(uri);
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
}
