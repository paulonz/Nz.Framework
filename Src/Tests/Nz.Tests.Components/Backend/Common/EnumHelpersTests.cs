/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Tests.Components.Backend.Common
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using FluentAssertions;
    using FluentAssertions.Extensions;
    using Microsoft.Extensions.DependencyInjection;
    using Nz.Common.Helpers;
    using Nz.Common.Helpers.Impl.Default;
    using Xunit;

    /// <summary>
    /// Testes para EnumHelpers
    /// </summary>
    public class EnumHelpersTests
    {
        /// <summary>
        /// Enum Helpers
        /// </summary>
        private readonly IEnumHelpers _enumHelpers;

        /// <summary>
        /// Tempo máximo para execução do método
        /// </summary>
        private readonly TimeSpan MaxExecutionTime = 2.Milliseconds();

        /// <summary>
        /// Construtor padrão
        /// </summary>
        public EnumHelpersTests()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddSingleton<IEnumHelpers, EnumHelpers>(impl =>
            {
                return new EnumHelpers(new Mocks.MockLogger<EnumHelpers>());
            });

            _enumHelpers = services.BuildServiceProvider().GetService<IEnumHelpers>();
        }

        /// <summary>
        /// Mock de um enum
        /// </summary>
        public enum MockEnum
        {
            [Display(
                ResourceType = typeof(Strings),
                Name = nameof(Strings.MockEnum_Val01))]
            Val01,
            [Display(
                Description = "Literal value")]
            Val02,
            Val03
        }

        /// <summary>
        /// Tempo de execução do método deve ser menor que 10 milisegundos
        /// </summary>
        /// <param name="value">Valor testado</param>
        [Theory]
        [InlineData(MockEnum.Val01)]
        [InlineData(MockEnum.Val02)]
        [InlineData(MockEnum.Val03)]
        public void execution_must_be_fast(
            MockEnum value)
        {
            //warm up
            _enumHelpers
                .GetDisplay(value, Strings.ResourceManager);

            _enumHelpers
                .ExecutionTimeOf(s => s.GetDisplay(value, Strings.ResourceManager))
                .Should()
                .BeLessOrEqualTo(MaxExecutionTime);

            _enumHelpers
                .ExecutionTimeOf(s => s.GetDisplay(value, null))
                .Should()
                .BeLessOrEqualTo(MaxExecutionTime);

            _enumHelpers
                .ExecutionTimeOf(s => s.GetDisplay(value, Empty.ResourceManager))
                .Should()
                .BeLessOrEqualTo(MaxExecutionTime);
        }

        /// <summary>
        /// Recupera a string do atributo display a partir de um enum
        /// </summary>
        /// <param name="value">Valor enum para recuperar o valor do atributo Display</param>
        /// <param name="expected">Valor do atributo Display esperado</param>
        [Theory]
        [InlineData(MockEnum.Val01, "Value 01")]
        [InlineData(MockEnum.Val02, "Literal value")]
        [InlineData(MockEnum.Val03, nameof(MockEnum.Val03))]
        public void get_display_string_from_valid_resource_manager(
            MockEnum value,
            string expected)
        {
            _enumHelpers
                .GetDisplay(value, Strings.ResourceManager)
                .Should()
                .Be(expected);
        }

        /// <summary>
        /// Recupera a string do atributo display a partir de um enum
        /// </summary>
        /// <param name="value">Valor enum para recuperar o valor do atributo Display</param>
        /// <param name="expected">Valor do atributo Display esperado</param>
        [Theory]
        [InlineData(MockEnum.Val01, nameof(MockEnum.Val01))]
        [InlineData(MockEnum.Val02, "Literal value")]
        [InlineData(MockEnum.Val03, nameof(MockEnum.Val03))]
        public void get_display_string_from_null_resource_manager(
            MockEnum value,
            string expected)
        {
            _enumHelpers
                .GetDisplay(value, null)
                .Should()
                .Be(expected);
        }

        /// <summary>
        /// Recupera a string do atributo display a partir de um enum
        /// </summary>
        /// <param name="value">Valor enum para recuperar o valor do atributo Display</param>
        /// <param name="expected">Valor do atributo Display esperado</param>
        [Theory]
        [InlineData(MockEnum.Val01, nameof(MockEnum.Val01))]
        [InlineData(MockEnum.Val02, "Literal value")]
        [InlineData(MockEnum.Val03, nameof(MockEnum.Val03))]
        public void get_display_string_from_empty_resource_manager(
            MockEnum value,
            string expected)
        {
            _enumHelpers
                .GetDisplay(value, Empty.ResourceManager)
                .Should()
                .Be(expected);
        }
    }
}
