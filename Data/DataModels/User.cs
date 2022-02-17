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
    public class User
    {
        [Key, StringLength(50), Comment("사용자ID"), Column(TypeName = "varchar(50)", Order = 0)]
        public string? UserId { get; set; }

        [Required, StringLength(100), Comment("사용자명"), Column(TypeName = "nvarchar(100)", Order = 1)]
        public string? UserName { get; set; }

        [Required, StringLength(320), Comment("이메일"), Column(TypeName = "varchar(320)", Order = 2)]
        public string? UserEmail { get; set; }

        [Required, StringLength(130), Comment("암호"), Column(TypeName = "nvarchar(130)", Order = 3)]
        public string? Password { get; set; }

        [Comment("삭제일"), Column(Order = 4)]
        public DateTime? DeletedDate { get; set; }

        [Required, Comment("생성일"), Column(Order = 5)]
        public DateTime CreatedUtcDate { get; set; }

        [Comment("최종 수정일"), Column(Order = 6)]
        public DateTime? ModifiedUtcDate { get; set; }

        //FK지정
        [ForeignKey("UserId")]
        public virtual ICollection<UserRolesByUser>? UserRolesByUsers { get; set; }
    }
}
