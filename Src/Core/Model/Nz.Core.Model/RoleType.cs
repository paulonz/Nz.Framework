/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Core.Model
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Regras de usuário
    /// </summary>
    public enum RoleType
    {
        /// <summary>
        /// Gestão de usuários
        /// </summary>
        [Display(
            ResourceType = typeof(Strings),
            Name = nameof(Strings.RoleType_ManageUsers))]
        ManageUsers = 0,

        /// <summary>
        /// Gestão de anúncios
        /// </summary>
        [Display(
            ResourceType = typeof(Strings),
            Name = nameof(Strings.RoleType_ManageAnnouncements))]
        ManageAnnouncements = 1
    }
}
