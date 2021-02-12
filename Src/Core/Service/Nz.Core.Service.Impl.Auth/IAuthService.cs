/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Core.Service.Impl.Auth
{
    using System.Threading.Tasks;
    using Nz.Core.Model.Impl.Auth;

    public interface IAuthService
    {
        /// <summary>
        /// Autentica o usuário
        /// </summary>
        /// <param name="model">Dados de autenticação</param>
        /// <returns>Tokens de acesso</returns>
        Task<Api.ViewModel.Auth.AuthResponse> SignInAsync(
            Api.ViewModel.Auth.LoginRequest model);

        /// <summary>
        /// Atualiza o token de autenticação do usuário
        /// </summary>
        /// <param name="model">Dados de autenticação</param>
        /// <returns>Tokens de acesso</returns>
        Task<Api.ViewModel.Auth.AuthResponse> RefreshTokenAsync(
            Api.ViewModel.Auth.RefreshTokenRequest model);

        /// <summary>
        /// Cadastra um novo usuário
        /// </summary>
        /// <param name="model">Dados de cadastro</param>
        /// <returns>Usuário cadastrado</returns>
        Task<User> RegisterAsync(
            Api.ViewModel.Auth.RegisterRequest model);

        /// <summary>
        /// Confirma o cadastro de um novo usuário
        /// </summary>
        /// <param name="model">Dados de confirmação</param>
        /// <returns>Confirmação do cadastro</returns>
        Task<User> ConfirmRegisterAsync(
            Api.ViewModel.Auth.ConfirmRegisterRequest model);

        /// <summary>
        /// Inicia processo para recuperação de senha
        /// </summary>
        /// <param name="model">Dados para recuperação de senha</param>
        /// <returns>Recuperação de senha</returns>
        Task ForgotPasswordAsync(
            Api.ViewModel.Auth.ForgotPasswordRequest model);

        /// <summary>
        /// Confirma a recuperação de senha, definindo uma nova senha para o usuário
        /// </summary>
        /// <param name="model">Dados da recuperação de senha</param>
        /// <returns>Recuperação de senha finalizada</returns>
        Task ConfirmForgotPasswordAsync(
            Api.ViewModel.Auth.ConfirmForgotPasswordRequest model);

        /// <summary>
        /// Altera a senha de um usuário
        /// </summary>
        /// <param name="model">Dados para a alteração de senha do usuário</param>
        /// <returns>Senha alterada</returns>
        Task<User> ChangePasswordAsync(
            Api.ViewModel.Auth.ChangePasswordRequest model);

        /// <summary>
        /// Reenvia o código de confirmação
        /// </summary>
        /// <param name="email">Email do usuário</param>
        /// <returns>Sucesso</returns>
        Task ResendConfirmationCodeAsync(
            string email);
    }
}
