/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Api.ViewModel
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Tokens de autenticação
    /// </summary>
    public class ApiErrorResponse : IResponseViewModel
    {
        /// <summary>
        /// Lista de erros
        /// </summary>
        public IList<ApiErrorDetailResponse> Errors { get; private set; }

        /// <summary>
        /// Construtor padrão
        /// </summary>
        public ApiErrorResponse()
        {
            Errors = new List<ApiErrorDetailResponse>();
        }

        /// <summary>
        /// Construtor padrão
        /// </summary>
        /// <param name="apiErrorDetail">Detalhe do erro</param>
        public ApiErrorResponse(
            ApiErrorDetailResponse apiErrorDetail) : this()
        {
            if (apiErrorDetail != null)
            {
                Errors.Add(apiErrorDetail);
            }
        }

        /// <summary>
        /// Construtor padrão
        /// </summary>
        /// <param name="apiErrorDetails">Lista de erros</param>
        public ApiErrorResponse(
            IList<ApiErrorDetailResponse> apiErrorDetails) : this()
        {
            if (apiErrorDetails != null && apiErrorDetails.Any())
            {
                foreach (ApiErrorDetailResponse item in apiErrorDetails)
                {
                    Errors.Add(item);
                }
            }
        }
    }
}
