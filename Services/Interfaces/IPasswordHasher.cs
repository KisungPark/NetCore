using Net.Data.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Service.Interfaces
{
    public interface IPasswordHasher
    {
        string GetGUIDSalt();
        string GetRNGSalt();
        string GetPasswordHash(string userId, string password, string guidSalt, string rngSalt);
        //UserService로 이동
        //bool MatchTheUserInfo(string userId, string password);
        bool CheckThePasswordInfo(string userId, string password, string guidSalt, string rngSalt, string passwordHash);

        PasswordHashInfo GetPasswordInfo(string userId, string password);
    }
}
