/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Api.ViewModel.Auth
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Model de autenticação de um usuário
    /// </summary>
    public class LoginRequest : IRequestViewModel
    {
        /// <summary>
        /// Login do usuário
        /// </summary>
        [Display(
            ResourceType = typeof(Core.Model.Impl.Auth.Strings),
            Name = nameof(Core.Model.Impl.Auth.Strings.LoginRequest_Email))]
        [Required(
            ErrorMessageResourceType = typeof(Core.Model.Validations),
            ErrorMessageResourceName = nameof(Core.Model.Validations.RequiredAttribute_ValidationError))]
        [StringLength(100,
            ErrorMessageResourceType = typeof(Core.Model.Validations),
            ErrorMessageResourceName = nameof(Core.Model.Validations.StringLengthAttribute_ValidationErrorIncludingMinimum))]
        public string Email { get; set; }

        /// <summary>
        /// Senha do usuário
        /// </summary>
        [Display(
            ResourceType = typeof(Core.Model.Impl.Auth.Strings),
            Name = nameof(Core.Model.Impl.Auth.Strings.LoginRequest_Password))]
        [Required(
            ErrorMessageResourceType = typeof(Core.Model.Validations),
            ErrorMessageResourceName = nameof(Core.Model.Validations.RequiredAttribute_ValidationError))]
        [StringLength(20,
            ErrorMessageResourceType = typeof(Core.Model.Validations),
            ErrorMessageResourceName = nameof(Core.Model.Validations.StringLengthAttribute_ValidationErrorIncludingMinimum))]
        public string Password { get; set; }
    }
}
