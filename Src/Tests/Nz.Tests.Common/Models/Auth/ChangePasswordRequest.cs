/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Tests.Common.Models.Auth
{
    /// <summary>
    /// Mock do request para alteração da senha de usuário
    /// </summary>
    public class ChangePasswordRequest
    {
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Senha atual
        /// </summary>
        public string CurrentPassword { get; set; }
        /// <summary>
        /// Nova senha
        /// </summary>
        public string NewPassword { get; set; }
    }
}
