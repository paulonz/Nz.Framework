/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Tests.Components.Backend.Common
{
    using System;
    using FluentAssertions;
    using FluentAssertions.Extensions;
    using Microsoft.Extensions.DependencyInjection;
    using Nz.Common.Helpers;
    using Xunit;

    /// <summary>
    /// Testes para ResourceHelper
    /// </summary>
    public class ResourceHelperTests
    {
        /// <summary>
        /// Resource Helper
        /// </summary>
        private readonly IResourceHelper _resourceHelper;

        /// <summary>
        /// Tempo máximo para execução do método
        /// </summary>
        private readonly TimeSpan MaxExecutionTime = 10.Milliseconds();

        /// <summary>
        /// Construtor padrão
        /// </summary>
        public ResourceHelperTests()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddSingleton<IResourceHelper, ResourceHelper>(impl =>
            {
                return new ResourceHelper(new Mocks.MockLogger<ResourceHelper>());
            });

            _resourceHelper = services.BuildServiceProvider().GetService<IResourceHelper>();
        }

        /// <summary>
        /// Tempo de execução do método deve ser menor que 10 milisegundos
        /// </summary>
        /// <param name="type">Resource</param>
        /// <param name="key">Chave</param>
        [Theory]
        [InlineData(typeof(Strings), "KeyName")]
        [InlineData(typeof(Strings), "MockEnum_Val01")]
        [InlineData(typeof(Strings), "InvalidKeyName")]
        [InlineData(typeof(ResourceHelperTests), "SomeKey")]
        [InlineData(typeof(Empty), "SomeKey")]
        public void execution_must_be_fast(
            Type type,
            string key)
        {
            //warm up
            _resourceHelper
                .LookupResource(type, key);

            _resourceHelper
                .ExecutionTimeOf(s => s.LookupResource(type, key))
                .Should()
                .BeLessOrEqualTo(MaxExecutionTime);
        }

        /// <summary>
        /// Localizar string em um resource
        /// </summary>
        /// <param name="type">Resource</param>
        /// <param name="key">Chave</param>
        /// <param name="expectedValue">String esperada</param>
        [Theory]
        [InlineData(typeof(Strings), "KeyName", "KeyValue")]
        [InlineData(typeof(Strings), "MockEnum_Val01", "Value 01")]
        public void get_string_by_key_from_valid_resource_type(
            Type type,
            string key,
            string expectedValue)
        {
            _resourceHelper
                .LookupResource(type, key)
                .Should()
                .Be(expectedValue);
        }

        /// <summary>
        /// Localizar string inválida em um resource (válido e inválido)
        /// </summary>
        /// <param name="type">Resource</param>
        /// <param name="key">Chave</param>
        [Theory]
        [InlineData(typeof(Strings), "InvalidKeyName")]
        [InlineData(typeof(ResourceHelperTests), "SomeKey")]
        [InlineData(typeof(Empty), "SomeKey")]
        public void get_empty_string_from_invalid_parameters(
            Type type,
            string key)
        {
            _resourceHelper
                .LookupResource(type, key)
                .Should()
                .BeNullOrEmpty();
        }
    }
}
