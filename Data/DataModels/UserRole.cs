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
    public class UserRole
    {
        [Key, StringLength(50), Comment("역할ID"), Column(TypeName = "varchar(50)", Order = 0)]
        public string? RoleId { get; set; }

        [Required, StringLength(100), Comment("역할명"), Column(TypeName = "nvarchar(100)", Order = 1)]
        public string? RoleName { get; set; }

        [Required, Comment("우선순위"), Column(Order = 2)]
        public byte RolePriority { get; set; }

        [Required, Comment("생성일"), Column(Order = 3)]
        public DateTime CreatedUtcDate { get; set; }

        [Comment("최종 수정일"), Column(Order = 4)]
        public DateTime? ModifiedUtcDate { get; set; }

        //FK지정
        [ForeignKey("RoleId")]
        public virtual ICollection<UserRolesByUser>? UserRolesByUsers { get; set; }
    }
}
