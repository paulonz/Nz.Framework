/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Tests.Common.Scenarios
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Newtonsoft.Json;
    using Npgsql;
    using Nz.Tests.Common;
    using Nz.Tests.Common.Models;
    using Nz.Tests.Common.Models.Auth;

    public class AuthCommonActions
    {
        /// <summary>
        /// Cliente Http
        /// </summary>
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Construtor padrão
        /// </summary>
        /// <param name="httpClient">Cliente Http</param>
        public AuthCommonActions(
            HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Registar um novo usuário
        /// </summary>
        /// <param name="registerRequest">Dados para o registro</param>
        /// <returns></returns>
        public async Task<RegisterResponse> RegisterNewUserAsync(
            RegisterRequest registerRequest)
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;

            HttpResponseMessage response = await _httpClient.PostAsync(
                Endpoints.Auth.Register_v1_0,
                registerRequest.ToStringContent()).ConfigureAwait(false);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            string bodyResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            RegisterResponse registerResponse = JsonConvert.DeserializeObject<RegisterResponse>(
                    bodyResponse);

            registerResponse.Should().NotBeNull();
            registerResponse.FirstName.Should().Be(registerRequest.FirstName);
            registerResponse.LastName.Should().Be(registerRequest.LastName);
            registerResponse.Email.Should().Be(registerRequest.Email);
            registerResponse.Phone.Should().Be(registerRequest.Phone);
            registerResponse.IsEmailValidated.Should().BeFalse();
            registerResponse.RecoveryPasswordToken.Should().BeNullOrEmpty();

            return registerResponse;
        }

        /// <summary>
        /// Tentativa de autenticar um usuário antes que ele valide o email
        /// </summary>
        /// <param name="signinRequest">Dados para autenticação</param>
        /// <returns></returns>
        public async Task<ErrorListResponse> TryAuthUserWithoutEmailValidatedAsync(
            SigninRequest signinRequest)
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;

            HttpResponseMessage response = await _httpClient.PostAsync(
                Endpoints.Auth.Signin_v1_0,
                signinRequest.ToStringContent()).ConfigureAwait(false);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            string bodyResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            ErrorListResponse errorListResponse = JsonConvert.DeserializeObject<ErrorListResponse>(
                    bodyResponse);

            errorListResponse.Should().NotBeNull();
            errorListResponse.Errors.Should().NotBeNull();
            errorListResponse.Errors.Count(w => w.ErrorType == 1 && w.Message == "Usuário ou senha inválidos").Should().Be(1);

            return errorListResponse;
        }

        /// <summary>
        /// Recuperar o código para validação do email
        /// </summary>
        /// <param name="toEmail">Email de destino</param>
        /// <param name="firstName">Primeiro nome do usuário</param>
        /// <returns></returns>
        public static async Task<string> GetRegisterConfirmationCodeAsync(
            string toEmail,
            string firstName)
        {
            Thread.Sleep(5000); // Aguardar 5 segundos para que o email seja entregue

            PopMailClient popMailClient = new PopMailClient(
                Pop3Configuration.Host,
                Pop3Configuration.Port,
                Pop3Configuration.User,
                Pop3Configuration.Password);

            MailMessage[] emails = await popMailClient.ReceiveAsync().ConfigureAwait(false);
            MailMessage mailMessage = emails.First(w => w.To == toEmail);
            // Template do email de confirmação:
            /*
                Olá Test, boas vindas! Seu código de confirmação é: 436A44F55FA4349801904263B430EB390B2CA6535BF70BE2A830FDDAEFA2D864B5A3027D17367DDC0AB09486DE941CE00E9034A32D1CFB24B15CDE357D5DAD67
             */

            return mailMessage.Body.Replace($"Olá {firstName}, boas vindas! Seu código de confirmação é: ", "");
        }

        /// <summary>
        /// Confirmar email de um usuário
        /// </summary>
        /// <param name="confirmRegisterRequest">Dados para validação do email</param>
        /// <returns></returns>
        public async Task<ConfirmRegisterResponse> ConfirmUserEmailAsync
            (ConfirmRegisterRequest confirmRegisterRequest)
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;

            HttpResponseMessage response = await _httpClient.PostAsync(
                Endpoints.Auth.ConfirmRegister_v1_0,
                confirmRegisterRequest.ToStringContent()).ConfigureAwait(false);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            string bodyResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            ConfirmRegisterResponse confirmRegisterResponse = JsonConvert.DeserializeObject<ConfirmRegisterResponse>(
                    bodyResponse);

            confirmRegisterResponse.Should().NotBeNull();
            confirmRegisterResponse.FirstName.Should().NotBeNull();
            confirmRegisterResponse.LastName.Should().NotBeNull();
            confirmRegisterResponse.Email.Should().Be(confirmRegisterRequest.Email);
            confirmRegisterResponse.Phone.Should().NotBeNull();
            confirmRegisterResponse.IsEmailValidated.Should().BeTrue();
            confirmRegisterResponse.RecoveryPasswordToken.Should().BeNullOrEmpty();

            return confirmRegisterResponse;
        }

        /// <summary>
        /// Autenticar um usuário válido
        /// </summary>
        /// <param name="signinRequest">Dados para autenticação</param>
        /// <returns></returns>
        public async Task<SigninResponse> AuthValidUserAsync(
            SigninRequest signinRequest)
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;

            HttpResponseMessage response = await _httpClient.PostAsync(
                Endpoints.Auth.Signin_v1_0,
                signinRequest.ToStringContent()).ConfigureAwait(false);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            string bodyResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            SigninResponse signinResponse = JsonConvert.DeserializeObject<SigninResponse>(
                    bodyResponse);

            signinResponse.Should().NotBeNull();
            signinResponse.Token.Should().NotBeNull();
            signinResponse.RefreshToken.Should().NotBeNull();

            return signinResponse;
        }

        /// <summary>
        /// Registra e valida o email de um novo usuário
        /// </summary>
        /// <param name="registerRequest"></param>
        /// <returns></returns>
        public async Task<ConfirmRegisterResponse> RegisterNewUserAndConfirmEmailAsync(
            RegisterRequest registerRequest)
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;

            RegisterResponse registerResponse = await RegisterNewUserAsync(registerRequest).ConfigureAwait(false);

            string confirmationCode = await GetRegisterConfirmationCodeAsync(registerResponse.Email, registerResponse.FirstName).ConfigureAwait(false);

            return await ConfirmUserEmailAsync(new ConfirmRegisterRequest()
            {
                ConfirmRegisterToken = confirmationCode,
                Email = registerResponse.Email
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// Executa a alteração de senha de um usuário
        /// </summary>
        /// <param name="changePasswordRequest">Nova senha</param>
        /// <param name="authToken">Token de autenticação</param>
        /// <returns>Sucesso</returns>
        public async Task<ChangePasswordResponse> ChangeUserPasswordAsync(
            ChangePasswordRequest changePasswordRequest,
            string authToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

            HttpResponseMessage response = await _httpClient.PostAsync(
                Endpoints.Auth.ChangePassword_v1_0,
                changePasswordRequest.ToStringContent()).ConfigureAwait(false);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            string bodyResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            ChangePasswordResponse changePasswordResponse = JsonConvert.DeserializeObject<ChangePasswordResponse>(
                    bodyResponse);

            changePasswordResponse.Should().NotBeNull();
            changePasswordResponse.Email.Should().Be(changePasswordRequest.Email);

            return changePasswordResponse;
        }

        /// <summary>
        /// Reenvia o código de confirmação de um usuário
        /// </summary>
        /// <param name="resendConfirmationCodeRequest">Dados para reenvio</param>
        /// <returns>Sucesso</returns>
        public async Task ResendConfirmationCodeAsync(
            ResendConfirmationCodeRequest resendConfirmationCodeRequest)
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;

            HttpResponseMessage response = await _httpClient.PostAsync(
                Endpoints.Auth.ResendConfirmationCode_v1_0,
                resendConfirmationCodeRequest.ToStringContent()).ConfigureAwait(false);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        /// <summary>
        /// Inicia a recuperação de senha para um usuário
        /// </summary>
        /// <param name="forgotPasswordRequest">Informações para recuperação de senha</param>
        /// <returns>Sucesso</returns>
        public async Task ForgotPasswordAsync(
            ForgotPasswordRequest forgotPasswordRequest)
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;

            HttpResponseMessage response = await _httpClient.PostAsync(
                Endpoints.Auth.ForgotPassword_v1_0,
                forgotPasswordRequest.ToStringContent()).ConfigureAwait(false);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        /// <summary>
        /// Recuperar o código para recuperação da senha
        /// </summary>
        /// <param name="toEmail">Email de destino</param>
        /// <param name="firstName">Primeiro nome do usuário</param>
        /// <returns></returns>
        public static async Task<string> GetForgotPasswordCodeAsync(
            string toEmail,
            string firstName)
        {
            Thread.Sleep(5000); // Aguardar 5 segundos para que o email seja entregue

            PopMailClient popMailClient = new PopMailClient(
                Pop3Configuration.Host,
                Pop3Configuration.Port,
                Pop3Configuration.User,
                Pop3Configuration.Password);

            MailMessage[] emails = await popMailClient.ReceiveAsync().ConfigureAwait(false);
            MailMessage mailMessage = emails.First(w => w.To == toEmail);
            // Template do email de confirmação:
            /*
                Olá {FirstName}, utilize o código {RecoveryPasswordToken} para recuperar sua senha.
             */

            return mailMessage.Body
                        .Replace($"Olá {firstName}, utilize o código ", "")
                        .Replace(" para recuperar sua senha.", "");
        }

        /// <summary>
        /// Inicia a recuperação de senha para um usuário
        /// </summary>
        /// <param name="forgotPasswordRequest">Informações para recuperação de senha</param>
        /// <returns>Sucesso</returns>
        public async Task ConfirmForgotPasswordAsync(
            ConfirmForgotPasswordRequest confirmForgotPasswordRequest)
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;

            HttpResponseMessage response = await _httpClient.PostAsync(
                Endpoints.Auth.ConfirmForgotPassword_v1_0,
                confirmForgotPasswordRequest.ToStringContent()).ConfigureAwait(false);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        /// <summary>
        /// Cria e valida um novo usuário, após, vincula uma roleType ao usuário recem criado
        /// </summary>
        /// <param name="registerRequest">Dados para criação do usuário</param>
        /// <param name="roleType">Role para ser adicionada</param>
        /// <returns>Dados de autenticação</returns>
        public async Task<SigninResponse> RegisterNewUserConfirmEmailAndAddRoleAsync(
            RegisterRequest registerRequest,
            int roleType)
        {
            ConfirmRegisterResponse confirmRegisterResponse = await RegisterNewUserAndConfirmEmailAsync(registerRequest).ConfigureAwait(false);

            string sql = $"insert into user_role (user_id, role_type, created_on, created_by) values ({confirmRegisterResponse.Id}, {roleType}, now(), {confirmRegisterResponse.Id});";

            using NpgsqlConnection npgsqlConnection = new NpgsqlConnection(Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING"));
            npgsqlConnection.Open();
            npgsqlConnection.State.Should().Be(System.Data.ConnectionState.Open);

            using NpgsqlCommand npgsqlCommand = new NpgsqlCommand(sql, npgsqlConnection);

            int lines = npgsqlCommand.ExecuteNonQuery();
            lines.Should().Be(1);

            return await AuthValidUserAsync(new SigninRequest()
            {
                Email = registerRequest.Email,
                Password = registerRequest.Password
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// Cria um novo usuário a partir da gestão de usuários
        /// </summary>
        /// <param name="managerUserCreateRequest">Dados para a criação de um novo usuário</param>
        /// <param name="authToken">Token de autenticação</param>
        /// <returns>Sucesso</returns>
        public async Task<ManagerUserResponse> ManagerUsersCreate(
            ManagerUserCreateRequest managerUserCreateRequest,
            string authToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

            HttpResponseMessage response = await _httpClient.PostAsync(
                Endpoints.Auth.ManageUsersPost_v1_0,
                managerUserCreateRequest.ToStringContent()).ConfigureAwait(false);
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            string bodyResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            ManagerUserResponse managerUserResponse = JsonConvert.DeserializeObject<ManagerUserResponse>(
                    bodyResponse);

            managerUserResponse.Should().NotBeNull();
            managerUserResponse.Email.Should().Be(managerUserCreateRequest.Email);

            return managerUserResponse;
        }
    }
}
