using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Data.ViewModels
{
    public class LoginInfo
    {
        [Required(ErrorMessage ="ID를 입력하세요.")]
        [MinLength(5,ErrorMessage ="5자 이상 입력해주세요.")]
        [Display(Name="아이디")]
        public string? UserId {get; set;}

        [Required(ErrorMessage = "PW를 입력하세요.")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "6자 이상 입력해주세요.")]
        [Display(Name = "비밀번호")]
        public string? Password { get; set; }

        [Display(Name="내 정보 기억")]
        public bool RemenberMe { get; set; }
    }
}
