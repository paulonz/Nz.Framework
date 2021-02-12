/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Core.Model.Impl.Auth
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Regra de um usuário
    /// </summary>
    [Display(
        ResourceType = typeof(Strings),
        Name = nameof(Strings.UserRole))]
    public class UserRole : ModelBase
    {
        /// <summary>
        /// Identificador do usuário
        /// </summary>
        [Display(
            ResourceType = typeof(Strings),
            Name = nameof(Strings.User))]
        public long UserId { get; set; }

        /// <summary>
        /// Usuário
        /// </summary>
        [Display(
            ResourceType = typeof(Strings),
            Name = nameof(Strings.User))]
        public virtual User User { get; private set; }

        /// <summary>
        /// Identificador da regra
        /// </summary>
        [Display(
            ResourceType = typeof(Model.Strings),
            Name = nameof(Model.Strings.RoleType))]
        public RoleType RoleType { get; set; }
    }
}
