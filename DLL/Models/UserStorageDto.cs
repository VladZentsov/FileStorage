using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
    public class UserStorageDto:BaseDto
    {
        public string Name { get; set; }
        public IEnumerable<FileDto>? Files { get; set; }
    }
}
