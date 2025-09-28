using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tingtel_Assessment.Core.Utilities
{
	public class MailRequest
	{
			public List<string> ToEmail { get; set; }

			public string Subject { get; set; }

			public string Body { get; set; }

			bool IsHtml { get; set; }

			public List<string>? Attachments { get; set; }

			public MailRequest(List<string> toEmail, string subject, string body, bool isHtml = false, List<string>? attachments = null)
			{
				ToEmail = toEmail;
				Subject = subject;
				Body = body;
				IsHtml = isHtml;
				Attachments = attachments;
			}
	}
	
}
