/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Tests.Common.Models.Announcement
{
    /// <summary>
    /// Anúncio classificado
    /// </summary>
    public class CreateMyAnnouncementRequest
    {
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
