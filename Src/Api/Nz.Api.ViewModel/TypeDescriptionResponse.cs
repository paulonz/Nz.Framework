/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Api.ViewModel
{
    /// <summary>
    /// Descrição de um tipo
    /// </summary>
    public class TypeDescriptionResponse : IResponseViewModel
    {
        /// <summary>
        /// Identificador do valor
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Descrição do valor
        /// </summary>
        public string Description { get; set; }
    }
}
