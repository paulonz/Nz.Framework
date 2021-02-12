/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Core.Service.Impl.Announcement
{
    using Microsoft.Extensions.Logging;
    using Nz.Core.Business.Impl.Announcement;
    using Nz.Core.DatabaseContext;
    using Nz.Core.Service.Impl.Default;

    /// <summary>
    /// Serviço para tratamento dos anúncios do usuário autenticado
    /// </summary>
    public class MyAnnouncementService : CRUDService<Model.Impl.Announcement.Announcement>, IMyAnnouncementService
    {
        /// <summary>
        /// Construtor padrão
        /// </summary>
        /// <param name="business">Classe de negócios</param>
        /// <param name="dbContext">Contexto de banco de dados</param>
        /// <param name="logger">Logger</param>
        public MyAnnouncementService(
            IMyAnnouncementBusiness business,
            IDbContext dbContext,
            ILogger<MyAnnouncementService> logger)
            : base(business, dbContext, logger)
        {

        }
    }
}
