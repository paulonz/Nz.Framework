/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Core.UnitOfWork
{
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface base para UnitOfWork
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Cria um novo objeto no repositório
        /// </summary>
        /// <typeparam name="T">ModelBase</typeparam>
        /// <param name="model">Novo objeto</param>
        Task<T> CreateAsync<T>(
            T model) where T : class, Model.IModel;

        /// <summary>
        /// Remove um objeto do repositório
        /// </summary>
        /// <typeparam name="T">ModelBase</typeparam>
        /// <param name="id">Identificador do ojeto</param>
        Task<T> DeleteAsync<T>(
            long id) where T : class, Model.IModel;

        /// <summary>
        /// Remove uma coleção de objetos do repositório
        /// </summary>
        /// <typeparam name="T">ModelBase</typeparam>
        /// <param name="where">Expression para localizar os objetos</param>
        Task<IQueryable<T>> DeleteAsync<T>(
            string where) where T : class, Model.IModel;

        /// <summary>
        /// Restaura um objeto do respositório
        /// </summary>
        /// <typeparam name="T">ModelBase</typeparam>
        /// <param name="id">Identificador do ojeto</param>
        Task<T> UnDeleteAsync<T>(
            long id) where T : class, Model.IModel;

        /// <summary>
        /// Restaura uma coleção de objetos do repositório
        /// </summary>
        /// <typeparam name="T">ModelBase</typeparam>
        /// <param name="where">Expression para localizar os objetos</param>
        Task<IQueryable<T>> UnDeleteAsync<T>(
            string where) where T : class, Model.IModel;

        /// <summary>
        /// Atualiza um objeto no repositório
        /// </summary>
        /// <typeparam name="T">ModelBaseo</typeparam>
        /// <param name="model">Objeto para ser atualizado</param>
        Task<T> UpdateAsync<T>(
            T model) where T : class, Model.IModel;

        /// <summary>
        /// Recupera um objeto do respositório a partir do Id
        /// </summary>
        /// <typeparam name="T">ModelBase</typeparam>
        /// <param name="id">Identificador do objeto</param>
        /// <param name="include">Objetos para incluir no retorno</param>
        /// <returns>Objeto localizado, null para não localizado</returns>
        Task<T> ReadAsync<T>(
            long id,
            [Optional] string[] include) where T : class, Model.IModel;

        /// <summary>
        /// Recupera uma coleção de objetos do respositório
        /// </summary>
        /// <typeparam name="T">ModelBase</typeparam>
        /// <param name="where">Expression para localizar os objetos</param>
        /// <param name="orderBy">Expressions de ordenação</param>
        /// <param name="include">Objetos para incluir no retorno</param>
        /// <returns>Coleção de objetos localizados</returns>
        Task<IQueryable<T>> ReadAsync<T>(
            string where,
            [Optional] string orderBy,
            [Optional] string[] include) where T : class, Model.IModel;

        /// <summary>
        /// Recupera primeira ocorrência do objeto no respositório
        /// </summary>
        /// <typeparam name="T">ModelBase</typeparam>
        /// <param name="where">Expression para localizar o objeto</param>
        /// <param name="orderBy">Expressions de ordenação</param>
        /// <param name="include">Objetos para incluir no retorno</param>
        /// <returns>Primeira ocorrência do objeto</returns>
        Task<T> ReadFirstAsync<T>(
            string where,
            [Optional] string orderBy,
            [Optional] string[] include) where T : class, Model.IModel;

        /// <summary>
        /// Recupera ultima ocorrência do objeto no respositório
        /// </summary>
        /// <typeparam name="T">ModelBase</typeparam>
        /// <param name="where">Expression para localizar o objeto</param>
        /// <param name="orderBy">Expressions de ordenação</param>
        /// <param name="include">Objetos para incluir no retorno</param>
        /// <returns>Ultima ocorrência do objeto</returns>
        Task<T> ReadLastAsync<T>(
            string where,
            [Optional] string orderBy,
            [Optional] string[] include) where T : class, Model.IModel;

        /// <summary>
        /// Aplica as migrations pendentes para o banco de dados
        /// </summary>
        /// <returns>Ok</returns>
        Task ApplyMigrationsAsync();

        /// <summary>
        /// Checagem da saúde da aplicação
        /// </summary>
        /// <returns>Ok</returns>
        Task<bool> HealthCheckAsync();
    }
}
