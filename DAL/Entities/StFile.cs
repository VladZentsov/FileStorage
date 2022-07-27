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
        public string Name { get; set; }
        public string Weight { get; set; }
        public string UserStorageId { get; set; }
        public string UserProfileId { get; set; }
        public UserProfile? UserProfile { get; set; }
    }
}
