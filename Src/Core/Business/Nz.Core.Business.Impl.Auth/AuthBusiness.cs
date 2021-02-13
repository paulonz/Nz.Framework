/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Core.Business.Impl.Auth
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Tokens;
    using Nz.Common.GeneralSettings;
    using Nz.Core.Model;
    using Nz.Core.Model.Impl.Auth;
    using Nz.Libs.EmailSender;
    using Nz.Libs.Encryption;
    using Nz.Libs.Jwt.Settings;
    using Nz.Libs.MessageTemplate;

    /// <summary>
    /// Classe para autenticação
    /// </summary>
    public partial class AuthBusiness : IAuthBusiness
    {
        /// <summary>
        /// Criptografia
        /// </summary>
        private readonly IEncryption _encryption;

        /// <summary>
        /// Negócios para usuários
        /// </summary>
        private readonly IUserBusiness _userBusiness;

        /// <summary>
        /// Configurações gerais
        /// </summary>
        private readonly IGeneralSettings _general;

        /// <summary>
        /// Configurações Jwt
        /// </summary>
        private readonly IJwtSettings _jwt;

        /// <summary>
        /// Envio de email
        /// </summary>
        private readonly IEmailSender _emailSender;

        /// <summary>
        /// Template de mensagens
        /// </summary>
        private readonly IMessageTemplate _messageTemplate;

        /// <summary>
        /// Usuário autenticado
        /// </summary>
        private readonly IAuthUser _authUser;

        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Construtor padrão
        /// </summary>
        /// <param name="encryption">Objeto de criptografia</param>
        /// <param name="userBusiness">Negócios para usuários</param>
        /// <param name="general">Configurações gerais</param>
        /// <param name="jwt">Configurações JWT</param>
        /// <param name="emailSender">Envio de email</param>
        /// <param name="messageTemplate">Template para mensagens</param>
        /// <param name="authUser">Usuário autenticado</param>
        /// <param name="logger">Logger</param>
        public AuthBusiness(
            IEncryption encryption,
            IUserBusiness userBusiness,
            IGeneralSettings general,
            IJwtSettings jwt,
            IEmailSender emailSender,
            IMessageTemplate messageTemplate,
            IAuthUser authUser,
            ILogger<AuthBusiness> logger)
        {
            _encryption = encryption;
            _userBusiness = userBusiness;
            _general = general;
            _jwt = jwt;
            _emailSender = emailSender;
            _messageTemplate = messageTemplate;
            _authUser = authUser;
            _logger = logger;
        }

        /// <summary>
        /// Realiza a autenticação de um usuário
        /// </summary>
        /// <param name="email">Login de acesso do usuário</param>
        /// <param name="password">Senha de acesso do usuário</param>
        /// <returns>Array com Token e RefreshToken</returns>
        public async Task<User> SignInAsync(
            string email,
            string password)
        {
            try
            {
                if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
                {
                    password = _encryption.Encrypt(password);

                    User user = await _userBusiness.ReadFirstAsync(
                        $"{nameof(User.Email)} == \"{email}\" && {nameof(User.Password)} == \"{password}\" && {nameof(User.IsEmailValidated)}",
                        include: new string[]
                            {
                                nameof(User.UserRoles)
                            }
                        ).ConfigureAwait(false);

                    return user;
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
        /// <param name="refreshToken">Dados de autenticação</param>
        /// <returns>Tokens de acesso</returns>
        public async Task<User> RefreshTokenAsync(
            string refreshToken)
        {
            try
            {
                if (!string.IsNullOrEmpty(refreshToken))
                {
                    string email = ExtractEmailFromRefreshToken(refreshToken);

                    if (!string.IsNullOrEmpty(email))
                    {
                        User user = await _userBusiness.ReadFirstAsync(
                        $"{nameof(User.Email)} == \"{email}\"",
                        include: new string[]
                            {
                                nameof(User.UserRoles)
                            }
                        ).ConfigureAwait(false);

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
        /// Gera o token de autenticação de um usuário
        /// </summary>
        /// <param name="user">Usuário</param>
        /// <returns>Token de autenticação</returns>
        public async Task<string> GenerateAuthToken(
            User user)
        {
            try
            {
                return await Task.Run(() =>
                {
                    JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

                    SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(GetClaims(user)),
                        Expires = _general.CurrentDateTime.AddMinutes(_jwt.ExpiresInMinutes),
                        IssuedAt = _general.CurrentDateTime.AddMinutes(_jwt.ExpiresInMinutes),
                        SigningCredentials = new SigningCredentials(
                            new SymmetricSecurityKey(
                                _general.DefaultEncoding.GetBytes(
                                    _jwt.IssuerSigningKey)),
                            SecurityAlgorithms.HmacSha256Signature),
                        Audience = _jwt.ValidAudience,
                        Issuer = _jwt.ValidIssuer
                    };

                    SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

                    return tokenHandler.WriteToken(token);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return null;
        }

        /// <summary>
        /// Gera o token para atualização da sessão do usuário
        /// </summary>
        /// <param name="user">Usuário</param>
        /// <returns>Token de atualização</returns>
        public async Task<string> GenerateRefreshToken(
            User user)
        {
            try
            {
                return await Task.Run(() =>
                {
                    JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

                    SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new List<Claim>() { new Claim(ClaimTypes.Email, user.Email) }),
                        SigningCredentials = new SigningCredentials(
                            new SymmetricSecurityKey(
                                _general.DefaultEncoding.GetBytes(
                                    _jwt.IssuerSigningKey)),
                            SecurityAlgorithms.HmacSha256Signature),
                        Audience = _jwt.ValidAudience,
                        Issuer = _jwt.ValidIssuer
                    };

                    SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

                    return tokenHandler.WriteToken(token);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return null;
        }

        /// <summary>
        /// Cadastra um novo usuário
        /// </summary>
        /// <param name="model">Dados de cadastro</param>
        /// <returns>Usuário cadastrado</returns>
        public async Task<User> RegisterAsync(
            User model)
        {
            try
            {
                return await _userBusiness.CreateAsync(model).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return null;
        }

        /// <summary>
        /// Confirmação de registro de um novo usuário
        /// </summary>
        /// <param name="email">Email da confirmação</param>
        /// <param name="confirmationToken">Token da confirmação</param>
        /// <returns>Sucesso</returns>
        public async Task<User> ConfirmRegisterAsync(
            string email,
            string confirmationToken)
        {
            try
            {
                User user = await _userBusiness.ReadFirstAsync(
                        $"{nameof(User.Email)} == \"{email}\" && {nameof(user.IsEmailValidated)} == false")
                        .ConfigureAwait(false);

                if (user != null)
                {
                    string registerToken = _encryption.Encrypt(email);

                    if (confirmationToken.Equals(registerToken, StringComparison.InvariantCultureIgnoreCase))
                    {
                        user.IsEmailValidated = true;
                        user = await _userBusiness.UpdateAsync(user.Id, user);

                        if (user != null)
                        {
                            await SendMailAsync(
                                        MessageTemplateType.EmailConfirmNewUserRegister,
                                        new
                                        {
                                            user.FirstName,
                                            user.Email,
                                            _general.BaseUri
                                        },
                                        user.FirstName,
                                        email).ConfigureAwait(false);

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

        /// <summary>
        /// Inicio da etapa para recuperação de senha
        /// </summary>
        /// <param name="email">Email para recuperação de senha</param>
        /// <returns>Sucesso</returns>
        public async Task<User> ForgotPasswordAsync(
            string email)
        {
            try
            {
                User user = await _userBusiness.ReadFirstAsync(
                        $"{nameof(user.Email)} == \"{email}\"")
                        .ConfigureAwait(false);

                if (user != null)
                {
                    user.RecoveryPasswordToken = _encryption.Encrypt($"{user.Email}_{_general.CurrentDateTime}");
                    user = await _userBusiness.UpdateAsync(user.Id, user).ConfigureAwait(false);

                    if (user != null)
                    {
                        await SendMailAsync(
                                    MessageTemplateType.EmailForgotPassword,
                                    new
                                    {
                                        user.FirstName,
                                        user.RecoveryPasswordToken,
                                        _general.BaseUri
                                    },
                                    user.FirstName,
                                    email).ConfigureAwait(false);

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
        /// Confirmação da recuperação de senha
        /// </summary>
        /// <param name="email">Email para a confirmação</param>
        /// <param name="recoveryPasswordToken">Token para recuperação de senha</param>
        /// <param name="newPassword">Nova senha para o usuário</param>
        /// <returns>Sucesso</returns>
        public async Task<User> ConfirmForgotPasswordAsync(
            string email,
            string recoveryPasswordToken,
            string newPassword)
        {
            try
            {
                if (!string.IsNullOrEmpty(email) &&
                    !string.IsNullOrEmpty(recoveryPasswordToken) &&
                    !string.IsNullOrEmpty(newPassword))
                {
                    User user = await _userBusiness.ReadFirstAsync(
                        $"{nameof(User.Email)} == \"{email}\" && {nameof(User.RecoveryPasswordToken)} == \"{recoveryPasswordToken}\"")
                        .ConfigureAwait(false);

                    if (user != null)
                    {
                        user.Password = _encryption.Encrypt(newPassword);
                        user.RecoveryPasswordToken = string.Empty;

                        user = await _userBusiness.UpdateAsync(user.Id, user).ConfigureAwait(false);

                        if (user != null)
                        {
                            await SendMailAsync(
                                        MessageTemplateType.EmailConfirmForgotPassword, new
                                        {
                                            user.FirstName,
                                            user.Email,
                                            _general.BaseUri
                                        },
                                        user.FirstName,
                                        email).ConfigureAwait(false);

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

        /// <summary>
        /// Alteração de senha de um usuário
        /// </summary>
        /// <param name="email">Login do usuário</param>
        /// <param name="currentPassword">Senha atual</param>
        /// <param name="newPassword">Nova senha</param>
        /// <returns>Sucesso</returns>
        public async Task<User> ChangePasswordAsync(
            string email,
            string currentPassword,
            string newPassword)
        {
            try
            {
                if (!string.IsNullOrEmpty(email) &&
                    !string.IsNullOrEmpty(currentPassword) &&
                    !string.IsNullOrEmpty(newPassword))
                {
                    if (_authUser.Email == email)
                    {
                        currentPassword = _encryption.Encrypt(currentPassword);

                        User user = await _userBusiness.ReadFirstAsync(
                            $"{nameof(User.Email)} == \"{email}\" && {nameof(User.Password)} == \"{currentPassword}\"")
                            .ConfigureAwait(false);

                        if (user != null)
                        {
                            user.Password = _encryption.Encrypt(newPassword);

                            user = await _userBusiness.UpdateAsync(user.Id, user).ConfigureAwait(false);

                            if (user != null)
                            {
                                await SendMailAsync(
                                            MessageTemplateType.EmailChangePassword,
                                            new
                                            {
                                                user.FirstName,
                                                user.Email
                                            },
                                            user.FirstName,
                                            email).ConfigureAwait(false);

                                return user;
                            }
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

        /// <summary>
        /// Lista de regras do usuário
        /// </summary>
        /// <param name="user">Usuário</param>
        /// <returns>Lista de regras</returns>
        private IList<Claim> GetClaims(
            User user)
        {
            try
            {
                IList<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Sid, user.Id.ToString(System.Globalization.CultureInfo.InvariantCulture)),
                    new Claim(ClaimTypes.GivenName, user.FirstName),
                    new Claim(ClaimTypes.Surname, user.LastName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.MobilePhone, user.Phone),
                    new Claim(nameof(IAuthUser.IsEmailValidated), user.IsEmailValidated.ToString(System.Globalization.CultureInfo.InvariantCulture))
                };

                if (user?.UserRoles != null && user.UserRoles.Any(w => !w.ExcludedOn.HasValue))
                {
                    foreach (UserRole item in user.UserRoles.Where(w => !w.ExcludedOn.HasValue))
                    {
                        claims.Add(new Claim(ClaimTypes.Role, item.RoleType.ToString()));
                    }
                }

                return claims;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return null;
        }

        /// <summary>
        /// Recupera o email do token de refresh
        /// </summary>
        /// <param name="refreshToken">Token de refresh</param>
        /// <returns>Email associado</returns>
        private string ExtractEmailFromRefreshToken(
            string refreshToken)
        {
            try
            {
                JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtSecurityToken = jwtSecurityTokenHandler.ReadJwtToken(refreshToken);
                if (jwtSecurityToken != null && jwtSecurityToken.Claims != null && jwtSecurityToken.Claims.Any())
                {
                    if (jwtSecurityToken.Claims.Any(w => w.Type == "email"))
                    {
                        return jwtSecurityToken.Claims.First(w => w.Type == "email").Value;
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
        /// Envio de email
        /// </summary>
        /// <param name="messageTemplateType">Template da mensagem</param>
        /// <param name="data">Dados da mensagem</param>
        /// <param name="firstName">Primeiro nome do destinatário</param>
        /// <param name="toEmail">Email do destinatário</param>
        /// <returns></returns>
        private async Task SendMailAsync(
            MessageTemplateType messageTemplateType,
            dynamic data,
            string firstName,
            string toEmail)
        {
            string message = _messageTemplate.GetTemplate(
                                    messageTemplateType,
                                    data);

            string subject = _messageTemplate.GetSubject(
                                    messageTemplateType,
                                    new
                                    {
                                        FirstName = firstName
                                    });

            await _emailSender.SendAsync(
                        toEmail,
                        subject,
                        message).ConfigureAwait(false);
        }

        /// <summary>
        /// Reenvia o código de confirmação
        /// </summary>
        /// <param name="email">Email do usuário</param>
        /// <returns>Sucesso</returns>
        public async Task ResendConfirmationCodeAsync(
            string email)
        {
            User user = await _userBusiness.ReadFirstAsync($"{nameof(User.Email)} == {email} AND {nameof(User.IsEmailValidated)} == {false}").ConfigureAwait(false);

            if (user != null)
            {
                await SendMailAsync(
                    MessageTemplateType.EmailNewUserRegister,
                    new
                    {
                        user.FirstName,
                        ConfirmationToken = _encryption.Encrypt(user.Email),
                        _general.BaseUri
                    },
                    user.FirstName,
                    user.Email);
            }
        }
    }
}
