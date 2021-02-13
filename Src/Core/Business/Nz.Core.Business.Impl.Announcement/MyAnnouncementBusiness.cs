/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Core.Business.Impl.Announcement
{
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Nz.Core.Business.Impl.Default;
    using Nz.Core.Model.Impl.Announcement;
    using Nz.Core.UnitOfWork;

    /// <summary>
    /// Regras para tratamento dos anúncios do usuário autenticado
    /// </summary>
    public class MyAnnouncementBusiness : CRUDBusiness<Announcement>, IMyAnnouncementBusiness
    {
        /// <summary>
        /// Implementação de IUnitOfWork
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Usuário autenticado
        /// </summary>
        private readonly Model.IAuthUser _authUser;

        /// <summary>
        /// Construtor padrão
        /// </summary>
        /// <param name="authUser">Usuário autenticado</param>
        /// <param name="unitOfWork">Implementação de IUnitOfWork</param>
        /// <param name="logger">Logger</param>
        public MyAnnouncementBusiness(
            Model.IAuthUser authUser,
            IUnitOfWork unitOfWork,
            ILogger<MyAnnouncementBusiness> logger)
            : base(unitOfWork, logger)
        {
            _authUser = authUser;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        /// <summary>
        /// Cria um novo objeto no repositório
        /// </summary>
        /// <param name="model">Novo objeto</param>
        public override Task<Announcement> CreateAsync(
            Announcement model)
        {
            return base.CreateAsync(model);
        }

        /// <summary>
        /// Remove um objeto do repositório
        /// </summary>
        /// <param name="id">Identificador do ojeto</param>
        public override async Task<Announcement> DeleteAsync(
            long id)
        {
            if (await IsOwnerAsync(id).ConfigureAwait(false))
            {
                return await base.DeleteAsync(id).ConfigureAwait(false);
            }

            _logger.LogWarning($"User {_authUser.UserId} trying to {nameof(DeleteAsync)} {nameof(Announcement)} id {id}");

            return null;
        }

        /// <summary>
        /// Remove uma coleção de objetos do repositório
        /// </summary>
        /// <param name="where">Expression para localizar os objetos</param>
        public override async Task<IQueryable<Announcement>> DeleteAsync(
            string where)
        {
            where = AdjustWhere(where);

            return await base.DeleteAsync(where).ConfigureAwait(false);
        }

        /// <summary>
        /// Recupera um objeto do respositório a partir do Id
        /// </summary>
        /// <param name="id">Identificador do objeto</param>
        /// <param name="include">Objetos para incluir no retorno</param>
        /// <returns>Objeto localizado, null para não localizado</returns>
        public override async Task<Announcement> ReadAsync(
            long id,
            [Optional] string[] include)
        {
            if (await IsOwnerAsync(id).ConfigureAwait(false))
            {
                return await base.ReadAsync(id, include).ConfigureAwait(false);
            }

            _logger.LogWarning($"User {_authUser.UserId} trying to {nameof(ReadAsync)} {nameof(Announcement)} id {id}");

            return null;
        }

        /// <summary>
        /// Recupera uma coleção de objetos do respositório
        /// </summary>
        /// <param name="where">Expression para localizar os objetos</param>
        /// <param name="orderBy">Expressions de ordenação</param>
        /// <param name="include">Objetos para incluir no retorno</param>
        /// <returns>Coleção de objetos localizados</returns>
        public override async Task<IQueryable<Announcement>> ReadAsync(
            string where,
            [Optional] string orderBy,
            [Optional] string[] include)
        {
            where = AdjustWhere(where);

            return await base.ReadAsync(where, orderBy, include).ConfigureAwait(false);
        }

        /// <summary>
        /// Recupera primeira ocorrência do objeto no respositório
        /// </summary>
        /// <param name="where">Expression para localizar o objeto</param>
        /// <param name="orderBy">Expressions de ordenação</param>
        /// <param name="include">Objetos para incluir no retorno</param>
        /// <returns>Primeira ocorrência do objeto</returns>
        public override async Task<Announcement> ReadFirstAsync(
            string where,
            [Optional] string orderBy,
            [Optional] string[] include)
        {
            where = AdjustWhere(where);

            return await base.ReadFirstAsync(where, orderBy, include).ConfigureAwait(false);
        }

        /// <summary>
        /// Recupera ultima ocorrência do objeto no respositório
        /// </summary>
        /// <param name="where">Expression para localizar o objeto</param>
        /// <param name="orderBy">Expressions de ordenação</param>
        /// <param name="include">Objetos para incluir no retorno</param>
        /// <returns>Ultima ocorrência do objeto</returns>
        public override async Task<Announcement> ReadLastAsync(
            string where,
            [Optional] string orderBy,
            [Optional] string[] include)
        {
            where = AdjustWhere(where);

            return await base.ReadLastAsync(where, orderBy, include).ConfigureAwait(false);
        }

        /// <summary>
        /// Restaura um objeto do respositório
        /// </summary>
        /// <param name="id">Identificador do ojeto</param>
        public override async Task<Announcement> UnDeleteAsync(
            long id)
        {
            if (await IsOwnerAsync(id).ConfigureAwait(false))
            {
                return await base.UnDeleteAsync(id).ConfigureAwait(false);
            }

            _logger.LogWarning($"User {_authUser.UserId} trying to {nameof(UnDeleteAsync)} {nameof(Announcement)} id {id}");

            return null;
        }

        /// <summary>
        /// Restaura uma coleção de objetos do repositório
        /// </summary>
        /// <param name="where">Expression para localizar os objetos</param>
        public override async Task<IQueryable<Announcement>> UnDeleteAsync(
            string where)
        {
            where = AdjustWhere(where);

            return await base.UnDeleteAsync(where).ConfigureAwait(false);
        }

        /// <summary>
        /// Atualiza um objeto no repositório
        /// </summary>
        /// <param name="model">Objeto para ser atualizado</param>
        /// <param name="id">Identificador do objeto</param>
        public override async Task<Announcement> UpdateAsync(
            long id,
            Announcement model)
        {
            if (await IsOwnerAsync(id).ConfigureAwait(false))
            {
                return await base.UpdateAsync(id, model).ConfigureAwait(false);
            }

            _logger.LogWarning($"User {_authUser.UserId} trying to {nameof(UpdateAsync)} {nameof(Announcement)} id {id}");

            return null;
        }

        /// <summary>
        /// Verifica se o registro pertence ao usuário atualmente logado
        /// </summary>
        /// <param name="announcementId">Identificador do anúncio</param>
        /// <returns>Pertence?</returns>
        private async Task<bool> IsOwnerAsync(long announcementId)
        {
            if (announcementId > 0)
            {
                Announcement announcement = await _unitOfWork.ReadAsync<Announcement>(announcementId).ConfigureAwait(false);

                return announcement.CreatedBy == _authUser.UserId;
            }

            return false;
        }

        /// <summary>
        /// Faz o ajuste do where incluindo o filtro pelo usuário atual
        /// </summary>
        /// <param name="currentWhere">String where atual</param>
        /// <returns>String where atualizada</returns>
        private string AdjustWhere(string currentWhere)
        {
            string baseWhere = $"{nameof(Announcement.CreatedBy)} == {_authUser.UserId}";
            if (string.IsNullOrEmpty(currentWhere))
            {
                currentWhere = baseWhere;
            }
            else
            {
                currentWhere = $"{baseWhere} and {currentWhere}";
            }

            return currentWhere;
        }
    }
}
