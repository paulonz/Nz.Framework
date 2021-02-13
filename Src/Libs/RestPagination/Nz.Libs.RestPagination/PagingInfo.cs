/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Libs.RestPagination
{
    using System;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// Informações sobre a paginação
    /// </summary>
    public sealed class PagingInfo
    {
        /// <summary>
        /// Construtor padrão
        /// </summary>
        internal PagingInfo()
        {
            Page = 1;
            PageSize = 50;
            TotalResults = 0;
            Next = null;
            Previous = null;
        }

        /// <summary>
        /// Quantidade de registros por página
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Quantidade total de registros
        /// </summary>
        public int TotalResults { get; set; }

        /// <summary>
        /// Quantidade total de páginas
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Página atual
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Link para a página anterior
        /// </summary>
        public string Previous { get; internal set; }

        /// <summary>
        /// Link para a próxima página
        /// </summary>
        public string Next { get; internal set; }

        /// <summary>
        /// Recupera as informações sobre a paginação a partir do request http
        /// </summary>
        /// <param name="httpRequest">Request Http</param>
        /// <returns>Informações sobre a paginação</returns>
        internal static PagingInfo FromRequest(
            HttpRequest httpRequest)
        {
            PagingInfo pagingInfo = new PagingInfo();

            if (httpRequest.Query.ContainsKey(Validations.Pagination_Query_Page))
            {
                pagingInfo.Page = httpRequest.Query.TryGet<int>(Validations.Pagination_Query_Page);
            }

            if (httpRequest.Query.ContainsKey(Validations.Pagination_Query_PageSize))
            {
                pagingInfo.PageSize = httpRequest.Query.TryGet<int>(Validations.Pagination_Query_PageSize);
            }

            if (pagingInfo.PageSize > 1000)
            {
                throw new ArgumentOutOfRangeException(Validations.Pagination_Error_PageSizeMustBeLessThan);
            }

            return pagingInfo;
        }
    }
}
