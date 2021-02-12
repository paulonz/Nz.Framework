/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Libs.RestPagination
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Http;

    internal static class Extensions
    {
        /// <summary>
        /// True caso a resposta http seja 2xx.
        /// </summary>
        /// <param name="response">Objeto HttpResponse</param>
        /// <returns>Sucesso</returns>
        public static bool IsSuccessStatusCode(
            this HttpResponse response)
        {
            return response.StatusCode == 200;
        }

        /// <summary>
        /// Scheme e host do request
        /// </summary>
        /// <param name="request">Request atual</param>
        /// <returns>string com Scheme e host do request</returns>
        public static string SchemeAndHost(
            this HttpRequest request)
        {
            string currentQuerystring = string.Empty;

            if (request.Query.Any())
            {
                foreach (System.Collections.Generic.KeyValuePair<string, Microsoft.Extensions.Primitives.StringValues> item in request.Query)
                {
                    if (item.Key.ToLowerInvariant() != Validations.Pagination_Query_Page &&
                        item.Key.ToLowerInvariant() != Validations.Pagination_Query_PageSize)
                    {
                        currentQuerystring += $"&{item.Key}={item.Value}";
                    }
                }

                if (!string.IsNullOrEmpty(currentQuerystring))
                {
                    currentQuerystring += "&";
                }
            }

            return $"{request.Scheme}://{request.Host}{request.Path}?{currentQuerystring}";
        }

        /// <summary>
        /// Tenta recuperar um valor da querystring
        /// </summary>
        /// <typeparam name="T">Tipo do valor de retorno</typeparam>
        /// <param name="query">Querystring</param>
        /// <param name="key">Chave que será procurada</param>
        /// <returns>Valor encontrado</returns>
        public static T TryGet<T>(
            this IQueryCollection query,
            string key)
        {
            if (query.ContainsKey(key))
            {
                if (query.TryGetValue(key, out Microsoft.Extensions.Primitives.StringValues keyValue))
                {
                    return (T)Convert.ChangeType(keyValue.ToString(), typeof(T));
                }
            }

            throw new ArgumentNullException(key);
        }

        /// <summary>
        /// Retorna um resultado paginado para uma queryable collection
        /// </summary>
        /// <param name="source">Collection</param>
        /// <param name="pageInfo">Informações da paginação</param>
        /// <param name="request">Http request</param>
        /// <returns>Resultado paginado</returns>
        public static PagedResult ToPagedResult(
            this IQueryable source,
            PagingInfo pageInfo,
            HttpRequest request = null)
        {
            dynamic list = GetGenericSkip((dynamic)source, (pageInfo.Page - 1) * pageInfo.PageSize, pageInfo.PageSize);
            int totalResults = GetGenericCount((dynamic)source);
            int totalPages = (int)Math.Ceiling(totalResults / (decimal)pageInfo.PageSize);

            string nextLink = string.Empty;
            string previousLink = string.Empty;

            if (request != null)
            {
                if (pageInfo.Page < totalPages)
                {
                    nextLink = request.SchemeAndHost();
                    nextLink += $"{Validations.Pagination_Query_Page}=" + (pageInfo.Page + 1).ToString();
                    if (request.Query.ContainsKey(Validations.Pagination_Query_PageSize))
                    {
                        nextLink += $"&{Validations.Pagination_Query_PageSize}=" + request.Query[Validations.Pagination_Query_PageSize];
                    }
                }
                if (pageInfo.Page > 1)
                {
                    previousLink = request.SchemeAndHost();
                    previousLink += $"{Validations.Pagination_Query_Page}=" + (pageInfo.Page - 1).ToString();
                    if (request.Query.ContainsKey(Validations.Pagination_Query_PageSize))
                    {
                        previousLink += $"&{Validations.Pagination_Query_PageSize}=" + request.Query[Validations.Pagination_Query_Page];
                    }
                }
            }

            return new PagedResult()
            {
                Items = list,
                Pagination = new PagingInfo
                {
                    TotalResults = totalResults,
                    TotalPages = totalPages,
                    PageSize = pageInfo.PageSize,
                    Page = pageInfo.Page,
                    Next = nextLink,
                    Previous = previousLink
                }
            };
        }

        /// <summary>
        /// Retorna a quantidade de registro para uma generic iqueryable collection.
        /// </summary>
        /// <typeparam name="T">Tipo da collection</typeparam>
        /// <param name="source">Collection</param>
        /// <returns>Quantidade de registros</returns>
        private static int GetGenericCount<T>(
            IQueryable<T> source)
        {
            return source.Count();
        }

        /// <summary>
        /// 'Pula' e 'pega' uma quantidade de registros da generic iqueryable collection.
        /// </summary>
        /// <typeparam name="T">Tipo da collection</typeparam>
        /// <param name="source">Collection</param>
        /// <param name="skip">Quantidade de registros para 'pular'</param>
        /// <param name="take">Quantidade de registros para 'pegar'</param>
        /// <returns>Recorte da collection</returns>
        private static T[] GetGenericSkip<T>(
            IQueryable<T> source,
            int skip,
            int take)
        {
            return source.Skip(skip).Take(take).ToArray();
        }
    }
}
