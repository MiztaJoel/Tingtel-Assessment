using Tingtel_Assessment.Core.Utilities;

namespace Core.Interfaces.ServiceInterfaces
{
	public interface IEmailService
	{
		Task SendEmailAsync(MailRequest mailRequest);
	}
}
