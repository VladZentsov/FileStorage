using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
    public class UserProfileDto : BaseDto
    {
        public string SysIdentityId { get; set; }
        public SysIdentityUser? SysIdentityUser { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? Description { get; set; }
        public int? UserStorageId { get; set; }
        public UserStorage UserStorage { get; set; }
        public ICollection<FileDto> Files { get; set; }
    }
}
