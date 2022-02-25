using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Data.ViewModels
{
    public class UserInfo : RegisterInfo
    {
        public bool Equals(UserInfo other)
        {
            if (!string.Equals(UserName, other.UserName, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (!string.Equals(UserEmail, other.UserEmail, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return true;
        }
    }
}
