/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Api.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Extensions;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Nz.Core.Service;

    /// <summary>
    /// Implementação padrão para controllers públicas do tipo CRUD
    /// </summary>
    /// <typeparam name="T">Model tratada pela controller</typeparam>
    [Authorize]
    public abstract class ApiControllerCRUDBase<T> : ApiControllerReadOnlyBase<T>
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
        public ApiControllerCRUDBase(
            ICRUDService<T> currentService,
            ILogger<ApiControllerCRUDBase<T>> logger) : base(currentService, logger)
        {
            _currentService = currentService;
            _logger = logger;
        }

        /// <summary>
        /// Cria um novo objeto com base nos dados informados em '<paramref name="value"/>'
        /// </summary>
        /// <param name="value">Objeto que será criado</param>
        /// <param name="include">Objetos para incluir no resultado ex: <code>?include=person</code></param>
        /// <returns>Objeto criado</returns>
        /// <response code="201">Retorna o objeto criado</response>
        /// <response code="400">Caso ocorra algum erro de validação</response>
        /// <response code="500">Se ocorrer um erro não tratado na aplicação</response>
        /// <response code="401">Não autorizado</response>
        /// <response code="403">Acesso negado</response>
        [HttpPost]
        [ProducesResponseType(
            StatusCodes.Status201Created)]
        [ProducesResponseType(
            type: typeof(ViewModel.ApiErrorResponse),
            StatusCodes.Status400BadRequest)]
        [ProducesResponseType(
            type: typeof(ViewModel.ApiErrorResponse),
            StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public virtual async Task<IActionResult> PostAsync(
            [FromBody] T value,
            [FromQuery] string[] include)
        {
            using (_logger.BeginScope($"{GetType().FullName}.{nameof(PostAsync)}"))
            {
                try
                {
                    _logger.LogInformation(value.ToString());

                    T model = await _currentService.CreateAsync(
                                        value).ConfigureAwait(false);

                    if (model != null)
                    {
                        if (include != null && include.Length > 0)
                        {
                            model = await _currentService.ReadAsync(
                                            id: model.Id,
                                            include: include).ConfigureAwait(false);
                        }

                        return Created(new Uri(HttpContext.Request.GetDisplayUrl()), model);
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

                return BadRequest(new ViewModel.ApiErrorResponse(new ViewModel.ApiErrorDetailResponse()
                {
                    ErrorType = ViewModel.ErrorType.ModelValidation,
                    Message = Validations.Controller_Invalid_Data
                }));
            }
        }

        /// <summary>
        /// Atualiza os dados de um objeto com base no '<paramref name="id"/>' e dados informados em '<paramref name="value"/>'
        /// </summary>
        /// <param name="id">Identificador único do objeto que será atualizado</param>
        /// <param name="value">Novos dados do objeto</param>
        /// <param name="include">Objetos para incluir no resultado ex: <code>?include=person</code></param>
        /// <returns>Objeto atualizado</returns>
        /// <response code="200">Retorna o objeto atualizado</response>
        /// <response code="400">Caso ocorra algum erro de validação</response>
        /// <response code="500">Se ocorrer um erro não tratado na aplicação</response>
        /// <response code="401">Não autorizado</response>
        /// <response code="403">Acesso negado</response>
        [HttpPut("{id}")]
        [ProducesResponseType(
            StatusCodes.Status200OK)]
        [ProducesResponseType(
            type: typeof(ViewModel.ApiErrorResponse),
            StatusCodes.Status400BadRequest)]
        [ProducesResponseType(
            type: typeof(ViewModel.ApiErrorResponse),
            StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public virtual async Task<IActionResult> PutAsync(
            int id,
            [FromBody] T value,
            [FromQuery] string[] include)
        {
            using (_logger.BeginScope($"{GetType().FullName}.{nameof(PutAsync)} - #{id}"))
            {
                try
                {
                    _logger.LogInformation(value.ToString());

                    T model = await _currentService.UpdateAsync(
                                        id: id,
                                        model: value).ConfigureAwait(false);

                    if (model != null)
                    {
                        if (include != null && include.Length > 0)
                        {
                            model = await _currentService.ReadAsync(
                                                model.Id,
                                                include: include).ConfigureAwait(false);
                        }

                        return Ok(model);
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

                return BadRequest(new ViewModel.ApiErrorResponse(new ViewModel.ApiErrorDetailResponse()
                {
                    ErrorType = ViewModel.ErrorType.ModelValidation,
                    Message = Validations.Controller_Invalid_Data
                }));
            }
        }

        /// <summary>
        /// Desativa um objeto com base no '<paramref name="id"/>' informado
        /// </summary>
        /// <param name="id">Id do objeto que será desativado</param>
        /// <returns>Sucesso</returns>
        /// <response code="200">Objeto desativado com sucesso</response>
        /// <response code="400">Caso ocorra algum erro de validação</response>
        /// <response code="500">Se ocorrer um erro não tratado na aplicação</response>
        /// <response code="401">Não autorizado</response>
        /// <response code="403">Acesso negado</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(
            StatusCodes.Status200OK)]
        [ProducesResponseType(
            type: typeof(ViewModel.ApiErrorResponse),
            StatusCodes.Status400BadRequest)]
        [ProducesResponseType(
            type: typeof(ViewModel.ApiErrorResponse),
            StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public virtual async Task<IActionResult> DeleteAsync(
            int id)
        {
            using (_logger.BeginScope($"{GetType().FullName}.{nameof(DeleteAsync)} - #{id}"))
            {
                try
                {
                    T model = await _currentService.DeleteAsync(id: id).ConfigureAwait(false);

                    if (model != null)
                    {
                        _logger.LogInformation(model.ToString());

                        return Ok(model);
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

                return BadRequest(new ViewModel.ApiErrorResponse(new ViewModel.ApiErrorDetailResponse()
                {
                    ErrorType = ViewModel.ErrorType.ModelValidation,
                    Message = Validations.Controller_Invalid_Data
                }));
            }
        }

        /// <summary>
        /// Reativa um objeto com base no '<paramref name="id"/>' informado
        /// </summary>
        /// <param name="id">Id do objeto que será reativado</param>
        /// <param name="include">Objetos para incluir no resultado ex: <code>?include=person</code></param>
        /// <returns>Sucesso</returns>
        /// <response code="200">Objeto reativado com sucesso</response>
        /// <response code="400">Caso ocorra algum erro de validação</response>
        /// <response code="500">Se ocorrer um erro não tratado na aplicação</response>
        /// <response code="401">Não autorizado</response>
        /// <response code="403">Acesso negado</response>
        [HttpPut("{id}/restore")]
        [ProducesResponseType(
            StatusCodes.Status200OK)]
        [ProducesResponseType(
            type: typeof(ViewModel.ApiErrorResponse),
            StatusCodes.Status400BadRequest)]
        [ProducesResponseType(
            type: typeof(ViewModel.ApiErrorResponse),
            StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public virtual async Task<IActionResult> UnDeleteAsync(
            int id,
            [FromQuery] string[] include)
        {
            using (_logger.BeginScope($"{GetType().FullName}.{nameof(UnDeleteAsync)} - #{id}"))
            {
                try
                {
                    T model = await _currentService.UnDeleteAsync(id: id).ConfigureAwait(false);

                    if (model != null)
                    {
                        _logger.LogInformation(model.ToString());

                        if (include != null && include.Length > 0)
                        {
                            model = await _currentService.ReadAsync(
                                                id: id,
                                                include: include).ConfigureAwait(false);
                        }

                        return Ok(model);
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

                return BadRequest(new ViewModel.ApiErrorResponse(new ViewModel.ApiErrorDetailResponse()
                {
                    ErrorType = ViewModel.ErrorType.ModelValidation,
                    Message = Validations.Controller_Invalid_Data
                }));
            }
        }
    }
}
