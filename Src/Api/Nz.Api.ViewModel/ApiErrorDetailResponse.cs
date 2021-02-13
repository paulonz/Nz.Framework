/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Api.ViewModel
{
    /// <summary>
    /// Detalhamento de um erro
    /// </summary>
    public class ApiErrorDetailResponse : IResponseViewModel
    {
        /// <summary>
        /// Tipo do erro
        /// </summary>
        public ErrorType ErrorType { get; set; }

        /// <summary>
        /// Mensagem de erro
        /// </summary>
        public string Message { get; set; }
    }
}
