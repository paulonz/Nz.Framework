/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Api.ViewModel.Auth
{
    /// <summary>
    /// Tokens de autenticação
    /// </summary>
    public class AuthResponse : IResponseViewModel
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
