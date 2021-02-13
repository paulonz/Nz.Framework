/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Tests.Common.Models
{
    using System;

    /// <summary>
    /// Mock das configurações Pop3
    /// </summary>
    public class Pop3Configuration
    {
        /// <summary>
        /// Endereço do servidor
        /// </summary>
        public static string Host => Environment.GetEnvironmentVariable("EMAIL_POP3_HOST");

        /// <summary>
        /// Porta do servidor
        /// </summary>
        public static int Port
        {
            get
            {
                string stringPort = Environment.GetEnvironmentVariable("EMAIL_POP3_PORT");

                if (int.TryParse(stringPort, out int port))
                {
                    return port;
                }

                return 0;
            }
        }

        /// <summary>
        /// Usuário para autenticação
        /// </summary>
        public static string User => Environment.GetEnvironmentVariable("EMAIL_POP3_USER");

        /// <summary>
        /// Senha do usuário
        /// </summary>
        public static string Password => Environment.GetEnvironmentVariable("EMAIL_POP3_PASSWORD");
    }
}
