using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
    public class FileDto: BaseDto
    {
        public bool IsPublic { get; set; }
        public string Name { get; set; }
        public int Weight { get; set; }

    }
}
