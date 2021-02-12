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
    using Nz.Core.Service.Impl.Announcement;

    /// <summary>
    /// Controller para gerenciamento de anúncios
    /// </summary>
    [Authorize(Roles = nameof(Core.Model.RoleType.ManageAnnouncements))]
    public class ManageAnnouncementsController : ApiControllerCRUDBase<Core.Model.Impl.Announcement.Announcement>
    {
        /// <summary>
        /// Construtor padrão
        /// </summary>
        /// <param name="currentService">Serviço para usuários</param>
        /// <param name="logger">Logger</param>
        public ManageAnnouncementsController(
            IManageAnnouncementsService currentService,
            ILogger<ManageAnnouncementsController> logger) :
            base(currentService, logger)
        { }
    }
}
