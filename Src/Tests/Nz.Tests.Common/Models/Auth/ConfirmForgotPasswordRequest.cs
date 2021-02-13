/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Tests.Common.Models.Auth
{
    /// <summary>
    /// Confirmação para recuperação de senha de um usuário
    /// </summary>
    public class ConfirmForgotPasswordRequest
    {
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Token para recuperação de senha
        /// </summary>
        public string RecoveryPasswordToken { get; set; }

        /// <summary>
        /// Nova senha
        /// </summary>
        public string NewPassword { get; set; }
    }
}
