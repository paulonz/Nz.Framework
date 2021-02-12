/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Api.ViewModel.Auth
{
    using System;

    /// <summary>
    /// Dados do usuário autenticado
    /// </summary>
    public class MeResponse : IResponseViewModel
    {
        /// <summary>
        /// Identificador único do registro
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Data em que o objeto foi criado
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Usuário que criou o objeto
        /// </summary>
        public long? CreatedBy { get; set; }

        /// <summary>
        /// Data em que o registro foi excluído
        /// </summary>
        public DateTime? ExcludedOn { get; set; }

        /// <summary>
        /// Usuário que criou o objeto
        /// </summary>
        public long? ExcludedBy { get; set; }

        /// <summary>
        /// Data em que o objeto foi atualizado pela última vez
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

        /// <summary>
        /// Último usuário que atualizou o objeto
        /// </summary>
        public long? UpdatedBy { get; set; }

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
        /// Número de telefone
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Usuário validou o email?
        /// </summary>
        public bool IsEmailValidated { get; set; }

        /// <summary>
        /// Lista de regras que o usuário possui
        /// </summary>
        public UserRoleResponse[] UserRoles { get; set; }
    }
}
