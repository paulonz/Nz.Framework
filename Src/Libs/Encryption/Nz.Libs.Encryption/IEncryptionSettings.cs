/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Libs.Encryption
{
    using System.Security.Cryptography;

    /// <summary>
    /// Configurações relacionados a segurança
    /// </summary>
    public interface IEncryptionSettings
    {
        /// <summary>
        /// Algoritmo de criptografia
        /// </summary>
        HashAlgorithm HashAlgorithm { get; }
    }
}
