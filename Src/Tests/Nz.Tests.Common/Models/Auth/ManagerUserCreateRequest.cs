/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Tests.Common.Models.Auth
{
    /// <summary>
    /// Mock do request para registrar um usuário
    /// </summary>
    public class ManagerUserCreateRequest
    {
        /// <summary>
        /// Primeiro nome
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Sobrenome
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Senha
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Número telefônico completo +ddi
        /// </summary>
        public string Phone { get; set; }
    }
}
