/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Api.Auth.Controllers.Private
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.Extensions.Logging;
    using Nz.Api.Controllers;
    using Nz.Core.Service.Impl.Auth;

    /// <summary>
    /// Controller para gerenciamento de usuários
    /// </summary>
    [Authorize(Roles = nameof(Core.Model.RoleType.ManageUsers))]
    public class ManageUsersController : ApiControllerCRUDBase<Core.Model.Impl.Auth.User>
    {
        /// <summary>
        /// Construtor padrão
        /// </summary>
        /// <param name="currentService">Serviço para usuários</param>
        /// <param name="logger">Logger</param>
        public ManageUsersController(
            IManageUsersService currentService,
            ILogger<ManageUsersController> logger) :
            base(currentService, logger)
        { }
    }
}
