/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Libs.Jwt.Settings.Impl.Default
{
    using System;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Configurações relacionados ao token jwt
    /// </summary>
    public class JwtSettings : IJwtSettings
    {
        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Construtor padrão
        /// </summary>
        /// <param name="logger">Logger</param>
        public JwtSettings(
            ILogger<JwtSettings> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Chave de autenticação
        /// </summary>
        public string IssuerSigningKey
        {
            get
            {
                try
                {
                    return Environment.GetEnvironmentVariable(EnvironmentVariable.IssuerSigningKey);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }

                return null;
            }
        }

        /// <summary>
        /// Audience habilitada para acessar
        /// </summary>
        public string ValidAudience
        {
            get
            {
                try
                {
                    return Environment.GetEnvironmentVariable(EnvironmentVariable.ValidAudience);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }

                return null;
            }
        }

        /// <summary>
        /// Issuer habilitado
        /// </summary>
        public string ValidIssuer
        {
            get
            {
                try
                {
                    return Environment.GetEnvironmentVariable(EnvironmentVariable.ValidIssuer);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }

                return null;
            }
        }

        /// <summary>
        /// Tempo de expiração do token em minutos
        /// </summary>
        public int ExpiresInMinutes
        {
            get
            {
                try
                {
                    string time = Environment.GetEnvironmentVariable(EnvironmentVariable.ExpiresInMinutes);

                    if (!string.IsNullOrEmpty(time))
                    {
                        if (int.TryParse(time, out int result))
                        {
                            return result;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }

                return 2;
            }
        }
    }
}
