/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Tests.Components.Backend.Libs
{
    using System;
    using FluentAssertions;
    using FluentAssertions.Extensions;
    using Microsoft.Extensions.DependencyInjection;
    using Nz.Libs.Encryption;
    using Nz.Libs.Encryption.Impl.HashAlgorithm;
    using Xunit;

    /// <summary>
    /// Testes para Encryption
    /// </summary>
    public class EncryptionTests
    {
        /// <summary>
        /// Enctyption
        /// </summary>
        private readonly IEncryption _encryption;

        /// <summary>
        /// Tempo máximo para execução do método
        /// </summary>
        private readonly TimeSpan MaxExecutionTime = 10.Milliseconds();

        /// <summary>
        /// Construtor padrão
        /// </summary>
        public EncryptionTests()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddSingleton<IEncryption, Encryption>(impl =>
            {
                return new Encryption(
                                new Mocks.MockEncryptionSettings(),
                                new Mocks.MockGeneralSettings(),
                                new Mocks.MockLogger<Encryption>());
            });

            _encryption = services.BuildServiceProvider().GetService<IEncryption>();
        }

        /// <summary>
        /// Tempo de execução do método deve ser menor que 10 milisegundos
        /// </summary>
        /// <param name="value">Valor para ser criptografado</param>
        [Theory]
        [InlineData("paulo")]
        [InlineData("o8yqefhsadlfhalsdhfl")]
        [InlineData("1234567890")]
        [InlineData("!@#$%¨&*(*¨%$#@")]
        [InlineData("")]
        [InlineData("sliufdy fhalsjkdhfioasas fpsdfhlaskjdhf sufhalsfjhaliyoaiuhflas fl asdhflkajhsdflakjhs dl asldas ldjfhalsjkfdhl  aspdifuhalsdjfhlasdl as dla shldkfjha sldjfhals djfhalsjhalsfhdlahopoweyrp qwe asd pfiwe r")]
        public void execution_must_be_fast(
            string value)
        {
            //warm up
            _encryption
                .Encrypt(value);

            _encryption
                .ExecutionTimeOf(s => s.Encrypt(value))
                .Should()
                .BeLessOrEqualTo(MaxExecutionTime);
        }

        /// <summary>
        /// Criptografar uma string
        /// </summary>
        /// <param name="value">Chave</param>
        /// <param name="expectedResult">String esperada</param>
        [Theory]
        [InlineData("paulo", "B8458E9C1FCA42E244000A9EC32B460C5DCAD0A2318BB0E220117BA69BFDFD0E8E2AF7897834A8F1899FDE6A380FF2A757BDFD71CA58FB6B6D8BD588159EAFF3")]
        [InlineData("o8yqefhsadlfhalsdhfl", "236497F1E442096554AEA8804E49CFA353CBCBA2982C998CFB39E01DA8FF29C99E977B97F85B3575A33AA5ADCBD36C01029919869991296DA800813E4C3E5376")]
        [InlineData("1234567890", "12B03226A6D8BE9C6E8CD5E55DC6C7920CAAA39DF14AAB92D5E3EA9340D1C8A4D3D0B8E4314F1F6EF131BA4BF1CEB9186AB87C801AF0D5C95B1BEFB8CEDAE2B9")]
        [InlineData("!@#$%¨&*(*¨%$#@", "80FD5DFE25BCC7D2A376676915CF8F48670EED28178980FFFF7F164A7F771B1A3985B7D1D60B978F48979436EF776AFCB8B318CF7B2A0A8FAA70E23799308386")]
        [InlineData("", "CF83E1357EEFB8BDF1542850D66D8007D620E4050B5715DC83F4A921D36CE9CE47D0D13C5D85F2B0FF8318D2877EEC2F63B931BD47417A81A538327AF927DA3E")]
        [InlineData("sliufdy fhalsjkdhfioasas fpsdfhlaskjdhf sufhalsfjhaliyoaiuhflas fl asdhflkajhsdflakjhs dl asldas ldjfhalsjkfdhl  aspdifuhalsdjfhlasdl as dla shldkfjha sldjfhals djfhalsjhalsfhdlahopoweyrp qwe asd pfiwe r", "FA5FB8CED67E7EC9DF9EFC5BCC74E5500115BBEAF3AE70CA66C9974A5B3E28A8E4C8E52AD06CD68EBF60B3C30103A3452C8C3E494D667C7119E7AAA19A2E6981")]
        public void encrypted_string_must_be_same_as_expected(
            string value,
            string expectedResult)
        {
            _encryption
                .Encrypt(value)
                .Should()
                .Be(expectedResult);
        }

        /// <summary>
        /// A string criptografada deve possuir 128 caracteres, independente do tamanho da string original
        /// </summary>
        /// <param name="value">Valor para ser criptografado</param>
        [Theory]
        [InlineData("paulo")]
        [InlineData("o8yqefhsadlfhalsdhfl")]
        [InlineData("1234567890")]
        [InlineData("!@#$%¨&*(*¨%$#@")]
        [InlineData("")]
        [InlineData("sliufdy fhalsjkdhfioasas fpsdfhlaskjdhf sufhalsfjhaliyoaiuhflas fl asdhflkajhsdflakjhs dl asldas ldjfhalsjkfdhl  aspdifuhalsdjfhlasdl as dla shldkfjha sldjfhals djfhalsjhalsfhdlahopoweyrp qwe asd pfiwe r")]
        public void encrypted_string_must_be_128_chars(
            string value)
        {
            _encryption
                .Encrypt(value)
                .Length
                .Should()
                .Be(128);
        }
    }
}
