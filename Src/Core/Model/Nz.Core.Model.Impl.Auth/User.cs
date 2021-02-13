/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Core.Model.Impl.Auth
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Usuário do sistema
    /// </summary>
    [Display(
        ResourceType = typeof(Strings),
        Name = nameof(Strings.User))]
    public class User : ModelBase
    {
        /// <summary>
        /// Primeiro nome
        /// </summary>
        [Display(
            ResourceType = typeof(Strings),
            Name = nameof(Strings.User_FirstName))]
        [Required(
            ErrorMessageResourceType = typeof(Validations),
            ErrorMessageResourceName = nameof(Validations.RequiredAttribute_ValidationError))]
        [StringLength(100,
            ErrorMessageResourceType = typeof(Strings),
            ErrorMessageResourceName = nameof(Validations.StringLengthAttribute_ValidationErrorIncludingMinimum))]
        public string FirstName { get; set; }

        /// <summary>
        /// Sobrenome
        /// </summary>
        [Display(
            ResourceType = typeof(Strings),
            Name = nameof(Strings.User_LastName))]
        [Required(
            ErrorMessageResourceType = typeof(Validations),
            ErrorMessageResourceName = nameof(Validations.RequiredAttribute_ValidationError))]
        [StringLength(100,
            ErrorMessageResourceType = typeof(Validations),
            ErrorMessageResourceName = nameof(Validations.StringLengthAttribute_ValidationErrorIncludingMinimum))]
        public string LastName { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [Display(
            ResourceType = typeof(Strings),
            Name = nameof(Strings.User_Email))]
        [Required(
            ErrorMessageResourceType = typeof(Validations),
            ErrorMessageResourceName = nameof(Validations.RequiredAttribute_ValidationError))]
        [StringLength(100,
            ErrorMessageResourceType = typeof(Validations),
            ErrorMessageResourceName = nameof(Validations.StringLengthAttribute_ValidationErrorIncludingMinimum))]
        [EmailAddress(
            ErrorMessageResourceType = typeof(Validations),
            ErrorMessageResourceName = nameof(Validations.EmailAddressAttribute_Invalid))]
        public string Email { get; set; }

        /// <summary>
        /// Número de telefone
        /// </summary>
        [Display(
            ResourceType = typeof(Strings),
            Name = nameof(Strings.User_Phone))]
        [Required(
            ErrorMessageResourceType = typeof(Validations),
            ErrorMessageResourceName = nameof(Validations.RequiredAttribute_ValidationError))]
        [StringLength(20,
            ErrorMessageResourceType = typeof(Validations),
            ErrorMessageResourceName = nameof(Validations.StringLengthAttribute_ValidationErrorIncludingMinimum))]
        [Phone(
            ErrorMessageResourceType = typeof(Validations),
            ErrorMessageResourceName = nameof(Validations.PhoneAttribute_Invalid))]
        public string Phone { get; set; }

        /// <summary>
        /// Senha criptografada do usuário
        /// </summary>
        [Display(
            ResourceType = typeof(Strings),
            Name = nameof(Strings.User_Password))]
        [Required(
            ErrorMessageResourceType = typeof(Validations),
            ErrorMessageResourceName = nameof(Validations.RequiredAttribute_ValidationError))]
        [StringLength(200,
            ErrorMessageResourceType = typeof(Validations),
            ErrorMessageResourceName = nameof(Validations.StringLengthAttribute_ValidationErrorIncludingMinimum))]
        public string Password { get; set; }

        /// <summary>
        /// Usuário validou o email?
        /// </summary>
        [Display(
            ResourceType = typeof(Strings),
            Name = nameof(Strings.User_IsEmailValidated))]
        public bool IsEmailValidated { get; set; }

        /// <summary>
        /// Token para recuperação de senha
        /// </summary>
        [Display(
            ResourceType = typeof(Strings),
            Name = nameof(Strings.User_RecoveryPasswordToken))]
        [StringLength(200,
            ErrorMessageResourceType = typeof(Validations),
            ErrorMessageResourceName = nameof(Validations.StringLengthAttribute_ValidationErrorIncludingMinimum))]
        public string RecoveryPasswordToken { get; set; }

        /// <summary>
        /// Lista de regras que o usuário possui
        /// </summary>
        [Display(
            ResourceType = typeof(Strings),
            Name = nameof(Strings.UserRole))]
        public virtual IEnumerable<UserRole> UserRoles { get; private set; }
    }
}
