/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Core.Service.Impl.Auth
{
    using Microsoft.Extensions.Logging;
    using Nz.Core.Business.Impl.Auth;
    using Nz.Core.DatabaseContext;
    using Nz.Core.Model.Impl.Auth;
    using Nz.Core.Service.Impl.Default;

    /// <summary>
    /// Serviço para tratamento de dados do usuário autenticado
    /// </summary>
    public class ManageUsersService : CRUDService<User>, IManageUsersService
    {
        /// <summary>
        /// Construtor padrão
        /// </summary>
        /// <param name="business">Negócios</param>
        /// <param name="dbContext">Contexto de banco de dados</param>
        /// <param name="logger">Logger</param>
        public ManageUsersService(
            IUserBusiness business,
            IDbContext dbContext,
            ILogger<ManageUsersService> logger)
            : base(business, dbContext, logger)
        { }
    }
}
