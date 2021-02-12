/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Libs.Encryption.Impl.HashAlgorithm
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using Microsoft.Extensions.Logging;
    using Nz.Common.GeneralSettings;

    /// <summary>
    /// Implementação da criptografia
    /// </summary>
    public class Encryption : IEncryption
    {
        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Configurações de segurança
        /// </summary>
        private readonly IEncryptionSettings _security;

        /// <summary>
        /// Configurações gerais
        /// </summary>
        private readonly IGeneralSettings _general;

        /// <summary>
        /// Construtor padrão
        /// </summary>
        /// <param name="security">Configurações de segurança</param>
        /// <param name="general">Configurações gerais</param>
        /// <param name="logger">Logger</param>
        public Encryption(
            IEncryptionSettings security,
            IGeneralSettings general,
            ILogger<Encryption> logger)
        {
            _security = security;
            _general = general;
            _logger = logger;
        }

        /// <summary>
        /// Faz a criptografia de uma string
        /// </summary>
        /// <param name="value">Valor para ser criptografado</param>
        /// <returns>Valor criptografado</returns>
        public string Encrypt(
            string value)
        {
            try
            {
                HashAlgorithm hashAlgorithm = _security.HashAlgorithm;

                byte[] encodedValue = _general.DefaultEncoding.GetBytes(value);
                byte[] encryptedPassword = hashAlgorithm.ComputeHash(encodedValue);

                StringBuilder stringBuilder = new StringBuilder();
                foreach (byte character in encryptedPassword)
                {
                    stringBuilder.Append(character.ToString("X2"));
                }

                return stringBuilder.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return null;
        }
    }
}
