/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Core.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Interface base para entidades da aplicação
    /// </summary>
    public interface IModel
    {
        /// <summary>
        /// Identificador único do registro
        /// </summary>
        [Key]
        long Id { get; set; }

        /// <summary>
        /// Data em que o objeto foi criado
        /// </summary>
        DateTime CreatedOn { get; set; }

        /// <summary>
        /// Usuário que criou o objeto
        /// </summary>
        long? CreatedBy { get; set; }

        /// <summary>
        /// Data em que o registro foi excluído
        /// </summary>
        DateTime? ExcludedOn { get; set; }

        /// <summary>
        /// Usuário que excluiu o objeto
        /// </summary>
        public long? ExcludedBy { get; set; }

        /// <summary>
        /// Data em que o objeto foi atualizado pela última vez
        /// </summary>
        DateTime? UpdatedOn { get; set; }

        /// <summary>
        /// Último usuário que atualizou o objeto
        /// </summary>
        long? UpdatedBy { get; set; }
    }
}
