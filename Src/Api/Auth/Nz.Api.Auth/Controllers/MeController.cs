/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Api.Auth.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Nz.Api.Controllers;
    using Nz.Core.Service.Impl.Auth;

    /// <summary>
    /// Controller responsável pelos dados pessoais do usuário autenticado
    /// </summary>
    [Authorize]
    public class MeController : ApiControllerBase
    {
        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Serviço para dados do usuário autenticado
        /// </summary>
        private readonly IMeService _service;

        /// <summary>
        /// Construtor padrão
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="service">Serviço para dados do usuário autenticado</param>
        public MeController(
            ILogger<MeController> logger,
            IMeService service) : base(logger)
        {
            _logger = logger;
            _service = service;
        }

        /// <summary>
        /// Recupera os dados do usuário autenticado
        /// </summary>
        /// <returns>Dados do usuário autenticado</returns>
        /// <response code="200">Retorna o objeto</response>
        /// <response code="204">Quando a consulta não retornar resultados</response>
        /// <response code="400">Caso ocorra algum erro de validação</response>
        /// <response code="500">Se ocorrer um erro interno ou de regra de negócio</response>
        /// <response code="401">Não autorizado</response>
        /// <response code="403">Acesso negado</response>
        [HttpGet]
        [ProducesResponseType(
            type: typeof(ViewModel.Auth.MeResponse),
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
        public virtual async Task<IActionResult> GetMeAsync(
            [FromQuery] string[] include)
        {
            using (_logger.BeginScope($"{GetType().FullName}.{nameof(GetMeAsync)}"))
            {
                try
                {
                    ViewModel.Auth.MeResponse me = await _service.GetMeAsync(include).ConfigureAwait(false);

                    if (me != null)
                    {
                        return Ok(me);
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
        /// Atualiza os dados do usuário autenticado
        /// </summary>
        /// <returns>Objeto atualizado</returns>
        /// <param name="value">Dados do usuário autenticado</param>
        /// <response code="200">Retorna o objeto atualizado</response>
        /// <response code="400">Caso ocorra algum erro de validação</response>
        /// <response code="500">Se ocorrer um erro não tratado na aplicação</response>
        /// <response code="401">Não autorizado</response>
        /// <response code="403">Acesso negado</response>
        [HttpPut]
        [ProducesResponseType(
            typeof(ViewModel.Auth.MeResponse),
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
            [FromBody] ViewModel.Auth.MeRequest value)
        {
            using (_logger.BeginScope($"{GetType().FullName}.{nameof(PutAsync)}"))
            {
                try
                {
                    _logger.LogInformation(value.ToString());

                    ViewModel.Auth.MeResponse model = await _service.UpdateAsync(value).ConfigureAwait(false);

                    if (model != null)
                    {
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
                    Message = Api.Validations.Controller_Invalid_Data
                }));
            }
        }
    }
}
