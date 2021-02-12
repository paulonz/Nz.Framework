/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Api.Auth.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.Extensions.Logging;
    using Nz.Api.Controllers;
    using Nz.Core.Service.Impl.Announcement;

    /// <summary>
    /// Controller responsável pelos anúncios do usuário autenticado
    /// </summary>
    [Authorize]
    public class MyAnnouncementsController : ApiControllerCRUDBase<Core.Model.Impl.Announcement.Announcement>
    {
        /// <summary>
        /// Construtor padrão
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="service">Serviço para anúncios do usuário autenticado</param>
        public MyAnnouncementsController(
            ILogger<MyAnnouncementsController> logger,
            IMyAnnouncementService service) : base(service, logger)
        {

        }
    }
}
