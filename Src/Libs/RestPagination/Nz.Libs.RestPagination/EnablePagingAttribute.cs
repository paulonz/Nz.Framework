/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Libs.RestPagination
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Enable method to return paged results sets.
    /// </summary>
    /// <remarks>Metod must return IQuerable as result.</remarks>
    [AttributeUsage(
        AttributeTargets.Method,
        AllowMultiple = false)]
    public class EnablePagingAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Construtor padrão
        /// </summary>
        /// <param name="logger">Logger</param>
        public EnablePagingAttribute(
            ILogger<EnablePagingAttribute> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Invoked as the method executes, and convert the output IQuerable to a paged result set.
        /// </summary>
        /// <param name="context">Context of the request.</param>
        public override void OnActionExecuted(
            ActionExecutedContext context)
        {
            try
            {
                Microsoft.AspNetCore.Http.HttpContext httpContext = context.HttpContext;
                Microsoft.AspNetCore.Http.HttpRequest httpRequest = httpContext.Request;
                Microsoft.AspNetCore.Http.HttpResponse httpResponse = httpContext.Response;

                if (httpResponse.IsSuccessStatusCode())
                {
                    ObjectResult objectResult = (ObjectResult)context.Result;

                    if (objectResult.Value is not IQueryable queryableValue)
                    {
                        base.OnActionExecuted(context);
                    }
                    else
                    {
                        PagingInfo pagingInfo = PagingInfo.FromRequest(httpRequest);

                        PagedResult pagedResult = queryableValue.ToPagedResult(pagingInfo, httpRequest);

                        if (pagedResult.Pagination.TotalResults > 0)
                        {
                            objectResult.Value = pagedResult;
                        }
                        else
                        {
                            context.Result = new NoContentResult();
                        }
                    }
                }
                else
                {
                    base.OnActionExecuted(context);
                }

                return;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            base.OnActionExecuted(context);
        }
    }
}
