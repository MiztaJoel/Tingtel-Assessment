using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tingtel_Assessment.Core.Utilities
{
	public class NotFoundException:Exception
	{
		public NotFoundException(string message):base(message) { }
	}

}

