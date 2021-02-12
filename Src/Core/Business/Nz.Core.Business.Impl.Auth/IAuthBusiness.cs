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
    /// Interface para regras de negócio para autenticação
    /// </summary>
    public interface IAuthBusiness
    {
        /// <summary>
        /// Realiza a autenticação de um usuário
        /// </summary>
        /// <param name="email">Login de acesso do usuário</param>
        /// <param name="password">Senha de acesso do usuário</param>
        /// <returns>Array com Token e RefreshToken</returns>
        Task<User> SignInAsync(
            string email,
            string password);

        /// <summary>
        /// Atualiza o token de autenticação do usuário
        /// </summary>
        /// <param name="refreshToken">Dados de autenticação</param>
        /// <returns>Tokens de acesso</returns>
        Task<User> RefreshTokenAsync(
            string refreshToken);

        /// <summary>
        /// Gera o token de autenticação de um usuário
        /// </summary>
        /// <param name="user">Usuário</param>
        /// <returns>Token de autenticação</returns>
        Task<string> GenerateAuthToken(
            User user);

        /// <summary>
        /// Gera o token para atualização da sessão do usuário
        /// </summary>
        /// <param name="user">Usuário</param>
        /// <returns>Token de atualização</returns>
        Task<string> GenerateRefreshToken(
            User user);

        /// <summary>
        /// Cadastra um novo usuário
        /// </summary>
        /// <param name="model">Dados de cadastro</param>
        /// <returns>Usuário cadastrado</returns>
        Task<User> RegisterAsync(
            User model);

        /// <summary>
        /// Confirmação de registro de um novo usuário
        /// </summary>
        /// <param name="email">Email da confirmação</param>
        /// <param name="confirmationToken">Token da confirmação</param>
        /// <returns>Sucesso</returns>
        Task<User> ConfirmRegisterAsync(
            string email,
            string confirmationToken);

        /// <summary>
        /// Inicio da etapa para recuperação de senha
        /// </summary>
        /// <param name="email">Email para recuperação de senha</param>
        /// <returns>Sucesso</returns>
        Task<User> ForgotPasswordAsync(
            string email);

        /// <summary>
        /// Confirmação da recuperação de senha
        /// </summary>
        /// <param name="email">Email para a confirmação</param>
        /// <param name="recoveryPasswordToken">Token para recuperação de senha</param>
        /// <param name="newPassword">Nova senha para o usuário</param>
        /// <returns>Sucesso</returns>
        Task<User> ConfirmForgotPasswordAsync(
            string email,
            string recoveryPasswordToken,
            string newPassword);

        /// <summary>
        /// Alteração de senha de um usuário
        /// </summary>
        /// <param name="email">Login do usuário</param>
        /// <param name="currentPassword">Senha atual</param>
        /// <param name="newPassword">Nova senha</param>
        /// <returns>Sucesso</returns>
        Task<User> ChangePasswordAsync(
            string email,
            string currentPassword,
            string newPassword);

        /// <summary>
        /// Reenvia o código de confirmação
        /// </summary>
        /// <param name="email">Email do usuário</param>
        /// <returns>Sucesso</returns>
        Task ResendConfirmationCodeAsync(
            string email);
    }
}
