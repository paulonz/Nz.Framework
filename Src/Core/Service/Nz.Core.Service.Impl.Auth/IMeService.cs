/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Core.Service.Impl.Auth
{
    using System.Threading.Tasks;

    /// <summary>
    /// Serviço para tratamento de dados do usuário autenticado
    /// </summary>
    public interface IMeService
    {
        /// <summary>
        /// Recupera os dados do usuário autenticado
        /// </summary>
        /// <param name="include">Objetos para incluir no retorno</param>
        /// <returns>Tokens de acesso</returns>
        Task<Api.ViewModel.Auth.MeResponse> GetMeAsync(
            string[] include);

        /// <summary>
        /// Atualiza os dados do usuário autenticado
        /// </summary>
        /// <param name="model">Dados do usuário autenticado</param>
        /// <returns>Dados atualizados</returns>
        Task<Api.ViewModel.Auth.MeResponse> UpdateAsync(
            Api.ViewModel.Auth.MeRequest model);
    }
}
