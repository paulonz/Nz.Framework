/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Core.Business.Impl.Announcement
{
    using Microsoft.Extensions.Logging;
    using Nz.Core.Business.Impl.Default;
    using Nz.Core.Model.Impl.Announcement;
    using Nz.Core.UnitOfWork;

    /// <summary>
    /// Regras para tratamento dos anúncios
    /// </summary>
    public class ManageAnnouncementsBusiness : CRUDBusiness<Announcement>, IManageAnnouncementsBusiness
    {
        /// <summary>
        /// Construtor padrão
        /// </summary>
        /// <param name="unitOfWork">Implementação de IUnitOfWork</param>
        /// <param name="logger">Logger</param>
        public ManageAnnouncementsBusiness(
            IUnitOfWork unitOfWork,
            ILogger<ManageAnnouncementsBusiness> logger)
            : base(unitOfWork, logger) { }
    }
}
