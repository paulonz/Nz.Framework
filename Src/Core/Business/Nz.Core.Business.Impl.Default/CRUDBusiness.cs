/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Core.Business.Impl.Default
{
    using System;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Nz.Core.UnitOfWork;

    /// <summary>
    /// Classe padrão de negócios
    /// </summary>
    /// <typeparam name="T">Tipo da model para manipulação</typeparam>
    public partial class CRUDBusiness<T> : ICRUDBusiness<T>
        where T : class, Model.IModel
    {
        /// <summary>
        /// Implementação de IUnitOfWork
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Construtor padrão
        /// </summary>
        /// <param name="unitOfWork">Implementação de IUnitOfWork</param>
        /// <param name="logger">Logger</param>
        public CRUDBusiness(
            IUnitOfWork unitOfWork,
            ILogger<CRUDBusiness<T>> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        /// <summary>
        /// Cria um novo objeto no repositório
        /// </summary>
        /// <param name="model">Novo objeto</param>
        public virtual async Task<T> CreateAsync(
            T model)
        {
            try
            {
                return await _unitOfWork
                    .CreateAsync(model)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return null;
        }

        /// <summary>
        /// Remove um objeto do repositório
        /// </summary>
        /// <param name="id">Identificador do ojeto</param>
        public virtual async Task<T> DeleteAsync(
            long id)
        {
            try
            {
                return await _unitOfWork
                    .DeleteAsync<T>(id)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return null;
        }

        /// <summary>
        /// Remove uma coleção de objetos do repositório
        /// </summary>
        /// <param name="where">Expression para localizar os objetos</param>
        public virtual async Task<IQueryable<T>> DeleteAsync(
            string where)
        {
            try
            {
                return await _unitOfWork
                    .DeleteAsync<T>(where)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return null;
        }

        /// <summary>
        /// Restaura um objeto do respositório
        /// </summary>
        /// <param name="id">Identificador do ojeto</param>
        public virtual async Task<T> UnDeleteAsync(
            long id)
        {
            try
            {
                return await _unitOfWork
                    .UnDeleteAsync<T>(id)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return null;
        }

        /// <summary>
        /// Restaura uma coleção de objetos do repositório
        /// </summary>
        /// <param name="where">Expression para localizar os objetos</param>
        public virtual async Task<IQueryable<T>> UnDeleteAsync(
            string where)
        {
            try
            {
                return await _unitOfWork
                    .UnDeleteAsync<T>(where)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return null;
        }

        /// <summary>
        /// Atualiza um objeto no repositório
        /// </summary>
        /// <param name="model">Objeto para ser atualizado</param>
        /// <param name="id">Identificador do objeto</param>
        public virtual async Task<T> UpdateAsync(
            long id,
            T model)
        {
            try
            {
                if (model != null)
                {
                    model.Id = id;
                    return await _unitOfWork
                        .UpdateAsync(model)
                        .ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return null;
        }

        /// <summary>
        /// Recupera um objeto do respositório a partir do Id
        /// </summary>
        /// <param name="id">Identificador do objeto</param>
        /// <param name="include">Objetos para incluir no retorno</param>
        /// <returns>Objeto localizado, null para não localizado</returns>
        public virtual async Task<T> ReadAsync(
            long id,
            [Optional] string[] include)
        {
            try
            {
                return await _unitOfWork
                    .ReadAsync<T>(id, include)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return null;
        }

        /// <summary>
        /// Recupera uma coleção de objetos do respositório
        /// </summary>
        /// <param name="where">Expression para localizar os objetos</param>
        /// <param name="orderBy">Expressions de ordenação</param>
        /// <param name="include">Objetos para incluir no retorno</param>
        /// <returns>Coleção de objetos localizados</returns>
        public virtual async Task<IQueryable<T>> ReadAsync(
            string where,
            [Optional] string orderBy,
            [Optional] string[] include)
        {
            try
            {
                return await _unitOfWork
                    .ReadAsync<T>(where, orderBy, include)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return null;
        }

        /// <summary>
        /// Recupera primeira ocorrência do objeto no respositório
        /// </summary>
        /// <param name="where">Expression para localizar o objeto</param>
        /// <param name="orderBy">Expressions de ordenação</param>
        /// <param name="include">Objetos para incluir no retorno</param>
        /// <returns>Primeira ocorrência do objeto</returns>
        public virtual async Task<T> ReadFirstAsync(
            string where,
            [Optional] string orderBy,
            [Optional] string[] include)
        {
            try
            {
                return await _unitOfWork
                    .ReadFirstAsync<T>(where, orderBy, include)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return null;
        }

        /// <summary>
        /// Recupera ultima ocorrência do objeto no respositório
        /// </summary>
        /// <param name="where">Expression para localizar o objeto</param>
        /// <param name="orderBy">Expressions de ordenação</param>
        /// <param name="include">Objetos para incluir no retorno</param>
        /// <returns>Ultima ocorrência do objeto</returns>
        public virtual async Task<T> ReadLastAsync(
            string where,
            [Optional] string orderBy,
            [Optional] string[] include)
        {
            try
            {
                return await _unitOfWork
                    .ReadLastAsync<T>(where, orderBy, include)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return null;
        }
    }
}
