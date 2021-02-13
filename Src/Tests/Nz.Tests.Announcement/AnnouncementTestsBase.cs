/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Tests.Announcements
{
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Nz.Api.Announcement;
    using Nz.Tests.Common.Endpoints;

    /// <summary>
    /// Base para testes de integração
    /// </summary>
    public abstract class AnnouncementTestsBase
    {
        /// <summary>
        /// Cliente Http
        /// </summary>
        protected readonly HttpClient HttpClient;

        /// <summary>
        /// Token de autenticação sem roles
        /// </summary>
        protected static string NoRoleToken => "xxxx";

        /// <summary>
        /// Token de autenticação com a role ManageAnnouncements
        /// </summary>
        protected static string ManageAnnouncementsToken => "xxxx";

        /// <summary>
        /// Construtor padrão
        /// </summary>
        public AnnouncementTestsBase()
        {
            HttpClient = new HttpClient()
            {
                BaseAddress = new Uri(Announcement.Base)
            };

            BuildTestServer();
        }

        /// <summary>
        /// Constrói o servidor de testes
        /// </summary>
        private void BuildTestServer()
        {
            BuildEnvironmentVariables();

            Task.Run(async () => await Program.Main(null));

            /// Aguardar servidor estar online
            Thread.Sleep(1000);

            while (!ServerIsOnline())
            {
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// Define as variáveis de ambiente para os testes
        /// </summary>
        private static void BuildEnvironmentVariables()
        {
            string databaseHost = "127.0.0.1";
            string databasePort = "5433";
            string databaseUser = "nz_announcement_tests";
            string databasePassword = "xxxx";
            string databaseName = BuildTemporaryDatabaseName();

            Environment.SetEnvironmentVariable("ASPNETCORE_URLS", Announcement.Base);
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
            Environment.SetEnvironmentVariable("GENERAL_BASE_URI", Announcement.Base);
            Environment.SetEnvironmentVariable("JWT_VALID_ISSUER", Announcement.BaseNoPort);
            Environment.SetEnvironmentVariable("JWT_SIGNING_KEY", "xxxx");
            Environment.SetEnvironmentVariable("JWT_VALID_AUDIENCE", Announcement.BaseNoPort);
            Environment.SetEnvironmentVariable("JWT_EXPIRES_IN_MINUTES", "60");
            Environment.SetEnvironmentVariable("EMAIL_SMTP_HOST", "smtp.mailtrap.io");
            Environment.SetEnvironmentVariable("EMAIL_FROM_EMAIL", "api@nz.com");
            Environment.SetEnvironmentVariable("EMAIL_SMTP_USER", "xxxx");
            Environment.SetEnvironmentVariable("EMAIL_FROM_NAME", "NzAPI");
            Environment.SetEnvironmentVariable("EMAIL_SMTP_PASSWORD", "xxxx");
            Environment.SetEnvironmentVariable("EMAIL_SMTP_PORT", "465");

            Environment.SetEnvironmentVariable("EMAIL_POP3_HOST", "pop3.mailtrap.io");
            Environment.SetEnvironmentVariable("EMAIL_POP3_PORT", "1100");
            Environment.SetEnvironmentVariable("EMAIL_POP3_USER", "xxxx");
            Environment.SetEnvironmentVariable("EMAIL_POP3_PASSWORD", "xxxx");

            Environment.SetEnvironmentVariable("DATABASE_CONNECTION_STRING", $"Server={databaseHost};Port={databasePort};Database={databaseName};User Id={databaseUser};Password={databasePassword};");
        }

        /// <summary>
        /// Cria o nome para um banco de dados temporário, utilizado apenas para os testes
        /// </summary>
        /// <returns>Nome do banco temporário</returns>
        private static string BuildTemporaryDatabaseName()
        {
            return $"nz_announcement_tests_{Common.Helpers.GenerateRandonIdentifier()}";
        }

        /// <summary>
        /// Verifica se o servidor está online
        /// </summary>
        /// <returns>Sucesso</returns>
        private bool ServerIsOnline()
        {
            HttpResponseMessage response = Task.Run(() => HttpClient.GetAsync(Announcement.HealthCheck_v1_0))
                .GetAwaiter()
                .GetResult();

            return response.IsSuccessStatusCode;
        }
    }
}
