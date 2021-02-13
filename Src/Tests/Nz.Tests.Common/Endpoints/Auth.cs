/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Tests.Common.Endpoints
{
    /// <summary>
    /// Lista de endpoints usados pelos testes
    /// </summary>
    public static class Auth
    {
        /// <summary>
        /// Url base
        /// </summary>
        public static string Base => "https://localhost:5001";
        /// <summary>
        /// Url base
        /// </summary>
        public static string BaseNoPort => "https://localhost";
        /// <summary>
        /// Sigin
        /// </summary>
        public static string Signin_v1_0 => "/1.0/Auth/SignIn";
        /// <summary>
        /// Register
        /// </summary>
        public static string Register_v1_0 => "/1.0/Auth/Register";
        /// <summary>
        /// ConfirmRegister
        /// </summary>
        public static string ConfirmRegister_v1_0 => "/1.0/Auth/ConfirmRegister";
        /// <summary>
        /// ResendConfirmationCode
        /// </summary>
        public static string ResendConfirmationCode_v1_0 => "/1.0/Auth/ResendConfirmationCode";
        /// <summary>
        /// ForgotPassword
        /// </summary>
        public static string ForgotPassword_v1_0 => "/1.0/Auth/ForgotPassword";
        /// <summary>
        /// ConfirmForgotPassword
        /// </summary>
        public static string ConfirmForgotPassword_v1_0 => "/1.0/Auth/ConfirmForgotPassword";
        /// <summary>
        /// ChangePassword
        /// </summary>
        public static string ChangePassword_v1_0 => "/1.0/Auth/ChangePassword";
        /// <summary>
        /// RefreshToken
        /// </summary>
        public static string RefreshToken_v1_0 => "/1.0/Auth/RefreshToken";
        /// <summary>
        /// HealthCheck
        /// </summary>
        public static string HealthCheck_v1_0 => "/1.0/HealthCheck";
        /// <summary>
        /// ManageUsersPost
        /// </summary>
        public static string ManageUsersPost_v1_0 => "/1.0/ManageUsers";
        /// <summary>
        /// ManageUsersPut
        /// </summary>
        /// <param name="id">Identificador</param>
        public static string ManageUsersPut_v1_0(
            long id)
        {
            return $"/1.0/ManageUsers/{id}";
        }
        /// <summary>
        /// ManageUsersDelete
        /// </summary>
        /// <param name="id">Identificador</param>
        public static string ManageUsersDelete_v1_0(
            long id)
        {
            return $"/1.0/ManageUsers/{id}";
        }
        /// <summary>
        /// ManageUsersUnDelete
        /// </summary>
        /// <param name="id">Identificador</param>
        public static string ManageUsersUnDelete_v1_0(
            long id)
        {
            return $"/1.0/ManageUsers/{id}";
        }
        /// <summary>
        /// ManageUsersGetAll
        /// </summary>
        public static string ManageUsersGetAll_v1_0 => "/1.0/ManageUsers/";
        /// <summary>
        /// ManageUsersGetSingle
        /// </summary>
        /// <param name="id">Identificador</param>
        public static string ManageUsersGetSingle_v1_0(
            long id)
        {
            return $"/1.0/ManageUsers/{id}";
        }
    }
}
