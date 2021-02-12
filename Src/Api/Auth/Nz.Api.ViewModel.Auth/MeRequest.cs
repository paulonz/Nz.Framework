/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

using System.ComponentModel.DataAnnotations;

namespace Nz.Api.ViewModel.Auth
{
    /// <summary>
    /// Dados do usuário autenticado
    /// </summary>
    public class MeRequest : IRequestViewModel
    {
        /// <summary>
        /// Primeiro nome
        /// </summary>
        [Display(
            ResourceType = typeof(Core.Model.Impl.Auth.Strings),
            Name = nameof(Core.Model.Impl.Auth.Strings.User_FirstName))]
        [StringLength(100,
            ErrorMessageResourceType = typeof(Core.Model.Strings),
            ErrorMessageResourceName = nameof(Core.Model.Validations.StringLengthAttribute_ValidationErrorIncludingMinimum))]
        public string FirstName { get; set; }

        /// <summary>
        /// Sobrenome
        /// </summary>
        [Display(
            ResourceType = typeof(Core.Model.Impl.Auth.Strings),
            Name = nameof(Core.Model.Impl.Auth.Strings.User_LastName))]
        [StringLength(100,
            ErrorMessageResourceType = typeof(Core.Model.Validations),
            ErrorMessageResourceName = nameof(Core.Model.Validations.StringLengthAttribute_ValidationErrorIncludingMinimum))]
        public string LastName { get; set; }

        /// <summary>
        /// Número de telefone
        /// </summary>
        [Display(
            ResourceType = typeof(Core.Model.Impl.Auth.Strings),
            Name = nameof(Core.Model.Impl.Auth.Strings.User_Phone))]
        [StringLength(20,
            ErrorMessageResourceType = typeof(Core.Model.Validations),
            ErrorMessageResourceName = nameof(Core.Model.Validations.StringLengthAttribute_ValidationErrorIncludingMinimum))]
        [Phone(
            ErrorMessageResourceType = typeof(Core.Model.Validations),
            ErrorMessageResourceName = nameof(Core.Model.Validations.PhoneAttribute_Invalid))]
        public string Phone { get; set; }
    }
}
