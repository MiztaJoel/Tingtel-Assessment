using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tingtel_Assessment.Core.Constant
{
	public class PagedResponse<T>
	{
		public IEnumerable<T> Data { get; set; }
		public int TotalRecords {  get; set; }
		public int PageNumber { get; set; }
		public int PageSize { get; set; }

		public PagedResponse(IEnumerable<T>data,int totalRecords,int pageNumber, int pageSize) =>
			(Data,TotalRecords,PageNumber,PageSize)=(data,totalRecords,pageNumber,pageSize);
		
	}
}
