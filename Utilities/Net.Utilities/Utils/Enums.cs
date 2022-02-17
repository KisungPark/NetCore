using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Utilities.Utils
{
    public class Enums
    {
        public enum CryptoType
        {
            Unmanaged = 1,
            Managed = 2,
            CngCbc = 3,
            CngGcm = 4
        }
    }
}
