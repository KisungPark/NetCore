using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Net.Data.Classes;
//using Net.Data.DataModels;
using Net.Data.ViewModels;
using Net.Service.Data;
using Net.Service.Interfaces;

namespace Net.Service.Services
{
    public class UserService : IUser
    {
        private DBFirstDbContext _context;
        private IPasswordHasher _hasher;

        public UserService(DBFirstDbContext context, IPasswordHasher hasher)
        {
            _context = context;
            _hasher = hasher;
        }

        #region private methods
        private IEnumerable<User> GetUserInfos()
        {
            return _context.Users.ToList();
            //return new List<User>()
            //{
            //    new User()
            //    {
            //        UserId = "itcpak",
            //        UserName ="박기성",
            //        UserEmail ="itcpak@naver.com",
            //        Password ="123456"
            //    }
            //};
        }

        private User GetUserInfo(string userId, string password)
        {
            User user;

            // Lambda 방식
            //user = _context.Users.Where(u => u.UserId.Equals(userId) && u.Password.Equals(password)).FirstOrDefault();

            // FromSql 방식
            // 1. Talbe(View도 가능)
            user = _context.Users.FromSqlInterpolated($"SELECT * FROM dbo.[User] where UserId = {userId} And Password = {password}").FirstOrDefault();
            //user = _context.Users.FromSqlInterpolated($"SELECT * FROM dbo.[User]").Where(u => u.UserId.Equals(userId) && u.Password.Equals(password)).FirstOrDefault();
            // 2. Function (테이블 반환 함수 작성 필요)
            //user = _context.Users.FromSqlInterpolated($"SELECT * FROM dbo.ufnUser({userId},{password})").FirstOrDefault();
            // 3. Procedure (프로시저 작성 필요)
            //user = _context.Users.FromSqlRaw("EXECUTE dbo.uspCheckLoginBtUserId @p0, @p1", new[] {userId, password}).FirstOrDefault();

            if (user == null)
            {
                //접속시도 횟수 증가
                int rowAffected = _context.Database.ExecuteSqlInterpolated($"UPDATE dbo.[User] SET AccessFailedCount += 1 WHERE UserId = { userId }");
            }

            return user;
        }

        private bool checkTheUserInfo(string userId, string password)
        {
            //return GetUserInfos().Where(u => u.UserId.Equals(userId) && u.Password.Equals(password)).Any();
            return GetUserInfo(userId, password) != null ? true : false;
        }

        private User GetUserInfo(string userId)
        {
            return _context.Users.Where(u => u.UserId.Equals(userId)).FirstOrDefault();
        }

        private UserRole GetUserRole(string roleId)
        {
            return _context.UserRoles.Where(r => r.RoleId.Equals(roleId)).FirstOrDefault();
        }

        private IEnumerable<UserRolesByUser> GetUserRolesByUserInfos(string userId)
        {
            var userRolesByUserInfos = _context.UserRolesByUsers.Where(uru => uru.UserId.Equals(userId)).ToList();

            foreach(var role in userRolesByUserInfos)
            {
                role.UserRole = GetUserRole(role.RoleId);
            }

            return userRolesByUserInfos.OrderByDescending(uru => uru.UserRole.RolePriority);
        }
        #endregion

        bool IUser.MatchTheUserInfo(LoginInfo login)
        {
            var user = _context.Users.Where(u => u.UserId.Equals(login.UserId)).FirstOrDefault();

            if(user == null)
            {
                return false;
            }
            return _hasher.CheckThePasswordInfo(login.UserId, login.Password, user.GUIDSalt, user.RNGSalt, user.PasswordHash);
        }

        User IUser.GetUserInfo(string userId)
        {
            return GetUserInfo(userId);
        }

        IEnumerable<UserRolesByUser> IUser.GetRolesOwnedByUser(string userId)
        {
            return GetUserRolesByUserInfos(userId);
        }
    }
}
