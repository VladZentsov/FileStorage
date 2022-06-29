using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class StFile:BaseEntity
    {
        public bool IsPublic { get; set; }
        public string Path { get; set; }
        public string UserId { get; set; }
        public UserProfile UserProfile { get; set; }
    }
}
