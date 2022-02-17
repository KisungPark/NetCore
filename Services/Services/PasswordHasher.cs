using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Net.Service.Data;
using Net.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Net.Service.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        private DBFirstDbContext _context;

        public PasswordHasher(DBFirstDbContext context)
        {
            _context = context;
        }

        #region private method
        private string GetGUIDSalt()
        {
            return Guid.NewGuid().ToString();
        }

        private string GetRNGSalt()
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            return Convert.ToBase64String(salt);
        }

        private string GetPasswordHash(string userId, string password, string guidSalt, string rngSalt)
        {
            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: userId + password + guidSalt, //복잡도 증가를 위해 userId와 guidSalt 추가
                salt: Encoding.UTF8.GetBytes(rngSalt), //byte로 변경
                prf: KeyDerivationPrf.HMACSHA512, //SHA512로 변경
                iterationCount: 45000, //10000,25000,45000
                numBytesRequested: 256 / 8));
        }

        private bool MatchTheUserInfo(string userId, string password, string guidSalt, string rngSalt, string passwordHash)
        {
            return GetPasswordHash(userId, password, guidSalt, rngSalt).Equals(passwordHash);
        }
        #endregion

        string IPasswordHasher.GetGUIDSalt()
        {
            return GetGUIDSalt();
        }

        string IPasswordHasher.GetRNGSalt()
        {
            return GetRNGSalt();
        }

        string IPasswordHasher.GetPasswordHash(string userId, string password, string guidSalt, string rngSalt)
        {
            return GetPasswordHash(userId, password, guidSalt, rngSalt);
        }

        // UserService로 이동
        //bool IPasswordHasher.MatchTheUserInfo(string userId, string password)
        //{
        //    var user = _context.Users.Where(u => u.UserId.Equals(userId)).FirstOrDefault();
        //
        //    string guidSalt = user.GUIDSalt;
        //    string rngSalt = user.RNGSalt;
        //    string passwordHash = user.PasswordHash;
        //
        //    return MatchTheUserInfo(userId, password, guidSalt, rngSalt, passwordHash);
        //}

        public bool CheckThePasswordInfo(string userId, string password, string guidSalt, string rngSalt, string passwordHash)
        {
            return MatchTheUserInfo(userId, password, guidSalt, rngSalt, passwordHash);
        }
    }
}
