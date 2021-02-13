/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Libs.Jwt.Settings
{
    /// <summary>
    /// Configurações relacionados ao token jwt
    /// </summary>
    public interface IJwtSettings
    {
        /// <summary>
        /// Chave de autenticação
        /// </summary>
        string IssuerSigningKey { get; }

        /// <summary>
        /// Audience habilitada para acessar
        /// </summary>
        string ValidAudience { get; }

        /// <summary>
        /// Issuer habilitado
        /// </summary>
        string ValidIssuer { get; }

        /// <summary>
        /// Tempo de expiração do token em minutos
        /// </summary>
        int ExpiresInMinutes { get; }
    }
}
