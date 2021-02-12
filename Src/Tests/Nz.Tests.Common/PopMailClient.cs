/*
 * Nz.Framework
 * Author Paulo Eduardo Nazeazeno
 * https://github.com/paulonz/Nz.Framework
 */

namespace Nz.Tests.Common
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using MailKit.Net.Pop3;
    using MimeKit;

    public class PopMailClient
    {
        private readonly string _serverAddress;
        private readonly int _port;
        private readonly string _username;
        private readonly string _password;

        public PopMailClient(
            string serverAddress,
            int port,
            string username,
            string password)
        {
            _serverAddress = serverAddress;
            _port = port;
            _username = username;
            _password = password;
        }

        public async Task<Models.MailMessage[]> ReceiveAsync()
        {
            try
            {
                using Pop3Client emailClient = new Pop3Client();

                await emailClient.ConnectAsync(_serverAddress, _port, false).ConfigureAwait(false);
                //emailClient.AuthenticationMechanisms.Add("USER/PASS");
                await emailClient.AuthenticateAsync(_username, _password);
                int totalMessages = await emailClient.GetMessageCountAsync().ConfigureAwait(false);

                IList<Models.MailMessage> emails = new List<Models.MailMessage>();

                for (int i = 0; i < 50 && i < totalMessages; i++)
                {
                    MimeMessage message = await emailClient.GetMessageAsync(i).ConfigureAwait(false);

                    emails.Add(new Models.MailMessage()
                    {
                        Body = !string.IsNullOrEmpty(message.HtmlBody) ? message.HtmlBody : message.TextBody,
                        Subject = message.Subject,
                        To = message.To.Select(x => (MailboxAddress)x).Select(x => x.Address).First(),
                        From = message.From.Select(x => (MailboxAddress)x).Select(x => x.Address).First()
                    });
                }

                return emails.ToArray();
            }
            catch { }

            return null;
        }
    }
}
