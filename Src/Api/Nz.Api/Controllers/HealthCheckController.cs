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
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Nz.Core.Service;

    /// <summary>
    /// Controller responsável pela autenticação de usuários
    /// </summary>
    [AllowAnonymous]
    public class HealthCheckController : ApiControllerBase
    {
        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Serviço de saúde da aplicação
        /// </summary>
        private readonly IApplicationHealthService _applicationHealthService;

        /// <summary>
        /// Construtor padrão
        /// </summary>
        /// <param name="applicationHealthService">Serviço de saúde da aplicação</param>
        /// <param name="logger">Logger</param>
        public HealthCheckController(
            IApplicationHealthService applicationHealthService,
            ILogger<HealthCheckController> logger) : base(logger)
        {
            _applicationHealthService = applicationHealthService;
            _logger = logger;
        }

        /// <summary>
        /// Verifica se a aplicação está online e acessando o banco de dados
        /// </summary>
        /// <returns>Aplicação online</returns>
        /// <response code="200">Aplicação OK</response>
        /// <response code="400">Caso ocorra algum erro tratado, aplicação NOK</response>
        /// <response code="500">Se ocorrer um erro interno, aplicação NOK</response>
        [HttpGet]
        [ProducesResponseType(
            statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(
            type: typeof(ViewModel.ApiErrorResponse),
            statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(
            type: typeof(ViewModel.ApiErrorResponse),
            statusCode: StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {
            using (_logger.BeginScope($"{GetType().FullName}.{nameof(Get)}"))
            {
                try
                {
                    bool ok = await _applicationHealthService.HealthCheckAsync().ConfigureAwait(false);

                    if (ok)
                    {
                        return Ok();
                    }
                    else
                    {
                        return BadRequest(new ViewModel.ApiErrorResponse(new ViewModel.ApiErrorDetailResponse()
                        {
                            ErrorType = ViewModel.ErrorType.HealthCheckFail
                        }));
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
            }
        }
    }
}
