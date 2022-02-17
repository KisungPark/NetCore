using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Data.Classes
{
    public class User
    {
        [Key]
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
        public string? GUIDSalt { get; set; }
        public string? RNGSalt { get; set; }
        public string? PasswordHash { get; set; }
        //public string? Password { get; set; } //비밀번호는 암호화해서 사용
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public Nullable<System.DateTime> ModifiedUtcDate { get; set; }
        public System.DateTime CreatedUtcDate { get; set; }

        public int AccessFailedCount { get; set; }

        public virtual ICollection<UserRolesByUser>? UserRolesByUsers { get; set; }
    }
}
