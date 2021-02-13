/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Api.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Nz.Core.Service;
    using Nz.Libs.RestPagination;

    /// <summary>
    /// Implementação padrão para controllers públicas do tipo CRUD
    /// </summary>
    /// <typeparam name="T">Model tratada pela controller</typeparam>
    public abstract class ApiControllerReadOnlyBase<T> : ApiControllerBase
        where T : class, Core.Model.IModel
    {
        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Objeto Service padrão para a controller
        /// </summary>
        private readonly ICRUDService<T> _currentService;

        /// <summary>
        /// Construtor padrão
        /// </summary>
        /// <param name="currentService">Serviço atual</param>
        /// <param name="logger">Logger</param>
        public ApiControllerReadOnlyBase(
            ICRUDService<T> currentService,
            ILogger<ApiControllerReadOnlyBase<T>> logger) : base(logger)
        {
            _currentService = currentService;
            _logger = logger;
        }

        /// <summary>
        /// Recupera uma lista paginada com todos objetos disponíveis de acordo com os filtros aplicados
        /// </summary>
        /// <param name="include">(Opcional) Objetos para incluir no resultado ex: <code>?include=person</code></param>
        /// <param name="where">(Opcional) Clausula de filtro ex: <code>?where=id == 100 or id == 200</code></param>
        /// <param name="orderBy">(Opcional) Ordenação do resultado ex: <code>?orderBy=id</code></param>
        /// <param name="page">(Opcional) Página atual ex: <code>?page=2</code></param>
        /// <param name="pageSize">(Opcional) Registros por página ex: <code>?pageSize=20</code></param>
        /// <returns>Lista paginada dos objetos localizados</returns>
        /// <response code="200">Retorna a lista de objetos cadastrados</response>
        /// <response code="204">Quando a consulta não retornar resultados</response>
        /// <response code="400">Caso ocorra algum erro de validação</response>
        /// <response code="500">Se ocorrer um erro interno ou de regra de negócio</response>
        /// <response code="401">Não autorizado</response>
        /// <response code="403">Acesso negado</response>
        [HttpGet]
        [ServiceFilter(typeof(EnablePagingAttribute))]
        [ProducesResponseType(
            statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
        [ProducesResponseType(
            type: typeof(ViewModel.ApiErrorResponse),
            statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(
            type: typeof(ViewModel.ApiErrorResponse),
            statusCode: StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden)]
        public virtual async Task<IActionResult> GetAllAsync(
            [FromQuery] string[] include,
            [FromQuery] string where,
            [FromQuery] string orderBy,
            [FromQuery] int? page,
            [FromQuery] int? pageSize)
        {
            using (_logger.BeginScope($"{GetType().FullName}.{nameof(GetAllAsync)}"))
            {
                try
                {
                    System.Linq.IQueryable<T> taskResult = await _currentService.ReadAsync(
                                where: where,
                                orderBy: orderBy,
                                include: include).ConfigureAwait(false);

                    if (taskResult != null)
                    {
                        return Ok(taskResult);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);

                    return BadRequest(new ViewModel.ApiErrorResponse(new ViewModel.ApiErrorDetailResponse()
                    {
                        ErrorType = ViewModel.ErrorType.GenericException,
                        Message = ex.Message
                    }));
                }

                return NoContent();
            }
        }

        /// <summary>
        /// Recupera um objeto específico a partir do valor informado em '<paramref name="id"/>'
        /// </summary>
        /// <param name="id">Identificador único do objeto</param>
        /// <param name="include">(Opcional) Objetos para incluir no resultado ex: <code>?include=person</code></param>
        /// <returns>Objeto localizado</returns>
        /// <response code="200">Retorna o objeto</response>
        /// <response code="204">Quando a consulta não retornar resultados</response>
        /// <response code="400">Caso ocorra algum erro de validação</response>
        /// <response code="500">Se ocorrer um erro interno ou de regra de negócio</response>
        /// <response code="401">Não autorizado</response>
        /// <response code="403">Acesso negado</response>
        [HttpGet("{id}")]
        [ProducesResponseType(
            statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
        [ProducesResponseType(
            type: typeof(ViewModel.ApiErrorResponse),
            statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(
            type: typeof(ViewModel.ApiErrorResponse),
            statusCode: StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden)]
        public virtual async Task<IActionResult> GetSingleAsync(
            int id,
            [FromQuery] string[] include)
        {
            using (_logger.BeginScope($"{GetType().FullName}.{nameof(GetSingleAsync)} - #{id}"))
            {
                try
                {
                    T taskResult = await _currentService.ReadAsync(
                                        id: id,
                                        include: include).ConfigureAwait(false);

                    if (taskResult != null)
                    {
                        return Ok(taskResult);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);

                    return BadRequest(new ViewModel.ApiErrorResponse(new ViewModel.ApiErrorDetailResponse()
                    {
                        ErrorType = ViewModel.ErrorType.GenericException,
                        Message = ex.Message
                    }));
                }

                return NoContent();
            }
        }
    }
}
