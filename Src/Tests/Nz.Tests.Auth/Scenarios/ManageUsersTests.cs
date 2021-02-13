/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Tests.Auth.Scenarios
{
    using System.Threading.Tasks;
    using Common.Models.Auth;
    using Nz.Tests.Common;
    using Nz.Tests.Common.Scenarios;
    using Xunit;

    /// <summary>
    /// Testes ManageUsers
    /// </summary>
    public class ManageUsersTests : AuthTestsBase
    {
        /// <summary>
        /// Ações comuns entre cenários
        /// </summary>
        private readonly AuthCommonActions _authCommonActions;

        /// <summary>
        /// Construtor padrão
        /// </summary>
        public ManageUsersTests()
        {
            _authCommonActions = new AuthCommonActions(HttpClient);
        }

        /// <summary>
        /// Novo usuário sendo criado por outro usuário
        /// </summary>
        /// <returns>Sucesso</returns>
        [Fact]
        public async Task create_new_user_happy_path()
        {
            string email = $"{Helpers.GenerateRandonIdentifier()}@test.com.br";
            string password = $"{Helpers.GenerateRandonIdentifier().Substring(0, 20)}";

            SigninResponse signinResponse = await _authCommonActions.RegisterNewUserConfirmEmailAndAddRoleAsync(new RegisterRequest()
            {
                Email = email,
                FirstName = "Test",
                LastName = "User",
                Password = password,
                Phone = "+5599999999999"
            }, 0).ConfigureAwait(false);

            email = $"{Helpers.GenerateRandonIdentifier()}@test.com.br";
            password = $"{Helpers.GenerateRandonIdentifier().Substring(0, 20)}";

            ManagerUserResponse managerUserResponse = await _authCommonActions.ManagerUsersCreate(new ManagerUserCreateRequest()
            {
                Email = email,
                FirstName = "Test",
                LastName = "User",
                Password = password,
                Phone = "+5599999999999"
            }, signinResponse.Token).ConfigureAwait(false);

            string confirmationCode = await AuthCommonActions.GetRegisterConfirmationCodeAsync(email, managerUserResponse.FirstName).ConfigureAwait(false);

            await _authCommonActions.ConfirmUserEmailAsync(new ConfirmRegisterRequest()
            {
                ConfirmRegisterToken = confirmationCode,
                Email = email
            }).ConfigureAwait(false);

            await _authCommonActions.AuthValidUserAsync(new SigninRequest()
            {
                Email = email,
                Password = password
            });
        }
    }
}
