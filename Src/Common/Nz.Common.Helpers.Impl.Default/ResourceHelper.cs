/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Common.Helpers
{
    using System;
    using System.Reflection;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Helper para tratamento de resources
    /// </summary>
    public class ResourceHelper : IResourceHelper
    {
        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Construtor padrão
        /// </summary>
        /// <param name="logger">Logger</param>
        public ResourceHelper(
            ILogger<ResourceHelper> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Recupera o valor em um resource
        /// </summary>
        /// <param name="resourceType">Type do resource</param>
        /// <param name="resourceKey">Chave</param>
        /// <returns>Valor</returns>
        public string LookupResource(
            Type resourceType,
            string resourceKey)
        {
            try
            {
                if (resourceType != null)
                {
                    foreach (PropertyInfo staticProperty in resourceType.GetProperties(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public))
                    {
                        if (staticProperty.PropertyType == typeof(System.Resources.ResourceManager))
                        {
                            System.Resources.ResourceManager resourceManager = (System.Resources.ResourceManager)staticProperty.GetValue(null, null);
                            string result = resourceManager.GetString(resourceKey, System.Globalization.CultureInfo.CurrentUICulture);

                            if (!string.IsNullOrEmpty(result))
                            {
                                return result;
                            }
                        }
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
