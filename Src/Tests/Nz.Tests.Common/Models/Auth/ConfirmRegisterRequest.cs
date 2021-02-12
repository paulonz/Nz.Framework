/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Tests.Common.Models.Auth
{
    /// <summary>
    /// Mock do request para confirmar o registro de um usuário
    /// </summary>
    public class ConfirmRegisterRequest
    {
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Token para confirmação do registro
        /// </summary>
        public string ConfirmRegisterToken { get; set; }
    }
}
