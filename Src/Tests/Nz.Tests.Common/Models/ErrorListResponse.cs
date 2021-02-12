/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Tests.Common.Models
{
    /// <summary>
    /// Mock do response de erros
    /// </summary>
    public class ErrorListResponse
    {
        /// <summary>
        /// Lista de erros retornados
        /// </summary>
        public ErrorResponse[] Errors { get; set; }
    }
}
