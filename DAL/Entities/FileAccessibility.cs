using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class FileAccessibility:BaseEntity
    {
        public string UserProfileId { get; set; }
        public string FileId { get; set; }
        public StFile File { get; set; }

    }
}
