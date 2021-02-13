/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Api.ViewModel
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Tipo do erro
    /// </summary>
    public enum ErrorType
    {
        /// <summary>
        /// Objeto nulo
        /// </summary>
        [Display(
            ResourceType = typeof(Strings),
            Name = nameof(Strings.ErrorType_NullObject))]
        NullObject = 0,

        /// <summary>
        /// Objeto não encontrado
        /// </summary>
        [Display(
            ResourceType = typeof(Strings),
            Name = nameof(Strings.ErrorSource_NotFound))]
        NotFound = 1,

        /// <summary>
        /// Objeto já existe
        /// </summary>
        [Display(
            ResourceType = typeof(Strings),
            Name = nameof(Strings.ErrorSource_AlreadyExists))]
        AlreadyExists = 2,

        /// <summary>
        /// Dados inválidos na model
        /// </summary>
        [Display(
            ResourceType = typeof(Strings),
            Name = nameof(Strings.ErrorSource_ModelValidation))]
        ModelValidation = 3,

        /// <summary>
        /// Exception não tratada
        /// </summary>
        [Display(
            ResourceType = typeof(Strings),
            Name = nameof(Strings.ErrorSource_GenericException))]
        GenericException = 4,

        /// <summary>
        /// Falha na checagem da saúde da aplicação
        /// </summary>
        [Display(
            ResourceType = typeof(Strings),
            Name = nameof(Strings.ErrorSource_HealthCheckFail))]
        HealthCheckFail = 5
    }
}
