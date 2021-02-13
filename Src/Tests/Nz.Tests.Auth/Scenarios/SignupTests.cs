/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Tests.Auth.Scenarios
{
    using System.Threading.Tasks;
    using FluentAssertions;
    using Nz.Tests.Common;
    using Nz.Tests.Common.Models.Auth;
    using Nz.Tests.Common.Scenarios;
    using Xunit;

    /// <summary>
    /// Testes Auth => SignUp
    /// </summary>
    public class SignupTests : AuthTestsBase
    {
        /// <summary>
        /// Ações comuns entre cenários
        /// </summary>
        private readonly AuthCommonActions _authCommonActions;

        /// <summary>
        /// Construtor padrão
        /// </summary>
        public SignupTests()
        {
            _authCommonActions = new AuthCommonActions(HttpClient);
        }

        /// <summary>
        /// Processo de registro de um novo usuário com sucesso:
        /// 1 - Registro da conta
        /// 2 - Recebimento de email com a confirmação do cadastro
        /// 3 - Validação do email
        /// 4 - Email de boas vindas
        /// 5 - Autenticação de usuário válido
        /// </summary>
        /// <returns>Sucesso</returns>
        [Fact]
        public async Task signup_user_process_happy_path()
        {
            string email = $"{Helpers.GenerateRandonIdentifier()}@test.com.br";
            string password = $"{Helpers.GenerateRandonIdentifier().Substring(0, 20)}";

            await _authCommonActions.RegisterNewUserAndConfirmEmailAsync(new RegisterRequest()
            {
                Email = email,
                FirstName = "Test",
                LastName = "User",
                Password = password,
                Phone = "+5599999999999"
            }).ConfigureAwait(false);

            await _authCommonActions.AuthValidUserAsync(new SigninRequest()
            {
                Email = email,
                Password = password
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// Usuário tenta se autenticar sem ter confirmado o email
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task auth_before_email_confirmation()
        {
            string email = $"{Helpers.GenerateRandonIdentifier()}@test.com.br";
            string password = $"{Helpers.GenerateRandonIdentifier().Substring(0, 20)}";

            await _authCommonActions.RegisterNewUserAsync(new RegisterRequest()
            {
                Email = email,
                FirstName = "Test",
                LastName = "User",
                Password = password,
                Phone = "+5599999999999"
            }).ConfigureAwait(false);

            await _authCommonActions.TryAuthUserWithoutEmailValidatedAsync(new SigninRequest()
            {
                Email = email,
                Password = password
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// Usuário altera a própria senha, a senha anterior não pode funcionar mais
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task user_change_password_happy_path()
        {
            string email = $"{Helpers.GenerateRandonIdentifier()}@test.com.br";
            string password = $"{Helpers.GenerateRandonIdentifier().Substring(0, 20)}";
            string newPassword = $"{Helpers.GenerateRandonIdentifier().Substring(0, 20)}";

            await _authCommonActions.RegisterNewUserAndConfirmEmailAsync(new RegisterRequest()
            {
                Email = email,
                FirstName = "Test",
                LastName = "User",
                Password = password,
                Phone = "+5599999999999"
            }).ConfigureAwait(false);

            SigninResponse signinResponse = await _authCommonActions.AuthValidUserAsync(new SigninRequest()
            {
                Email = email,
                Password = password
            }).ConfigureAwait(false);

            await _authCommonActions.ChangeUserPasswordAsync(new ChangePasswordRequest()
            {
                CurrentPassword = password,
                Email = email,
                NewPassword = newPassword
            }, signinResponse.Token).ConfigureAwait(false);

            await _authCommonActions.AuthValidUserAsync(new SigninRequest()
            {
                Email = email,
                Password = newPassword
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// Usuário solicita reenvio do código para confirmação do email
        /// </summary>
        /// <returns>Sucesso</returns>
        [Fact]
        public async Task user_ask_to_resend_confirmation_code_happy_path()
        {
            string email = $"{Helpers.GenerateRandonIdentifier()}@test.com.br";
            string password = $"{Helpers.GenerateRandonIdentifier().Substring(0, 20)}";
            string firstName = "Test";

            await _authCommonActions.RegisterNewUserAsync(new RegisterRequest()
            {
                Email = email,
                FirstName = firstName,
                LastName = "User",
                Password = password,
                Phone = "+5599999999999"
            }).ConfigureAwait(false);

            string firstConfirmationCode = await AuthCommonActions.GetRegisterConfirmationCodeAsync(email, firstName).ConfigureAwait(false);
            firstConfirmationCode.Should().NotBeNullOrEmpty();

            await _authCommonActions.ResendConfirmationCodeAsync(new ResendConfirmationCodeRequest()
            {
                Email = email
            }).ConfigureAwait(false);

            string secondConfirmationCode = await AuthCommonActions.GetRegisterConfirmationCodeAsync(email, firstName).ConfigureAwait(false);
            secondConfirmationCode.Should().NotBeNullOrEmpty();
            secondConfirmationCode.Should().Be(firstConfirmationCode);

            await _authCommonActions.ConfirmUserEmailAsync(new ConfirmRegisterRequest()
            {
                ConfirmRegisterToken = secondConfirmationCode,
                Email = email
            });
        }

        /// <summary>
        /// Usuário esqueceu a senha e irá criar outra
        /// </summary>
        /// <returns>Senha</returns>
        [Fact]
        public async Task user_forgot_password()
        {
            string email = $"{Helpers.GenerateRandonIdentifier()}@test.com.br";
            string password = $"{Helpers.GenerateRandonIdentifier().Substring(0, 20)}";
            string newPassword = $"{Helpers.GenerateRandonIdentifier().Substring(0, 20)}";
            string firstName = "Test";

            await _authCommonActions.RegisterNewUserAndConfirmEmailAsync(new RegisterRequest()
            {
                Email = email,
                FirstName = firstName,
                LastName = "User",
                Password = password,
                Phone = "+5599999999999"
            }).ConfigureAwait(false);

            await _authCommonActions.ForgotPasswordAsync(new ForgotPasswordRequest()
            {
                Email = email
            }).ConfigureAwait(false);

            string recoveryCode = await AuthCommonActions.GetForgotPasswordCodeAsync(email, firstName);
            recoveryCode.Should().NotBeNullOrEmpty();

            await _authCommonActions.ConfirmForgotPasswordAsync(new ConfirmForgotPasswordRequest()
            {
                Email = email,
                NewPassword = newPassword,
                RecoveryPasswordToken = recoveryCode
            });

            await _authCommonActions.AuthValidUserAsync(new SigninRequest()
            {
                Email = email,
                Password = newPassword
            });
        }
    }
}
