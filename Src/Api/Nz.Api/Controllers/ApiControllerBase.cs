/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.Extensions.Logging;
    using Nz.Api.ViewModel;

    /// <summary>
    /// Controller base para Apis
    /// </summary>
    [Route("{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public abstract class ApiControllerBase : Controller
    {
        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Construtor padrão
        /// </summary>
        /// <param name="logger">Logger</param>
        public ApiControllerBase(
            ILogger<ApiControllerBase> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Executa antes de uma action
        /// </summary>
        /// <param name="context">Contexto da execução</param>
        public override void OnActionExecuting(
            ActionExecutingContext context)
        {
            try
            {
                if (context != null)
                {
                    CheckModelState(context);
                }

                base.OnActionExecuting(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        /// <summary>
        /// Executa antes de uma action async
        /// </summary>
        /// <param name="context">Contexto da execução</param>
        /// <param name="next">Próxima ação a ser executada</param>
        /// <returns>Task</returns>
        public override Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            try
            {
                if (context != null)
                {
                    CheckModelState(context);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return base.OnActionExecutionAsync(context, next);
        }

        /// <summary>
        /// Verifica a situação do modelo
        /// </summary>
        /// <param name="context">Contexto da action</param>
        private void CheckModelState(
            ActionExecutingContext context)
        {
            try
            {
                if (!context.ModelState.IsValid)
                {
                    ApiErrorResponse apiError = new ApiErrorResponse();

                    foreach (KeyValuePair<string, ModelStateEntry> item in context.ModelState)
                    {
                        if (item.Value.Errors != null && item.Value.Errors.Count > 0)
                        {
                            foreach (ModelError error in item.Value.Errors)
                            {
                                apiError.Errors.Add(new ApiErrorDetailResponse()
                                {
                                    ErrorType = ErrorType.ModelValidation,
                                    Message = error.ErrorMessage
                                });
                            }
                        }
                    }

                    context.Result = new BadRequestObjectResult(apiError);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}
