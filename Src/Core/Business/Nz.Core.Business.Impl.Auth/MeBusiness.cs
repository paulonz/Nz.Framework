/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Core.Business.Impl.Auth
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Nz.Core.Model.Impl.Auth;
    using Nz.Core.UnitOfWork;

    /// <summary>
    /// Negócios para usuário autenticado
    /// </summary>
    public class MeBusiness : IMeBusiness
    {
        /// <summary>
        /// Usuário autenticado
        /// </summary>
        private readonly Model.IAuthUser _authUser;

        /// <summary>
        /// Implementação de IUnitOfWork
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Construtor padrão
        /// </summary>
        /// <param name="authUser">Usuário autenticado</param>
        /// <param name="unitOfWork">Acesso a dados</param>
        /// <param name="logger">Logger</param>
        public MeBusiness(
            Model.IAuthUser authUser,
            IUnitOfWork unitOfWork,
            ILogger<MeBusiness> logger)
        {
            _authUser = authUser;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Recupera os dados do usuário autenticado
        /// </summary>
        /// <param name="include">Objetos para incluir no retorno</param>
        /// <returns>Dados do usuário autenticado</returns>
        public async Task<User> GetMeAsync(
            string[] include)
        {
            try
            {
                if (_authUser != null && _authUser.UserId > 0)
                {
                    User user = await _unitOfWork.ReadFirstAsync<User>($"id == {_authUser.UserId}", include: include).ConfigureAwait(false);

                    if (user != null)
                    {
                        return user;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return null;
        }

        /// <summary>
        /// Atualiza os dados do usuário autenticado
        /// </summary>
        /// <param name="model">Novos dados do usuário autenticado</param>
        /// <returns>Dados atualizados</returns>
        public async Task<User> UpdateAsync(
            User model)
        {
            try
            {
                if (_authUser != null && _authUser.UserId > 0 && model != null)
                {
                    User user = await _unitOfWork.ReadFirstAsync<User>($"id == {_authUser.UserId}").ConfigureAwait(false);

                    if (user != null)
                    {
                        user.FirstName = model?.FirstName ?? user.FirstName;
                        user.LastName = model?.LastName ?? user.LastName;
                        user.Phone = model?.Phone ?? user.Phone;

                        user = await _unitOfWork.UpdateAsync(user).ConfigureAwait(false);

                        if (user != null)
                        {
                            return user;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return null;
        }
    }
}
