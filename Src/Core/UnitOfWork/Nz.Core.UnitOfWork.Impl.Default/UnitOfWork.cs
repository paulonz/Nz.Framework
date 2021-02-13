/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Core.UnitOfWork.Impl.Default
{
    using System;
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Nz.Common.GeneralSettings;
    using Nz.Core.DatabaseContext;

    /// <summary>
    /// Implementação de IUnitOfWork
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// Contexto do banco de dados
        /// </summary>
        private readonly DbContext _dbContext;

        /// <summary>
        /// Usuário autenticado
        /// </summary>
        private readonly Model.IAuthUser _authUser;

        /// <summary>
        /// Configurações gerais
        /// </summary>
        private readonly IGeneralSettings _general;

        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Construtor padrão
        /// </summary>
        /// <param name="dbContext">Contexto do banco de dados</param>
        /// <param name="authUser">Usuário autenticado</param>
        /// <param name="general">Configurações gerais</param>
        /// <param name="logger">Logger</param>
        public UnitOfWork(
            IDbContext dbContext,
            Model.IAuthUser authUser,
            IGeneralSettings general,
            ILogger<UnitOfWork> logger)
        {
            _dbContext = dbContext.CurrentDbContext;
            _authUser = authUser;
            _general = general;
            _logger = logger;
        }

        /// <summary>
        /// Cria um novo objeto no repositório
        /// </summary>
        /// <typeparam name="T">ModelBase</typeparam>
        /// <param name="model">Novo objeto</param>
        public async Task<T> CreateAsync<T>(
            T model) where T : class, Model.IModel
        {
            try
            {
                if (model == null)
                {
                    return null;
                }

                model.ExcludedOn = null;
                model.CreatedOn = _general.CurrentDateTime;
                model.CreatedBy = _authUser?.UserId;

                await _dbContext.Set<T>()
                    .AddAsync(model)
                    .ConfigureAwait(false);

                return model;
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
        /// <typeparam name="T">ModelBase</typeparam>
        /// <param name="id">Identificador do ojeto</param>
        public async Task<T> DeleteAsync<T>(
            long id) where T : class, Model.IModel
        {
            try
            {
                T model = await ReadAsync<T>(id)
                    .ConfigureAwait(false);

                if (model != null)
                {
                    model.ExcludedOn = _general.CurrentDateTime;
                    model.ExcludedBy = _authUser?.UserId;

                    _dbContext.Entry(model).State = EntityState.Modified;

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
        /// <typeparam name="T">ModelBase</typeparam>
        /// <param name="where">Expression para localizar os objetos</param>
        public async Task<IQueryable<T>> DeleteAsync<T>(
            string where) where T : class, Model.IModel
        {
            try
            {
                IQueryable<T> items = await ReadAsync<T>(where)
                    .ConfigureAwait(false);

                if (items != null && items.Any())
                {
                    foreach (T item in items)
                    {
                        item.ExcludedOn = _general.CurrentDateTime;
                        item.ExcludedBy = _authUser?.UserId;

                        _dbContext.Entry(item).State = EntityState.Modified;
                    }

                    return items;
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
        /// <typeparam name="T">ModelBase</typeparam>
        /// <param name="id">Identificador do ojeto</param>
        public async Task<T> UnDeleteAsync<T>(
            long id) where T : class, Model.IModel
        {
            try
            {
                T model = await ReadAsync<T>(id)
                    .ConfigureAwait(false);

                if (model != null)
                {
                    model.ExcludedOn = null;
                    model.ExcludedBy = null;

                    _dbContext.Entry(model).State = EntityState.Modified;

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
        /// <typeparam name="T">ModelBase</typeparam>
        /// <param name="where">Expression para localizar os objetos</param>
        public async Task<IQueryable<T>> UnDeleteAsync<T>(
            string where) where T : class, Model.IModel
        {
            try
            {
                IQueryable<T> items = await ReadAsync<T>(where)
                    .ConfigureAwait(false);

                if (items != null && items.Any())
                {
                    foreach (T item in items)
                    {
                        item.ExcludedOn = null;
                        item.ExcludedBy = null;

                        _dbContext.Entry(item).State = EntityState.Modified;
                    }

                    return items;
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
        /// <typeparam name="T">ModelBaseo</typeparam>
        /// <param name="model">Objeto para ser atualizado</param>
        public async Task<T> UpdateAsync<T>(
            T model) where T : class, Model.IModel
        {
            try
            {
                if (model != null && model.Id > 0)
                {
                    T controlModel = await ReadAsync<T>(model.Id)
                        .ConfigureAwait(false);

                    if (controlModel != null)
                    {
                        model.CreatedBy = controlModel.CreatedBy;
                        model.CreatedOn = controlModel.CreatedOn;
                        model.ExcludedBy = controlModel.ExcludedBy;
                        model.ExcludedOn = controlModel.ExcludedOn;

                        model.UpdatedOn = _general.CurrentDateTime;
                        model.UpdatedBy = _authUser?.UserId;

                        _dbContext.Entry(model).State = EntityState.Modified;

                        return model;
                    }
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
        /// <typeparam name="T">ModelBase</typeparam>
        /// <param name="id">Identificador do objeto</param>
        /// <param name="include">Objetos para incluir no retorno</param>
        /// <returns>Objeto localizado, null para não localizado</returns>
        public async Task<T> ReadAsync<T>(
            long id,
            [Optional] string[] include) where T : class, Model.IModel
        {
            try
            {
                if (id > 0)
                {
                    IQueryable<T> objectList = await ReadAsync<T>($"w => w.Id == {id}", include: include)
                        .ConfigureAwait(false);

                    if (objectList != null)
                    {
                        return await objectList.FirstOrDefaultAsync()
                            .ConfigureAwait(false);
                    }
                }
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
        /// <typeparam name="T">ModelBase</typeparam>
        /// <param name="where">Expression para localizar os objetos</param>
        /// <param name="orderBy">Expressions de ordenação</param>
        /// <param name="include">Objetos para incluir no retorno</param>
        /// <returns>Coleção de objetos localizados</returns>
        public async Task<IQueryable<T>> ReadAsync<T>(
            string where,
            [Optional] string orderBy,
            [Optional] string[] include) where T : class, Model.IModel
        {
            try
            {
                if (string.IsNullOrEmpty(where))
                {
                    where = "ExcludedOn == null";
                }

                IQueryable<T> queryResult = _dbContext.Set<T>()
                    .Where(where)
                    .AsNoTrackingWithIdentityResolution();

                if (!string.IsNullOrEmpty(orderBy))
                {
                    queryResult = queryResult.OrderBy(orderBy);
                }

                return await IncludeAsync(queryResult, include)
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
        /// <typeparam name="T">ModelBase</typeparam>
        /// <param name="where">Expression para localizar o objeto</param>
        /// <param name="orderBy">Expressions de ordenação</param>
        /// <param name="include">Objetos para incluir no retorno</param>
        /// <returns>Primeira ocorrência do objeto</returns>
        public async Task<T> ReadFirstAsync<T>(
            string where,
            [Optional] string orderBy,
            [Optional] string[] include) where T : class, Model.IModel
        {
            try
            {
                if (where != null)
                {
                    IQueryable<T> objectList = await ReadAsync<T>(where, orderBy: orderBy, include: include)
                        .ConfigureAwait(false);

                    if (objectList != null)
                    {
                        return await objectList.FirstOrDefaultAsync()
                            .ConfigureAwait(false);
                    }
                }
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
        /// <typeparam name="T">ModelBase</typeparam>
        /// <param name="where">Expression para localizar o objeto</param>
        /// <param name="orderBy">Expressions de ordenação</param>
        /// <param name="include">Objetos para incluir no retorno</param>
        /// <returns>Ultima ocorrência do objeto</returns>
        public async Task<T> ReadLastAsync<T>(
            string where,
            [Optional] string orderBy,
            [Optional] string[] include) where T : class, Model.IModel
        {
            try
            {
                if (where != null)
                {
                    IQueryable<T> objectList = await ReadAsync<T>(where, orderBy: orderBy, include: include)
                        .ConfigureAwait(false);

                    if (objectList != null)
                    {
                        return await objectList.LastOrDefaultAsync()
                            .ConfigureAwait(false);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return null;
        }

        /// <summary>
        /// Aplica as migrations pendentes para o banco de dados
        /// </summary>
        /// <returns>Ok</returns>
        public async Task ApplyMigrationsAsync()
        {
            try
            {
                if (_dbContext != null)
                {
                    await _dbContext.Database.MigrateAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        /// <summary>
        /// Checagem da saúde da aplicação
        /// </summary>
        /// <returns>Ok</returns>
        public async Task<bool> HealthCheckAsync()
        {
            return await _dbContext.Database.CanConnectAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Adiciona os itens de include na consulta
        /// </summary>
        /// <typeparam name="T">Tipo do objeto de resultado</typeparam>
        /// <param name="query">Consulta</param>
        /// <param name="include">Atributos para adicionar ao resultado</param>
        /// <returns>Query atualizada</returns>
        private async Task<IQueryable<T>> IncludeAsync<T>(
            IQueryable<T> query,
            string[] include) where T : class, Model.IModel
        {
            try
            {
                if (query != null)
                {
                    if (include != null && include.Any())
                    {
                        await Task.Run(() =>
                        {
                            foreach (string item in include)
                            {
                                // ajuste de case
                                string tmp = $"{item.First().ToString().ToUpperInvariant()}{item[1..]}";
                                query = query.Include(tmp);
                            }
                        }).ConfigureAwait(false);
                    }

                    return query;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return null;
        }
    }
}
