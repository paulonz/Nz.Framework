/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Libs.Encryption
{
    /// <summary>
    /// Interface de criptografia
    /// </summary>
    public interface IEncryption
    {
        /// <summary>
        /// Faz a criptografia de uma string
        /// </summary>
        /// <param name="value">Valor para ser criptografado</param>
        /// <returns>Valor criptografado</returns>
        string Encrypt(
            string value);
    }
}
