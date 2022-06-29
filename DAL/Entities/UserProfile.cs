using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class UserProfile: BaseEntity
    {
        public string SysIdentityId { get; set; }
        public SysIdentityUser? SysIdentityUser { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? Description { get; set; }
        public string? UserStorageId { get; set; } 
        public UserStorage UserStorage { get; set; }

        public ICollection<StorageAccessibility> AccessibleStorages { get; set; }

        public ICollection<FileAccessibility> AccessibleFiles { get; set; }

    }
}
