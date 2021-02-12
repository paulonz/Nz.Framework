/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Core.Business
{
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface para objetos de negócio
    /// </summary>
    /// <typeparam name="T">Model manipulada</typeparam>
    public interface ICRUDBusiness<T>
        where T : Model.IModel
    {
        /// <summary>
        /// Cria um novo objeto no repositório
        /// </summary>
        /// <param name="model">Novo objeto</param>
        Task<T> CreateAsync(
            T model);

        /// <summary>
        /// Remove um objeto do repositório
        /// </summary>
        /// <param name="id">Identificador do ojeto</param>
        Task<T> DeleteAsync(
            long id);

        /// <summary>
        /// Remove uma coleção de objetos do repositório
        /// </summary>
        /// <param name="where">Expression para localizar os objetos</param>
        Task<IQueryable<T>> DeleteAsync(
            string where);

        /// <summary>
        /// Restaura um objeto do respositório
        /// </summary>
        /// <param name="id">Identificador do ojeto</param>
        Task<T> UnDeleteAsync(
            long id);

        /// <summary>
        /// Restaura uma coleção de objetos do repositório
        /// </summary>
        /// <param name="where">Expression para localizar os objetos</param>
        Task<IQueryable<T>> UnDeleteAsync(
            string where);

        /// <summary>
        /// Atualiza um objeto no repositório
        /// </summary>
        /// <param name="model">Objeto para ser atualizado</param>
        /// <param name="id">Identificador do objeto</param>
        Task<T> UpdateAsync(
            long id,
            T model);

        /// <summary>
        /// Recupera um objeto do respositório a partir do Id
        /// </summary>
        /// <param name="id">Identificador do objeto</param>
        /// <param name="include">Objetos para incluir no retorno</param>
        /// <returns>Objeto localizado, null para não localizado</returns>
        Task<T> ReadAsync(
            long id,
            string[] include);

        /// <summary>
        /// Recupera uma coleção de objetos do respositório
        /// </summary>
        /// <param name="where">Expression para localizar os objetos</param>
        /// <param name="orderBy">Expressions de ordenação</param>
        /// <param name="include">Objetos para incluir no retorno</param>
        /// <returns>Coleção de objetos localizados</returns>
        Task<IQueryable<T>> ReadAsync(
            string where,
            [Optional] string orderBy,
            [Optional] string[] include);

        /// <summary>
        /// Recupera primeira ocorrência do objeto no respositório
        /// </summary>
        /// <param name="where">Expression para localizar o objeto</param>
        /// <param name="orderBy">Expressions de ordenação</param>
        /// <param name="include">Objetos para incluir no retorno</param>
        /// <returns>Primeira ocorrência do objeto</returns>
        Task<T> ReadFirstAsync(
            string where,
            [Optional] string orderBy,
            [Optional] string[] include);

        /// <summary>
        /// Recupera ultima ocorrência do objeto no respositório
        /// </summary>
        /// <param name="where">Expression para localizar o objeto</param>
        /// <param name="orderBy">Expressions de ordenação</param>
        /// <param name="include">Objetos para incluir no retorno</param>
        /// <returns>Ultima ocorrência do objeto</returns>
        Task<T> ReadLastAsync(
            string where,
            [Optional] string orderBy,
            [Optional] string[] include);
    }
}
