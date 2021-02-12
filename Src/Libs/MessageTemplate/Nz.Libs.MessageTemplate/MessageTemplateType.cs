/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Libs.MessageTemplate
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Templates de email
    /// </summary>
    public enum MessageTemplateType
    {
        /// <summary>
        /// Template para Email de registro de um novo usuário
        /// </summary>
        [Display(
            ResourceType = typeof(Strings),
            Name = nameof(Strings.MessageTemplateType_EmailNewUserRegister))]
        EmailNewUserRegister = 0,

        /// <summary>
        /// Template para Email de recuperação de senha
        /// </summary>
        [Display(
            ResourceType = typeof(Strings),
            Name = nameof(Strings.MessageTemplateType_EmailForgotPassword))]
        EmailForgotPassword = 1,

        /// <summary>
        /// Template para email de alteração de senha
        /// </summary>
        [Display(
            ResourceType = typeof(Strings),
            Name = nameof(Strings.MessageTemplateType_EmailChangePassword))]
        EmailChangePassword = 2,

        /// <summary>
        /// Template para email de confirmação de novo usuário
        /// </summary>
        [Display(
            ResourceType = typeof(Strings),
            Name = nameof(Strings.MessageTemplateType_EmailConfirmNewUserRegister))]
        EmailConfirmNewUserRegister = 3,

        /// <summary>
        /// Template para email de confirmação da recuperação de senha
        /// </summary>
        [Display(
            ResourceType = typeof(Strings),
            Name = nameof(Strings.MessageTemplateType_EmailConfirmForgotPassword))]
        EmailConfirmForgotPassword = 4
    }
}
