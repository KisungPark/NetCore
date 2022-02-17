using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Data.Classes
{
    public class UserRole
    {
        [Key]
        public string? RoleId { get; set; }
        public string? RoleName { get; set; }
        public byte RolePriority { get; set; }
        public Nullable<System.DateTime> ModifiedUtcDate { get; set; }
        public System.DateTime CreatedUtcDate { get; set; }

        public virtual ICollection<UserRolesByUser> UserRolesByUsers { get; set; }
    }
}
