using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Data.ViewModels
{
    public class AESInfo : LoginInfo
    {
        [DataType(DataType.MultilineText)]
        [Display(Name ="암호화 정보")]
        public string? EncUserInfo { get; set; }

        [Display(Name = "복호화 정보")]
        public string? DecUserInfo { get; set; }
    }
}
