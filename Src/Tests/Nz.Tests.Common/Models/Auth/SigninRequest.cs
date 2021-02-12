/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Tests.Common.Models.Auth
{
    /// <summary>
    /// Mock do request para login de usuário
    /// </summary>
    public class SigninRequest
    {
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Senha
        /// </summary>
        public string Password { get; set; }
    }
}
