/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Common.Helpers.Impl.Default
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Classe auxiliar
    /// </summary>
    public class EnumHelpers : IEnumHelpers
    {
        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Construtor padrão
        /// </summary>
        /// <param name="logger">Logger</param>
        public EnumHelpers(
            ILogger<EnumHelpers> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Recupera o valor do atributo Display de um enum value
        /// </summary>
        /// <typeparam name="T">Enum</typeparam>
        /// <param name="source">Valor</param>
        /// <param name="resourceManager">Resource</param>
        /// <returns>Descrição</returns>
        public string GetDisplay<T>(
            T source,
            System.Resources.ResourceManager resourceManager) where T : Enum
        {
            try
            {
                if (GetAttribute<T, DisplayAttribute>(source) is DisplayAttribute attribute)
                {
                    string displayValue = !string.IsNullOrEmpty(attribute.Description) ?
                                            attribute.Description :
                                            resourceManager.GetString(attribute.Name, System.Globalization.CultureInfo.CurrentCulture);

                    if (!string.IsNullOrEmpty(displayValue))
                    {
                        return displayValue;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return source.ToString();
        }

        /// <summary>
        /// Recupera um atributo específico de um enum
        /// </summary>
        /// <typeparam name="T">Enum</typeparam>
        /// <typeparam name="Y">Atributo</typeparam>
        /// <param name="source">Valor</param>
        /// <returns>Atributo</returns>
        private Attribute GetAttribute<T, Y>(T source)
            where T : Enum
            where Y : Attribute
        {
            try
            {
                System.Reflection.FieldInfo fieldInfo = source
                    .GetType()
                    .GetField(source.ToString());

                if (fieldInfo != null)
                {
                    object[] attributes = fieldInfo.GetCustomAttributes(typeof(Y), false);

                    if (attributes != null && attributes.Length > 0)
                    {
                        return attributes.First() is not Y attribute ? null : (Attribute)attribute;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return null;
        }
    }
}
