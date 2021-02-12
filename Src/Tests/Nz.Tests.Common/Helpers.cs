/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Tests.Common
{
    using System;
    using System.Net.Http;
    using System.Net.Mime;
    using System.Text;

    /// <summary>
    /// Métodos auxiliares para os testes
    /// </summary>
    public static class Helpers
    {
        /// <summary>
        /// Extensão que converte um objeto em uma StringContent para requisições POST
        /// </summary>
        /// <param name="obj">Objeto</param>
        /// <returns>StringContent</returns>
        public static StringContent ToStringContent(
            this object obj)
        {
            return new StringContent(
                Newtonsoft.Json.JsonConvert.SerializeObject(obj),
                Encoding.UTF8,
                MediaTypeNames.Application.Json);
        }

        /// <summary>
        /// Gera uma string com um identificador randomico
        /// </summary>
        /// <returns>String com um identificador randomico</returns>
        public static string GenerateRandonIdentifier()
        {
            string guid = Guid.NewGuid().ToString();
            guid = guid.Replace("-", "_");

            return guid;
        }
    }
}
