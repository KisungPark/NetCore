using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Data.ViewModels
{
    public class RegisterInfo : LoginInfo
    {
        [Required(ErrorMessage = "사용자 이름을 입력하세요.")]
        [Display(Name = "사용자 이름")]
        public string UserName { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "사용자 이메일을 입력하세요.")]
        [Display(Name = "사용자 이메일")]
        public string UserEmail { get; set; }
    }
}
