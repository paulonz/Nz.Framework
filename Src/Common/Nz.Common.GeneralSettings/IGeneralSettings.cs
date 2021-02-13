/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Common.GeneralSettings
{
    using System;
    using System.Text;

    /// <summary>
    /// Configurações gerais da aplicação
    /// </summary>
    public interface IGeneralSettings
    {
        /// <summary>
        /// Data e hora atual (UTC)
        /// </summary>
        DateTime CurrentDateTime { get; }

        /// <summary>
        /// Encoding padrão da aplicação
        /// </summary>
        Encoding DefaultEncoding { get; }

        /// <summary>
        /// Uri base da aplicação
        /// </summary>
        Uri BaseUri { get; }
    }
}
