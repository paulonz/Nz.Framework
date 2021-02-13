/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Common.Helpers
{
    /// <summary>
    /// Helper para converção de objetos
    /// </summary>
    public interface IParserHelper
    {
        /// <summary>
        /// Converte um objeto para uma string Json
        /// </summary>
        /// <typeparam name="T">Tipo do objeto</typeparam>
        /// <param name="model">Objeto para conversão</param>
        /// <returns>String Json</returns>
        string ToJson<T>(
            T model) where T : class;

        /// <summary>
        /// Converte uma string Json em um objeto
        /// </summary>
        /// <typeparam name="T">Tipo do objeto</typeparam>
        /// <param name="json">String para conversão</param>
        /// <returns>Objeto convertido</returns>
        T FromJson<T>(
            string json) where T : class;

        /// <summary>
        /// Converte de um objeto para outro
        /// </summary>
        /// <typeparam name="Target">Tipo do objeto de origem</typeparam>
        /// <typeparam name="From">Tipo do objeto de destino</typeparam>
        /// <param name="model">Objeto que será convertido</param>
        /// <returns>Objeto convertido</returns>
        Target To<Target, From>(From model)
            where Target : class
            where From : class;
    }
}
