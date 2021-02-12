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
    using Nz.Core.Business.Impl.Auth;
    using Nz.Core.DatabaseContext;
    using Nz.Core.Model.Impl.Auth;

    /// <summary>
    /// Serviço para tratamento da autenticação de usuários
    /// </summary>
    public class AuthService : IAuthService
    {
        /// <summary>
        /// Negócios
        /// </summary>
        private readonly IAuthBusiness _business;

        /// <summary>
        /// Contexto de banco de dados
        /// </summary>
        private readonly DbContext _dbContext;

        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Construtor padrão
        /// </summary>
        /// <param name="business">Negócios</param>
        /// <param name="dbContext">Contexto de banco de dados</param>
        /// <param name="general">Configurações gerais</param>
        /// <param name="jwt">Configurações JWT</param>
        /// <param name="encryption">Criptografia</param>
        /// <param name="logger">Logger</param>
        public AuthService(
            IAuthBusiness business,
            IDbContext dbContext,
            ILogger<AuthService> logger)
        {
            _business = business;
            _business = business;
            _dbContext = dbContext?.CurrentDbContext;
            _logger = logger;
        }

        /// <summary>
        /// Autentica o usuário
        /// </summary>
        /// <param name="model">Dados de autenticação</param>
        /// <returns>Tokens de acesso</returns>
        public async Task<Api.ViewModel.Auth.AuthResponse> SignInAsync(
            Api.ViewModel.Auth.LoginRequest model)
        {
            try
            {
                if (model != null && !string.IsNullOrEmpty(model.Email) && !string.IsNullOrEmpty(model.Password))
                {
                    User user = await _business.SignInAsync(model.Email, model.Password).ConfigureAwait(false);

                    return await BuildToken(user).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return null;
        }

        /// <summary>
        /// Atualiza o token de autenticação do usuário
        /// </summary>
        /// <param name="model">Dados de autenticação</param>
        /// <returns>Tokens de acesso</returns>
        public async Task<Api.ViewModel.Auth.AuthResponse> RefreshTokenAsync(
            Api.ViewModel.Auth.RefreshTokenRequest model)
        {
            try
            {
                if (model != null && !string.IsNullOrEmpty(model.RefreshToken))
                {
                    User user = await _business.RefreshTokenAsync(model.RefreshToken).ConfigureAwait(false);

                    return await BuildToken(user).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return null;
        }

        /// <summary>
        /// Constroi a resposta com os tokens
        /// </summary>
        /// <param name="user">Usuário</param>
        /// <returns>Tokens</returns>
        private async Task<Api.ViewModel.Auth.AuthResponse> BuildToken(
            User user)
        {
            if (user != null)
            {
                string authToken = await _business.GenerateAuthToken(user).ConfigureAwait(false);
                string refreshToken = await _business.GenerateRefreshToken(user).ConfigureAwait(false);

                if (!string.IsNullOrEmpty(authToken) && !string.IsNullOrEmpty(refreshToken))
                {
                    return new Api.ViewModel.Auth.AuthResponse()
                    {
                        Token = authToken,
                        RefreshToken = refreshToken
                    };
                }
            }

            return null;
        }

        /// <summary>
        /// Cadastra um novo usuário
        /// </summary>
        /// <param name="model">Dados de cadastro</param>
        /// <returns>Usuário cadastrado</returns>
        public async Task<User> RegisterAsync(
            Api.ViewModel.Auth.RegisterRequest model)
        {
            try
            {
                if (model != null)
                {
                    User user = await _business.RegisterAsync(new User()
                    {
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Password = model.Password,
                        Phone = model.Phone
                    }).ConfigureAwait(false);

                    if (user != null)
                    {
                        _dbContext.SaveChanges();
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
        /// Confirma o cadastro de um novo usuário
        /// </summary>
        /// <param name="model">Dados de confirmação</param>
        /// <returns>Confirmação do cadastro</returns>
        public async Task<User> ConfirmRegisterAsync(
            Api.ViewModel.Auth.ConfirmRegisterRequest model)
        {
            try
            {
                if (model != null)
                {
                    User user = await _business.ConfirmRegisterAsync(
                        model.Email,
                        model.ConfirmRegisterToken).ConfigureAwait(false);

                    if (user != null)
                    {
                        _dbContext.SaveChanges();
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
        /// Inicia processo para recuperação de senha
        /// </summary>
        /// <param name="model">Dados para recuperação de senha</param>
        /// <returns>Recuperação de senha</returns>
        public async Task ForgotPasswordAsync(
            Api.ViewModel.Auth.ForgotPasswordRequest model)
        {
            try
            {
                if (model != null)
                {
                    User user = await _business.ForgotPasswordAsync(
                        model.Email).ConfigureAwait(false);

                    if (user != null)
                    {
                        _dbContext.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        /// <summary>
        /// Confirma a recuperação de senha, definindo uma nova senha para o usuário
        /// </summary>
        /// <param name="model">Dados da recuperação de senha</param>
        /// <returns>Recuperação de senha finalizada</returns>
        public async Task ConfirmForgotPasswordAsync(
            Api.ViewModel.Auth.ConfirmForgotPasswordRequest model)
        {
            try
            {
                if (model != null)
                {
                    User user = await _business.ConfirmForgotPasswordAsync(
                        model.Email,
                        model.RecoveryPasswordToken,
                        model.NewPassword).ConfigureAwait(false);

                    if (user != null)
                    {
                        _dbContext.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        /// <summary>
        /// Altera a senha de um usuário
        /// </summary>
        /// <param name="model">Dados para a alteração de senha do usuário</param>
        /// <returns>Senha alterada</returns>
        public async Task<User> ChangePasswordAsync(
            Api.ViewModel.Auth.ChangePasswordRequest model)
        {
            try
            {
                if (model != null)
                {
                    User user = await _business.ChangePasswordAsync(
                        model.Email,
                        model.CurrentPassword,
                        model.NewPassword).ConfigureAwait(false);

                    if (user != null)
                    {
                        _dbContext.SaveChanges();
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
        /// Reenvia o código de confirmação
        /// </summary>
        /// <param name="email">Email do usuário</param>
        /// <returns>Sucesso</returns>
        public async Task ResendConfirmationCodeAsync(
            string email)
        {
            try
            {
                await _business.ResendConfirmationCodeAsync(email).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}
