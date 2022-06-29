using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class UserStorage:BaseEntity
    {
        public string UserProfileId { get; set; }
        public UserProfile UserProfile { get; set; }
        public ICollection<StFile>? Files { get; set; }
    }
}
