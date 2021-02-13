/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Core.Model.Impl.Jwt
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Usuário autenticado
    /// </summary>
    public class AuthUser : IAuthUser
    {
        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Http Context
        /// </summary>
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Construtor padrão
        /// </summary>
        /// <param name="logger">Logger</param>
        public AuthUser(
            ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Id do usuário
        /// </summary>
        public long UserId { get; private set; }

        /// <summary>
        /// Primeiro nome
        /// </summary>
        public string FirstName { get; private set; }

        /// <summary>
        /// Sobrenome
        /// </summary>
        public string LastName { get; private set; }

        /// <summary>
        /// Email principal
        /// </summary>
        public string Email { get; private set; }

        /// <summary>
        /// Telefone principal
        /// </summary>
        public string Phone { get; private set; }

        /// <summary>
        /// Lista de regras
        /// </summary>
        public IList<int> Roles { get; private set; }

        /// <summary>
        /// Usuário validou o email?
        /// </summary>
        public bool IsEmailValidated { get; private set; }

        /// <summary>
        /// Token para recuperação de senha
        /// </summary>
        public string RecoveryPasswordToken { get; private set; }

        /// <summary>
        /// Construtor padrão
        /// </summary>
        /// <param name="httpContextAccessor">Contexto http</param>
        public AuthUser(
            IHttpContextAccessor httpContextAccessor)
        {
            try
            {
                _httpContextAccessor = httpContextAccessor;

                string authToken = ExtractAuthToken();

                if (!string.IsNullOrEmpty(authToken))
                {
                    ExtractData(authToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        /// <summary>
        /// Extrai o token de autenticação do usuário atual
        /// </summary>
        /// <returns>Token do usuário atual</returns>
        private string ExtractAuthToken()
        {
            try
            {
                if (_httpContextAccessor?.HttpContext?.Request?.Headers != null)
                {
                    if (_httpContextAccessor.HttpContext.Request.Headers.ContainsKey("Authorization"))
                    {
                        string authToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
                        authToken = authToken.Replace("Bearer ", "", StringComparison.InvariantCultureIgnoreCase);

                        return authToken;
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
        /// Executa a extração dos dados do authToken
        /// </summary>
        /// <param name="authToken">Token de autenticação</param>
        private void ExtractData(
            string authToken)
        {
            try
            {
                JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtSecurityToken = jwtSecurityTokenHandler.ReadJwtToken(authToken);
                if (jwtSecurityToken != null && jwtSecurityToken.Claims != null && jwtSecurityToken.Claims.Any())
                {
                    foreach (Claim item in jwtSecurityToken.Claims)
                    {
                        switch (item.Type)
                        {
                            case ClaimTypes.Sid:
                                {
                                    if (int.TryParse(item.Value, out int id))
                                    {
                                        UserId = id;
                                    }
                                    break;
                                }
                            case "given_name":
                                {
                                    FirstName = item.Value;
                                    break;
                                }
                            case "family_name":
                                {
                                    LastName = item.Value;
                                    break;
                                }
                            case "email":
                                {
                                    Email = item.Value;
                                    break;
                                }
                            case ClaimTypes.MobilePhone:
                                {
                                    Phone = item.Value;
                                    break;
                                }
                            case nameof(IAuthUser.IsEmailValidated):
                                {
                                    if (bool.TryParse(item.Value, out bool isEmailValidated))
                                    {
                                        IsEmailValidated = isEmailValidated;
                                    }

                                    break;
                                }
                            case "role":
                                {
                                    if (int.TryParse(item.Value, out int role))
                                    {
                                        if (Roles == null)
                                        {
                                            Roles = new List<int>();
                                        }

                                        Roles.Add(role);
                                    }
                                    break;
                                }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}
