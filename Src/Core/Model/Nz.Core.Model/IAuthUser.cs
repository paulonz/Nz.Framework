/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Core.Model
{
    using System.Collections.Generic;

    /// <summary>
    /// Usuário autenticado
    /// </summary>
    public interface IAuthUser
    {
        /// <summary>
        /// Id do usuário
        /// </summary>
        long UserId { get; }

        /// <summary>
        /// Primeiro nome
        /// </summary>
        string FirstName { get; }

        /// <summary>
        /// Sobrenome
        /// </summary>
        string LastName { get; }

        /// <summary>
        /// Email principal
        /// </summary>
        string Email { get; }

        /// <summary>
        /// Telefone principal
        /// </summary>
        string Phone { get; }

        /// <summary>
        /// Usuário validou o email?
        /// </summary>
        bool IsEmailValidated { get; }

        /// <summary>
        /// Token para recuperação de senha
        /// </summary>
        string RecoveryPasswordToken { get; }

        /// <summary>
        /// Lista de regras
        /// </summary>
        IList<int> Roles { get; }
    }
}
