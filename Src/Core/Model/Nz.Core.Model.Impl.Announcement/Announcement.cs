/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Core.Model.Impl.Announcement
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Anúncio classificado
    /// </summary>
    [Display(
        ResourceType = typeof(Strings),
        Name = nameof(Strings.Announcement))]
    public class Announcement : ModelBase
    {
        /// <summary>
        /// Título do anúncio
        /// </summary>
        [Display(
            ResourceType = typeof(Strings),
            Name = nameof(Strings.Announcement_Title))]
        [Required(
            ErrorMessageResourceType = typeof(Validations),
            ErrorMessageResourceName = nameof(Validations.RequiredAttribute_ValidationError))]
        [StringLength(100,
            ErrorMessageResourceType = typeof(Strings),
            ErrorMessageResourceName = nameof(Validations.StringLengthAttribute_ValidationErrorIncludingMinimum))]
        public string Title { get; set; }

        /// <summary>
        /// Descrição completa do anúncio
        /// </summary>
        [Display(
            ResourceType = typeof(Strings),
            Name = nameof(Strings.Announcement_Description))]
        [Required(
            ErrorMessageResourceType = typeof(Validations),
            ErrorMessageResourceName = nameof(Validations.RequiredAttribute_ValidationError))]
        [StringLength(8000,
            ErrorMessageResourceType = typeof(Strings),
            ErrorMessageResourceName = nameof(Validations.StringLengthAttribute_ValidationErrorIncludingMinimum))]
        public string Description { get; set; }

        /// <summary>
        /// Valor do item anúnciado
        /// </summary>
        [Display(
            ResourceType = typeof(Strings),
            Name = nameof(Strings.Announcement_Price))]
        [Required(
            ErrorMessageResourceType = typeof(Validations),
            ErrorMessageResourceName = nameof(Validations.RequiredAttribute_ValidationError))]
        [DataType(
            DataType.Currency,
            ErrorMessageResourceType = typeof(Validations),
            ErrorMessageResourceName = nameof(Validations.DataTypeAttribute_InvalidCurrentyValue))]
        public decimal Price { get; set; }
    }
}
