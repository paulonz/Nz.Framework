/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Tests.Common.Models.Auth
{
    /// <summary>
    /// Mock do request para reenvio do email de confirmação
    /// </summary>
    public class ResendConfirmationCodeRequest
    {
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
    }
}
