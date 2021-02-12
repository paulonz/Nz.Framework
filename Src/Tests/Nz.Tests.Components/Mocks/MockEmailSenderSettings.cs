/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Tests.Components.Mocks
{
    using Nz.Libs.EmailSender;

    /// <summary>
    /// Mock para IEmailSenderSettings
    /// </summary>
    public class MockEmailSenderSettings : IEmailSenderSettings
    {
        public string FromEmail => "tests@nzapi.com";

        public string FromName => "Tests";

        public string SmtpHost => "smtp.mailtrap.io";

        public int SmtpPort => 465;

        public string SmtpUser => "9e5a5a2ea1bc5a";

        public string SmtpPassword => "de1fdec669d4e3";
    }
}
