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
    using Nz.Common.Helpers.Impl.Default;
    using Xunit;

    /// <summary>
    /// Testes para ParserHelper
    /// </summary>
    public class ParserHelperTests
    {
        /// <summary>
        /// Parser Helper
        /// </summary>
        private readonly IParserHelper _parserHelper;

        /// <summary>
        /// Tempo máximo para execução do método
        /// </summary>
        private readonly TimeSpan MaxExecutionTime = 2.Milliseconds();

        /// <summary>
        /// Construtor padrão
        /// </summary>
        public ParserHelperTests()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddSingleton<IParserHelper, ParserHelper>(impl =>
            {
                return new ParserHelper(new Mocks.MockLogger<ParserHelper>());
            });

            _parserHelper = services.BuildServiceProvider().GetService<IParserHelper>();
        }

        /// <summary>
        /// Tempo de execução do método deve ser menor que 10 milisegundos
        /// </summary>
        [Fact]
        public void execution_must_be_fast()
        {
            //warm up
            _parserHelper
                .ToJson(new { Id = 1, Name = "Abc" });

            _parserHelper
                .ExecutionTimeOf(s => s.ToJson(new { Id = 1, Name = "Abc" }))
                .Should()
                .BeLessOrEqualTo(MaxExecutionTime);

            _parserHelper
                .ExecutionTimeOf(s => s.ToJson(new
                {
                    Id = 1,
                    Name = "Abc",
                    List = new[]
                    {
                        new { Id = 2, Name = "" },
                        new { Id = 3, Name = "XYZ" }
                    },
                    Other = new
                    {
                        OtherId = 4333,
                        OtherName = "Test"
                    }
                }))
                .Should()
                .BeLessOrEqualTo(MaxExecutionTime);

            _parserHelper
                .ExecutionTimeOf(s => s.ToJson(new Mocks.SimpleModel()
                {
                    Code = 123456789012345678,
                    Id = 123,
                    Name = nameof(Mocks.SimpleModel)
                }))
                .Should()
                .BeLessOrEqualTo(MaxExecutionTime);

            _parserHelper
                .ExecutionTimeOf(s => s.ToJson(new Mocks.ComplexModel()
                {
                    Code = 987642341212114498,
                    Id = 67890,
                    Name = nameof(Mocks.ComplexModel),
                    SimpleModels = new Mocks.SimpleModel[]
                    {
                        new Mocks.SimpleModel() { Id = 1, Name = nameof(Mocks.SimpleModel), Code = 987654347865 },
                        new Mocks.SimpleModel() { Id = 2, Name = nameof(Mocks.SimpleModel), Code = 0475039592398 }
                    }
                }))
                .Should()
                .BeLessOrEqualTo(MaxExecutionTime);

            _parserHelper
                .ExecutionTimeOf(s => s.FromJson<dynamic>("{\"id\":123,\"name\":\"SimpleModel\",\"code\":123456789012345678}"))
                .Should()
                .BeLessOrEqualTo(MaxExecutionTime);

            _parserHelper
                .ExecutionTimeOf(s => s.FromJson<dynamic>("{\"id\":67890,\"name\":\"ComplexModel\",\"code\":987642341212114498,\"simpleModels\":[{\"id\":1,\"name\":\"SimpleModel\",\"code\":987654347865},{\"id\":2,\"name\":\"SimpleModel\",\"code\":475039592398}]}"))
                .Should()
                .BeLessOrEqualTo(MaxExecutionTime);

            _parserHelper
                .ExecutionTimeOf(s => s.FromJson<Mocks.SimpleModel>("{\"id\":123,\"name\":\"SimpleModel\",\"code\":123456789012345678}"))
                .Should()
                .BeLessOrEqualTo(MaxExecutionTime);

            _parserHelper
                .ExecutionTimeOf(s => s.FromJson<Mocks.ComplexModel>("{\"id\":67890,\"name\":\"ComplexModel\",\"code\":987642341212114498,\"simpleModels\":[{\"id\":1,\"name\":\"SimpleModel\",\"code\":987654347865},{\"id\":2,\"name\":\"SimpleModel\",\"code\":475039592398}]}"))
                .Should()
                .BeLessOrEqualTo(MaxExecutionTime);

            _parserHelper
                .ExecutionTimeOf(s => s.To<Mocks.SimpleModel, dynamic>(new
                {
                    Id = 123,
                    Code = 56789,
                    Name = nameof(Mocks.SimpleModel)
                }))
                .Should()
                .BeLessOrEqualTo(MaxExecutionTime);

            _parserHelper
                .ExecutionTimeOf(s => s.To<dynamic, Mocks.SimpleModel>(new Mocks.SimpleModel()
                {
                    Id = 123,
                    Code = 56789,
                    Name = nameof(Mocks.SimpleModel)
                }))
                .Should()
                .BeLessOrEqualTo(MaxExecutionTime);

            _parserHelper
                .ExecutionTimeOf(s => s.To<Mocks.ComplexModel, Mocks.SimpleModel>(new Mocks.SimpleModel()
                {
                    Id = 123,
                    Code = 56789,
                    Name = nameof(Mocks.SimpleModel)
                }))
                .Should()
                .BeLessOrEqualTo(MaxExecutionTime);
        }

        /// <summary>
        /// Parse de dynamic para json com objetos válidos
        /// </summary>
        [Fact]
        public void parse_tojson_from_simple_dynamic_object()
        {
            _parserHelper
                .ToJson(new { Id = 1, Name = "Abc" })
                .Should()
                .Be("{\"id\":1,\"name\":\"Abc\"}");
        }

        /// <summary>
        /// Parse de dynamic para json com objetos complexos
        /// </summary>
        [Fact]
        public void parse_tojson_from_complex_dynamic_object()
        {
            _parserHelper
                .ToJson(new
                {
                    Id = 1,
                    Name = "Abc",
                    List = new[]
                    {
                        new { Id = 2, Name = "" },
                        new { Id = 3, Name = "XYZ" }
                    },
                    Other = new
                    {
                        OtherId = 4333,
                        OtherName = "Test"
                    }
                })
                .Should()
                .Be("{\"id\":1,\"name\":\"Abc\",\"list\":[{\"id\":2,\"name\":\"\"},{\"id\":3,\"name\":\"XYZ\"}],\"other\":{\"otherId\":4333,\"otherName\":\"Test\"}}");
        }

        /// <summary>
        /// Parse de objetos concreteos simples para json
        /// </summary>
        [Fact]
        public void parse_tojson_from_simple_concrete_object()
        {
            Mocks.SimpleModel simpleModel = new Mocks.SimpleModel()
            {
                Code = 123456789012345678,
                Id = 123,
                Name = nameof(Mocks.SimpleModel)
            };

            _parserHelper
                .ToJson(simpleModel)
                .Should()
                .Be("{\"id\":123,\"name\":\"SimpleModel\",\"code\":123456789012345678}");
        }

        /// <summary>
        /// Parse de objetos concreteos complexos para json
        /// </summary>
        [Fact]
        public void parse_tojson_from_complex_concrete_object()
        {
            Mocks.ComplexModel complexModel = new Mocks.ComplexModel()
            {
                Code = 987642341212114498,
                Id = 67890,
                Name = nameof(Mocks.ComplexModel),
                SimpleModels = new Mocks.SimpleModel[]
                {
                    new Mocks.SimpleModel() { Id = 1, Name = nameof(Mocks.SimpleModel), Code = 987654347865 },
                    new Mocks.SimpleModel() { Id = 2, Name = nameof(Mocks.SimpleModel), Code = 0475039592398 }
                }
            };

            _parserHelper
                .ToJson(complexModel)
                .Should()
                .Be("{\"id\":67890,\"name\":\"ComplexModel\",\"code\":987642341212114498,\"simpleModels\":[{\"id\":1,\"name\":\"SimpleModel\",\"code\":987654347865},{\"id\":2,\"name\":\"SimpleModel\",\"code\":475039592398}]}");
        }

        /// <summary>
        /// Parse para objetos simples
        /// </summary>
        [Fact]
        public void parse_from_json_simple_dynamic_object()
        {
            string json = "{\"id\":123,\"name\":\"SimpleModel\",\"code\":123456789012345678}";

            dynamic model = _parserHelper.FromJson<dynamic>(json);
            string controlJson = _parserHelper.ToJson(model);

            controlJson.Should().Be(json);
        }

        /// <summary>
        /// Parse para objetos complexos
        /// </summary>
        [Fact]
        public void parse_from_json_complex_dynamic_object()
        {
            string json = "{\"id\":67890,\"name\":\"ComplexModel\",\"code\":987642341212114498,\"simpleModels\":[{\"id\":1,\"name\":\"SimpleModel\",\"code\":987654347865},{\"id\":2,\"name\":\"SimpleModel\",\"code\":475039592398}]}";

            dynamic model = _parserHelper.FromJson<dynamic>(json);
            string controlJson = _parserHelper.ToJson(model);

            controlJson.Should().Be(json);
        }

        /// <summary>
        /// Parse para objetos simples
        /// </summary>
        [Fact]
        public void parse_from_json_simple_concrete_object()
        {
            string json = "{\"id\":123,\"name\":\"SimpleModel\",\"code\":123456789012345678}";

            Mocks.SimpleModel simpleModel = _parserHelper
                .FromJson<Mocks.SimpleModel>(json);

            simpleModel.Id.Should().Be(123);
            simpleModel.Name.Should().Be("SimpleModel");
            simpleModel.Code.Should().Be(123456789012345678);
        }

        /// <summary>
        /// Parse para objetos complexos
        /// </summary>
        [Fact]
        public void parse_from_json_complex_concrete_object()
        {
            string json = "{\"id\":67890,\"name\":\"ComplexModel\",\"code\":987642341212114498,\"simpleModels\":[{\"id\":1,\"name\":\"SimpleModel\",\"code\":987654347865},{\"id\":2,\"name\":\"SimpleModel\",\"code\":475039592398}]}";

            Mocks.ComplexModel complexModel = _parserHelper
                .FromJson<Mocks.ComplexModel>(json);

            complexModel.Id.Should().Be(67890);
            complexModel.Name.Should().Be("ComplexModel");
            complexModel.Code.Should().Be(987642341212114498);
            complexModel.SimpleModels.Length.Should().Be(2);
            complexModel.SimpleModels[0].Id.Should().Be(1);
            complexModel.SimpleModels[0].Name.Should().Be("SimpleModel");
            complexModel.SimpleModels[0].Code.Should().Be(987654347865);
            complexModel.SimpleModels[1].Id.Should().Be(2);
            complexModel.SimpleModels[1].Name.Should().Be("SimpleModel");
            complexModel.SimpleModels[1].Code.Should().Be(475039592398);
        }

        /// <summary>
        /// Parse de um objeto dynamic para um objeto concreteo
        /// </summary>
        [Fact]
        public void parse_from_dynamic_to_concrete_simple_object()
        {
            dynamic dynamicModel = new
            {
                Id = 123,
                Code = 56789,
                Name = nameof(Mocks.SimpleModel)
            };

            Mocks.SimpleModel simpleModel = _parserHelper.To<Mocks.SimpleModel, dynamic>(dynamicModel);

            simpleModel.Id.Should().Be(dynamicModel.Id);
            simpleModel.Code.Should().Be(dynamicModel.Code);
            simpleModel.Name.Should().Be(dynamicModel.Name);
        }

        /// <summary>
        /// Parse de um objeto concreteo para um objeto dynamic
        /// </summary>
        [Fact]
        public void parse_from_concrete_to_dynamic_simple_object()
        {
            Mocks.SimpleModel simpleModel = new Mocks.SimpleModel()
            {
                Id = 123,
                Code = 56789,
                Name = nameof(Mocks.SimpleModel)
            };

            dynamic dynamicModel = _parserHelper.To<dynamic, Mocks.SimpleModel>(simpleModel);

            int id = dynamicModel.id;
            long code = dynamicModel.code;
            string name = dynamicModel.name;

            id.Should().Be(simpleModel.Id);
            code.Should().Be(simpleModel.Code);
            name.Should().Be(simpleModel.Name);
        }

        /// <summary>
        /// Parse de um objeto concreteo para outro objeto concreteo
        /// </summary>
        [Fact]
        public void parse_from_concrete_to_concrete_objects()
        {
            Mocks.SimpleModel simpleModel = new Mocks.SimpleModel()
            {
                Id = 123,
                Code = 56789,
                Name = nameof(Mocks.SimpleModel)
            };

            Mocks.ComplexModel complexModel = _parserHelper.To<Mocks.ComplexModel, Mocks.SimpleModel>(simpleModel);

            complexModel.Id.Should().Be(simpleModel.Id);
            complexModel.Code.Should().Be(simpleModel.Code);
            complexModel.Name.Should().Be(simpleModel.Name);
        }
    }
}
