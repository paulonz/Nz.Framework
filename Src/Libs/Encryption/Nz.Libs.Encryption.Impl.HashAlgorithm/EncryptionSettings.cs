/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Libs.Encryption.Impl.HashAlgorithm
{
    using System;
    using System.Security.Cryptography;
    using Microsoft.Extensions.Logging;
    using Nz.Libs.Encryption;

    /// <summary>
    /// Configurações relacionadas a segurança
    /// </summary>
    public class EncryptionSettings : IEncryptionSettings
    {
        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Construtor padrão
        /// </summary>
        /// <param name="logger">Logger</param>
        public EncryptionSettings(
            ILogger<EncryptionSettings> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Algoritmo de criptografia
        /// </summary>
        public HashAlgorithm HashAlgorithm
        {
            get
            {
                try
                {
                    return SHA512.Create();
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
