/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Core.Service.Impl.Default
{
    using System;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Nz.Core.Business;
    using Nz.Core.DatabaseContext;

    /// <summary>
    /// Classe padrão de serviço
    /// </summary>
    /// <typeparam name="T">Tipo da model para manipulação</typeparam>
    public class CRUDService<T> : ICRUDService<T>
        where T : class, Model.IModel
    {
        /// <summary>
        /// Implementação de IBusiness
        /// </summary>
        private readonly ICRUDBusiness<T> _business;

        /// <summary>
        /// Contexto de banco de dados
        /// </summary>
        private readonly DbContext _dbContext;

        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Construtor padrão
        /// </summary>
        /// <param name="business">Classe de negócios</param>
        /// <param name="dbContext">Contexto de banco de dados</param>
        /// <param name="logger">Logger</param>
        public CRUDService(
            ICRUDBusiness<T> business,
            IDbContext dbContext,
            ILogger<CRUDService<T>> logger)
        {
            _dbContext = dbContext?.CurrentDbContext;
            _business = business;
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
                model = await _business.CreateAsync(model).ConfigureAwait(false);
                if (model != null)
                {
                    await _dbContext.SaveChangesAsync().ConfigureAwait(false);
                    return model;
                }
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
                T model = await _business.DeleteAsync(id).ConfigureAwait(false);
                if (model != null)
                {
                    await _dbContext.SaveChangesAsync().ConfigureAwait(false);

                    return model;
                }
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
                IQueryable<T> models = await _business.DeleteAsync(where).ConfigureAwait(false);
                if (models != null)
                {
                    await _dbContext.SaveChangesAsync().ConfigureAwait(false);

                    return models;
                }
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
                T model = await _business.UnDeleteAsync(id).ConfigureAwait(false);

                if (model != null)
                {
                    await _dbContext.SaveChangesAsync().ConfigureAwait(false);

                    return model;
                }
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
                IQueryable<T> models = await _business.UnDeleteAsync(where).ConfigureAwait(false);

                if (models != null)
                {
                    await _dbContext.SaveChangesAsync().ConfigureAwait(false);

                    return models;
                }
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
        /// <param name="id">Identificador do objeto</param>
        /// <param name="model">Objeto para ser atualizado</param>
        public virtual async Task<T> UpdateAsync(
            int id,
            T model)
        {
            try
            {
                model = await _business.UpdateAsync(id, model).ConfigureAwait(false);

                if (model != null)
                {
                    await _dbContext.SaveChangesAsync().ConfigureAwait(false);
                    return model;
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
                return await _business.ReadAsync(id, include).ConfigureAwait(false);
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
                return await _business.ReadAsync(where, orderBy, include).ConfigureAwait(false);
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
                return await _business.ReadFirstAsync(where, orderBy, include).ConfigureAwait(false);
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
                return await _business.ReadLastAsync(where, orderBy, include).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return null;
        }
    }
}
