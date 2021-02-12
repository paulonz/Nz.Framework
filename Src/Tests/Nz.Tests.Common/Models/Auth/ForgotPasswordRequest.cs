/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Tests.Common.Models.Auth
{
    /// <summary>
    /// Recuperação de senha de um usuário
    /// </summary>
    public class ForgotPasswordRequest
    {
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
    }
}
