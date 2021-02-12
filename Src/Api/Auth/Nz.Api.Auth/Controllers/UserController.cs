/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Api.Auth.Controllers.Public
{
    using Microsoft.Extensions.Logging;
    using Nz.Api.Controllers;
    using Nz.Core.Service.Impl.Auth;

    /// <summary>
    /// Controller somente leitura para usuários
    /// </summary>
    public class UserController : ApiControllerReadOnlyBase<Core.Model.Impl.Auth.User>
    {
        /// <summary>
        /// Construtor padrão
        /// </summary>
        /// <param name="currentService">Serviço para usuários</param>
        /// <param name="logger">Logger</param>
        public UserController(
            IUserService currentService,
            ILogger<UserController> logger) :
            base(currentService, logger)
        { }
    }
}
