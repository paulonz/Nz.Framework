/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Core.Service.Impl.Auth
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Nz.Common.Helpers;
    using Nz.Core.Business.Impl.Auth;
    using Nz.Core.DatabaseContext;
    using Nz.Core.Model.Impl.Auth;

    /// <summary>
    /// Serviço para tratamento de dados do usuário autenticado
    /// </summary>
    public class MeService : IMeService
    {
        /// <summary>
        /// Negócios
        /// </summary>
        private readonly IMeBusiness _business;

        /// <summary>
        /// Contexto de banco de dados
        /// </summary>
        private readonly DbContext _dbContext;

        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Parser Helper
        /// </summary>
        private readonly IParserHelper _parserHelper;

        /// <summary>
        /// Construtor padrão
        /// </summary>
        /// <param name="business">Negócios</param>
        /// <param name="dbContext">Contexto de banco de dados</param>
        /// <param name="logger">Logger</param>
        /// <param name="parserHelper">Parser Helper</param>
        public MeService(
            IMeBusiness business,
            IDbContext dbContext,
            ILogger<AuthService> logger,
            IParserHelper parserHelper)
        {
            _business = business;
            _dbContext = dbContext?.CurrentDbContext;
            _logger = logger;
            _parserHelper = parserHelper;
        }

        /// <summary>
        /// Recupera os dados do usuário autenticado
        /// </summary>
        /// <param name="include">Objetos para incluir no retorno</param>
        /// <returns>Dados do usuário autenticado</returns>
        public async Task<Api.ViewModel.Auth.MeResponse> GetMeAsync(
            string[] include)
        {
            try
            {

                User user = await _business.GetMeAsync(include).ConfigureAwait(false);

                if (user != null)
                {
                    Api.ViewModel.Auth.MeResponse meResponse = _parserHelper.To<Api.ViewModel.Auth.MeResponse, User>(user);

                    if (meResponse != null)
                    {
                        return meResponse;
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
        public async Task<Api.ViewModel.Auth.MeResponse> UpdateAsync(
            Api.ViewModel.Auth.MeRequest model)
        {
            try
            {
                User user = await _business.GetMeAsync(null).ConfigureAwait(false);

                if (user != null)
                {
                    user.FirstName = model?.FirstName ?? user.FirstName;
                    user.LastName = model?.LastName ?? user.LastName;
                    user.Phone = model?.Phone ?? user.Phone;

                    user = await _business.UpdateAsync(user).ConfigureAwait(false);

                    if (user != null)
                    {
                        await _dbContext.SaveChangesAsync().ConfigureAwait(false);

                        Api.ViewModel.Auth.MeResponse meResponse = _parserHelper.To<Api.ViewModel.Auth.MeResponse, User>(user);

                        if (meResponse != null)
                        {
                            return meResponse;
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
