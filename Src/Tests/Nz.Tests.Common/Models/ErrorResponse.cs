/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Tests.Common.Models
{
    /// <summary>
    /// Mock do response de erro
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// Tipo do erro
        /// </summary>
        public int ErrorType { get; set; }

        /// <summary>
        /// Mensagem de erro
        /// </summary>
        public string Message { get; set; }
    }
}
