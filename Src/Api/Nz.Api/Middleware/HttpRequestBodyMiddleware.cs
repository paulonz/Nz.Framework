namespace Nz.Api.Middleware
{
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Middleware para tratamento do corpo das requisições
    /// </summary>
    public class HttpRequestBodyMiddleware
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
        public HttpRequestBodyMiddleware(
            ILogger<HttpRequestBodyMiddleware> logger,
            RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        /// <summary>
        /// Execução
        /// </summary>
        /// <param name="context">Contexto http</param>
        /// <returns>Ok</returns>
        public async Task Invoke(HttpContext context)
        {
            context.Request.EnableBuffering();

            StreamReader reader = new StreamReader(context.Request.Body);
            string body = await reader.ReadToEndAsync();

            _logger.LogInformation($"Request {context.Request?.Method}: {context.Request?.Path.Value}\n{body}");

            context.Request.Body.Position = 0L;

            await _next(context);
        }
    }
}
