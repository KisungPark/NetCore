using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Data.DataModels
{
    public class UserRolesByUser
    {
        [Key, StringLength(50), Comment("사용자ID"), Column(TypeName = "varchar(50)", Order = 0)]
        public string? UserId { get; set; }

        [Key, StringLength(50), Comment("역할ID"), Column(TypeName = "varchar(50)", Order = 1)]
        public string? RoleId { get; set; }

        [Required, Comment("적용일"), Column(Order = 2)]
        public DateTime OwnedUtcDate { get; set; }

        //FK로 연결된 테이블
        public virtual User? User { get; set; }

        //FK로 연결된 테이블
        public virtual UserRole? UserRole { get; set; }
    }
}
