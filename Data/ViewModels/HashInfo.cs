using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Data.ViewModels
{
    public class HashInfo : LoginInfo
    {
        [Display(Name = "GUID Salt")]
        public string? GUIDSalt { get; set; }

        [Display(Name = "RNG Salt")]
        public string? RNGSalt { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "비밀번호 Hash값")]
        public string? PasswordHash { get; set; }
    }
}
