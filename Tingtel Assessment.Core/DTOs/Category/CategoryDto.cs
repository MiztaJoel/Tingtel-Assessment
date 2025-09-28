using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tingtel_Assessment.Core.DTOs.Category
{
    public class CategoryDto
    {
        public string CategoryId { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string? Description { get; set; }
    }
}
