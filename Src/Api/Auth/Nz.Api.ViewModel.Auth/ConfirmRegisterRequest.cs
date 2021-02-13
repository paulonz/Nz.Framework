/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Api.ViewModel.Auth
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Confirmação de registro de um usuário
    /// </summary>
    public class ConfirmRegisterRequest : IRequestViewModel
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
        /// Token de confirmação
        /// </summary>
        [Display(
            ResourceType = typeof(Core.Model.Impl.Auth.Strings),
            Name = nameof(Core.Model.Impl.Auth.Strings.User_ConfirmRegisterToken))]
        [Required(
            ErrorMessageResourceType = typeof(Core.Model.Validations),
            ErrorMessageResourceName = nameof(Core.Model.Validations.RequiredAttribute_ValidationError))]
        [StringLength(200,
            ErrorMessageResourceType = typeof(Core.Model.Validations),
            ErrorMessageResourceName = nameof(Core.Model.Validations.StringLengthAttribute_ValidationErrorIncludingMinimum))]
        public string ConfirmRegisterToken { get; set; }
    }
}
