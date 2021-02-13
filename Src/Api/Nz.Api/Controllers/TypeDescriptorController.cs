/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Api.Controllers
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Reflection;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Nz.Common.Helpers;
    using Nz.Libs.RestPagination;

    /// <summary>
    /// Controller responsável por descrever tipos
    /// </summary>
    [AllowAnonymous]
    public class TypeDescriptorController : ApiControllerBase
    {
        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Helper para resources
        /// </summary>
        private readonly IResourceHelper _resourceHelper;

        /// <summary>
        /// Cache dos tipos já descritos
        /// </summary>
        private static ConcurrentDictionary<Type, IList<ViewModel.TypeDescriptionResponse>> CachedDescriptions { get; set; }

        /// <summary>
        /// Lista de tipos disponíveis
        /// </summary>
        private static IList<Type> _cachedEnumTypes;

        /// <summary>
        /// Lista de tipos disponíveis
        /// </summary>
        private IList<Type> EnumTypes
        {
            get
            {
                try
                {
                    if (_cachedEnumTypes == null || !_cachedEnumTypes.Any())
                    {
                        _cachedEnumTypes = AppDomain
                                        .CurrentDomain
                                        .GetAssemblies()
                                        .SelectMany(w => w.GetTypes())
                                        .Where(w => w.IsEnum && w.IsPublic && w.Namespace.StartsWith("Nz.", StringComparison.Ordinal))
                                        .ToList();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);

                    throw;
                }

                return _cachedEnumTypes;
            }
        }

        /// <summary>
        /// Construtor padrão
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="resourceHelper">Helper para resources</param>
        public TypeDescriptorController(
            ILogger<TypeDescriptorController> logger,
            IResourceHelper resourceHelper) : base(logger)
        {
            _logger = logger;
            _resourceHelper = resourceHelper;

            if (CachedDescriptions == null)
            {
                CachedDescriptions = new ConcurrentDictionary<Type, IList<ViewModel.TypeDescriptionResponse>>();
            }
        }

        /// <summary>
        /// Recupera a descrição de um tipo
        /// </summary>
        /// <param name="typeName">Tipo</param>
        /// <returns>Lista de descrição do tipo</returns>
        /// <response code="200">Retorna a lista de descrições do tipo</response>
        /// <response code="204">Quando a consulta não retornar resultados</response>
        /// <response code="400">Caso ocorra algum erro de validação</response>
        /// <response code="500">Se ocorrer um erro interno ou de regra de negócio</response>
        [HttpGet("{typeName}")]
        [ServiceFilter(typeof(EnablePagingAttribute))]
        [ProducesResponseType(
            type: typeof(ViewModel.TypeDescriptionResponse[]),
            statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
        [ProducesResponseType(
            type: typeof(ViewModel.ApiErrorResponse),
            statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(
            type: typeof(ViewModel.ApiErrorResponse),
            statusCode: StatusCodes.Status500InternalServerError)]
        public IActionResult GetTypeDescription(
            [FromRoute] string typeName)
        {
            using (_logger.BeginScope($"{GetType().FullName}.{nameof(GetTypeDescription)} - #{typeName}"))
            {
                try
                {
                    if (EnumTypes != null && EnumTypes.Any(w => w.Name.Equals(typeName, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        Type type = EnumTypes.First(w => w.Name.Equals(typeName, StringComparison.InvariantCultureIgnoreCase));

                        return BaseAction(type);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);

                    return BadRequest(new ViewModel.ApiErrorResponse(new ViewModel.ApiErrorDetailResponse()
                    {
                        ErrorType = ViewModel.ErrorType.GenericException,
                        Message = ex.Message
                    }));
                }

                return NoContent();
            }
        }

        /// <summary>
        /// Action base para descrever um tipo
        /// </summary>
        /// <param name="enumType">Tipo que será descrito</param>
        /// <returns>Descrição do tipo</returns>
        private IActionResult BaseAction(
            Type enumType)
        {
            try
            {
                IQueryable<ViewModel.TypeDescriptionResponse> typeDescriptions = GetDescription(enumType);

                if (typeDescriptions != null)
                {
                    return Ok(typeDescriptions);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return BadRequest(new ViewModel.ApiErrorResponse(new ViewModel.ApiErrorDetailResponse()
                {
                    ErrorType = ViewModel.ErrorType.GenericException,
                    Message = ex.Message
                }));
            }

            return NoContent();
        }

        /// <summary>
        /// Recupera a lista de valores dos atributos Description de um enum
        /// </summary>
        /// <param name="enumType">Tipo do enum</param>
        /// <returns>Lista de valores dos atributos Description</returns>
        private IQueryable<ViewModel.TypeDescriptionResponse> GetDescription(
            Type enumType)
        {
            try
            {
                if (CachedDescriptions == null || !CachedDescriptions.Any(w => w.Key == enumType))
                {
                    Array enumValues = enumType.GetEnumValues();

                    if (enumValues != null && enumValues.Length > 0)
                    {
                        IList<ViewModel.TypeDescriptionResponse> result = new List<ViewModel.TypeDescriptionResponse>();

                        foreach (object item in enumValues)
                        {
                            MemberInfo memberInfo = enumType.GetMember(item.ToString()).FirstOrDefault();

                            if (memberInfo != null)
                            {
                                ViewModel.TypeDescriptionResponse typeDescription = new ViewModel.TypeDescriptionResponse()
                                {
                                    Id = (int)item
                                };

                                DisplayAttribute displayAttribute = memberInfo.GetCustomAttribute<DisplayAttribute>(false);

                                if (displayAttribute != null && displayAttribute.ResourceType != null)
                                {
                                    typeDescription.Description = _resourceHelper.LookupResource(displayAttribute.ResourceType, displayAttribute.Name);
                                }
                                else
                                {
                                    typeDescription.Description = item.ToString();
                                }

                                result.Add(typeDescription);
                            }
                        }

                        CachedDescriptions.TryAdd(enumType, result);

                        return result.AsQueryable();
                    }
                }
                else
                {
                    return CachedDescriptions.First(w => w.Key == enumType).Value.AsQueryable();
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
