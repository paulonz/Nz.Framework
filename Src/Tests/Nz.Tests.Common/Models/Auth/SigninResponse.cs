/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Tests.Common.Models.Auth
{
    /// <summary>
    /// Mock do response para login de usuário
    /// </summary>
    public class SigninResponse
    {
        /// <summary>
        /// Token de autenticação
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// Token para refresh da autenticação
        /// </summary>
        public string RefreshToken { get; set; }
    }
}
