/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Api.ViewModel.Auth
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Model para solicitar reenvio do código de confirmação
    /// </summary>
    public class ResendConfirmationCodeRequest : IRequestViewModel
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
    }
}
