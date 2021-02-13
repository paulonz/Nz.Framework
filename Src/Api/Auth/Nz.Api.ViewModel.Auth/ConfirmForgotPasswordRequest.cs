/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Api.ViewModel.Auth
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Confirmação para recuperação de senha de um usuário
    /// </summary>
    public class ConfirmForgotPasswordRequest : IRequestViewModel
    {
        /// <summary>
        /// Email
        /// </summary>
        [Display(
            ResourceType = typeof(Core.Model.Impl.Auth.Strings),
            Name = nameof(Core.Model.Impl.Auth.Strings.User_Email))]
        [Required(
            ErrorMessageResourceType = typeof(Core.Model.Validations),
            ErrorMessageResourceName = nameof(Core.Model.Validations.RequiredAttribute_ValidationError))]
        [StringLength(100,
            ErrorMessageResourceType = typeof(Core.Model.Validations),
            ErrorMessageResourceName = nameof(Core.Model.Validations.StringLengthAttribute_ValidationErrorIncludingMinimum))]
        [EmailAddress(
            ErrorMessageResourceType = typeof(Core.Model.Validations),
            ErrorMessageResourceName = nameof(Core.Model.Validations.EmailAddressAttribute_Invalid))]
        public string Email { get; set; }

        /// <summary>
        /// Token para recuperação de senha
        /// </summary>
        [Display(
            ResourceType = typeof(Core.Model.Impl.Auth.Strings),
            Name = nameof(Core.Model.Impl.Auth.Strings.User_RecoveryPasswordToken))]
        [Required(
            ErrorMessageResourceType = typeof(Core.Model.Validations),
            ErrorMessageResourceName = nameof(Core.Model.Validations.RequiredAttribute_ValidationError))]
        [StringLength(200,
            ErrorMessageResourceType = typeof(Core.Model.Validations),
            ErrorMessageResourceName = nameof(Core.Model.Validations.StringLengthAttribute_ValidationErrorIncludingMinimum))]
        public string RecoveryPasswordToken { get; set; }

        /// <summary>
        /// Nova senha
        /// </summary>
        [Display(
            ResourceType = typeof(Core.Model.Impl.Auth.Strings),
            Name = nameof(Core.Model.Impl.Auth.Strings.User_NewPassword))]
        [Required(
            ErrorMessageResourceType = typeof(Core.Model.Validations),
            ErrorMessageResourceName = nameof(Core.Model.Validations.RequiredAttribute_ValidationError))]
        [StringLength(20,
            ErrorMessageResourceType = typeof(Core.Model.Validations),
            ErrorMessageResourceName = nameof(Core.Model.Validations.StringLengthAttribute_ValidationErrorIncludingMinimum))]
        public string NewPassword { get; set; }
    }
}
