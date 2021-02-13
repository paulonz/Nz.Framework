/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Common.Helpers
{
    using System;

    /// <summary>
    /// Helper para tratamento de resources
    /// </summary>
    public interface IResourceHelper
    {
        /// <summary>
        /// Recupera o valor em um resource
        /// </summary>
        /// <param name="resourceType">Type do resource</param>
        /// <param name="resourceKey">Chave</param>
        /// <returns>Valor</returns>
        string LookupResource(
            Type resourceType,
            string resourceKey);
    }
}
