using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class StorageAccessibility:BaseEntity
    {
        public string StorageId { get; set; }
        public string UserProfileId { get; set; }
        public UserStorage? UserStorage { get; set; }
    }
}
