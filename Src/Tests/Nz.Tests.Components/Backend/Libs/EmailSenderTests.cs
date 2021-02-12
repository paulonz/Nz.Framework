/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Tests.Components.Backend.Libs
{
    using System;
    using System.Threading.Tasks;
    using FluentAssertions;
    using FluentAssertions.Extensions;
    using Microsoft.Extensions.DependencyInjection;
    using Nz.Libs.EmailSender;
    using Nz.Libs.EmailSender.Impl.Smtp;
    using Xunit;

    /// <summary>
    /// Testes para EmailSender
    /// </summary>
    public class EmailSenderTests
    {
        /// <summary>
        /// Email Sender
        /// </summary>
        private readonly IEmailSender _emailSender;

        /// <summary>
        /// Tempo máximo para execução do método
        /// </summary>
        private readonly TimeSpan MaxExecutionTime = 10.Milliseconds();

        /// <summary>
        /// Construtor padrão
        /// </summary>
        public EmailSenderTests()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddSingleton<IEmailSender, EmailSender>(impl =>
            {
                return new EmailSender(
                    new Mocks.MockEmailSenderSettings(),
                    new Mocks.MockLogger<EmailSender>());
            });

            _emailSender = services.BuildServiceProvider().GetService<IEmailSender>();
        }

        [Theory]
        [InlineData(null, "subject", "body")]
        [InlineData("to@test.com", null, "body")]
        [InlineData("to@test.com", "subject", null)]
        [InlineData(null, null, null)]
        [InlineData("josé", "subject", "body")]
        [InlineData("12345", "subject", "body")]
        [InlineData("maria.silva", "subject", "body")]
        [InlineData("paulo@nzapi.com", "subject", "body")]
        public async Task execution_must_be_fast(
            string to,
            string subject,
            string body)
        {
            //warm up
            await _emailSender.SendAsync(to, subject, body).ConfigureAwait(false);

            _emailSender
                .ExecutionTimeOf(s => s.SendAsync(to, subject, body))
                .Should()
                .BeLessOrEqualTo(MaxExecutionTime);
        }

        /// <summary>
        /// Tenta fazer o envio de email onde um dos parametros é nulo
        /// </summary>
        /// <param name="to">Destinatário</param>
        /// <param name="subject">Assunto</param>
        /// <param name="body">Conteúdo do email</param>
        /// <returns>Sucesso</returns>
        [Theory]
        [InlineData(null, "subject", "body")]
        [InlineData("to@test.com", null, "body")]
        [InlineData("to@test.com", "subject", null)]
        [InlineData(null, null, null)]
        public async Task send_mail_without_all_parameters(
            string to,
            string subject,
            string body)
        {
            bool result = await _emailSender
                                    .SendAsync(to, subject, body)
                                    .ConfigureAwait(false);

            result
                .Should()
                .BeFalse();
        }

        /// <summary>
        /// Tenta fazer o envio de email onde o destinatário é inválido
        /// </summary>
        /// <param name="to">Destinatário</param>
        /// <returns>Sucesso</returns>
        [Theory]
        [InlineData(null)]
        [InlineData("josé")]
        [InlineData("12345")]
        [InlineData("maria.silva")]
        public async Task send_mail_with_invalid_to_parameter(
            string to)
        {
            bool result = await _emailSender
                                    .SendAsync(to, "subject", "body")
                                    .ConfigureAwait(false);

            result
                .Should()
                .BeFalse();
        }

        /// <summary>
        /// Tentativa de enviar um email com dados válidos
        /// </summary>
        /// <returns>Sucesso</returns>
        [Fact]
        public async Task send_valid_email()
        {
            bool result = await _emailSender
                                    .SendAsync("paulo@nzapi.com", "subject", "body")
                                    .ConfigureAwait(false);

            result
                .Should()
                .BeTrue();
        }
    }
}
