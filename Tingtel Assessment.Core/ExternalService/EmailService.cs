using Core.Interfaces.ServiceInterfaces;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Tingtel_Assessment.Core.Utilities;

namespace Tingtel_Assessment.Core.ExternalService
{
	public class EmailService : IEmailService
	{
		private readonly EmailSettings emailSettings;
		public EmailService(IOptions<EmailSettings> options)
		{
			emailSettings = options.Value;
		}



		public async Task SendEmailAsync(MailRequest mailRequest)
		{
			var email = new MimeMessage();

			email.Sender = MailboxAddress.Parse(emailSettings.Email);
			email.Subject = mailRequest.Subject;
			var builder = new BodyBuilder();
			builder.HtmlBody = mailRequest.Body;
			

			foreach (var recipient in mailRequest.ToEmail)
			{
				email.To.Add(MailboxAddress.Parse(recipient));
			}

			if (mailRequest.Attachments != null)
			{
				foreach (var filePath in mailRequest.Attachments) 
				{
					if (File.Exists(filePath))
					{
						builder.Attachments.Add(filePath);						
					}
				}
			}
			email.Body = builder.ToMessageBody();

			using var smtp = new MailKit.Net.Smtp.SmtpClient();
			try
			{
				smtp.Connect(emailSettings.Host, emailSettings.Port, SecureSocketOptions.StartTls);
				smtp.Authenticate(emailSettings.Email, emailSettings.Password);

				await smtp.SendAsync(email);

			}
			catch
			{
				throw;
			}
			finally
			{
				smtp.Disconnect(true);
				smtp.Dispose();
			}
		}
	}
}
