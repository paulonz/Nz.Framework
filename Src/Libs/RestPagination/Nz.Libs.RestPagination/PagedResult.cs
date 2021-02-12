/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Libs.RestPagination
{
    /// <summary>
    /// Lista paginada
    /// </summary>
    public class PagedResult
    {
        /// <summary>
        /// Lista de objetos
        /// </summary>
        public object Items { get; set; }

        /// <summary>
        /// Informações sobre a paginação
        /// </summary>
        public PagingInfo Pagination { get; set; }
    }
}
