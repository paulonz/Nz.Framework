namespace Nz.Api.Middleware
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Middleware para tratar exceptions não trataas
    /// </summary>
    public class UnhandledExceptionMiddleware
    {
        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Próximo passo
        /// </summary>
        private readonly RequestDelegate _next;

        /// <summary>
        /// Construtor padrão
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="next">Próximo passo</param>
        public UnhandledExceptionMiddleware(
            ILogger<UnhandledExceptionMiddleware> logger,
            RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        /// <summary>
        /// Execução
        /// </summary>
        /// <param name="context">Contexto Http</param>
        /// <returns>Ok</returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception,
                    $"Request {context.Request?.Method}: {context.Request?.Path.Value} failed");
            }
        }
    }
}
