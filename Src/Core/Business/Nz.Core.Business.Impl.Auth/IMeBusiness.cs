/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Core.Business.Impl.Auth
{
    using System.Threading.Tasks;
    using Nz.Core.Model.Impl.Auth;

    /// <summary>
    /// Interface para dados do usuário autenticado
    /// </summary>
    public interface IMeBusiness
    {
        /// <summary>
        /// Recupera os dados do usuário autenticado
        /// </summary>
        /// <param name="include">Objetos para incluir no retorno</param>
        /// <returns>Dados do usuário autenticado</returns>
        Task<User> GetMeAsync(
            string[] include);

        /// <summary>
        /// Atualiza os dados do usuário autenticado
        /// </summary>
        /// <param name="model">Novos dados do usuário autenticado</param>
        /// <returns>Dados atualizados</returns>
        Task<User> UpdateAsync(
            User model);
    }
}
