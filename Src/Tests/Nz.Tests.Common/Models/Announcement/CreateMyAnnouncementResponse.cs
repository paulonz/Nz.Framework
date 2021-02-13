/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Tests.Common.Models.Announcement
{
    using System;

    /// <summary>
    /// Anúncio classificado
    /// </summary>
    public class CreateMyAnnouncementResponse
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
        /// Usuário que excluíu o objeto
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
        /// Título do anúncio
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Descrição completa do anúncio
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Valor do item anúnciado
        /// </summary>
        public decimal Price { get; set; }
    }
}
