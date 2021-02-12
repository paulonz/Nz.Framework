/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Common.Helpers
{
    using System;

    /// <summary>
    /// Classe auxiliar
    /// </summary>
    public interface IEnumHelpers
    {
        /// <summary>
        /// Recupera o valor do atributo Display de um enum value
        /// </summary>
        /// <typeparam name="T">Enum</typeparam>
        /// <param name="source">Valor</param>
        /// <param name="resourceManager">Resource</param>
        /// <returns>Descrição</returns>
        string GetDisplay<T>(
            T source,
            System.Resources.ResourceManager resourceManager) where T : Enum;
    }
}
