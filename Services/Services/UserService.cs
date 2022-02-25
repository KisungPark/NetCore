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

        //대소문자 처리(ID)
        private int RegisterUser(RegisterInfo register)
        {
            var utcNow = DateTime.UtcNow;
            var passwordInfo = _hasher.GetPasswordInfo(register.UserId, register.Password);

            var user = new User()
            {
                UserId = register.UserId.ToLower(),
                UserName = register.UserName,
                UserEmail = register.UserEmail,
                GUIDSalt = passwordInfo.GUIDSalt,
                RNGSalt = passwordInfo.RNGSalt,
                PasswordHash = passwordInfo.PasswordHash,
                AccessFailedCount = 0,
                CreatedUtcDate = utcNow,
            };
            var userRolesByUser = new UserRolesByUser()
            {
                UserId = register.UserId.ToLower(),
                RoleId = "AssociateUser",
                OwnedUtcDate = utcNow,
            };

            //DB Insert
            _context.Add(user);
            _context.Add(userRolesByUser);

            return _context.SaveChanges();
        }

        private UserInfo GetUserInfoForUpdate(string userId)
        {
            var user = GetUserInfo(userId);
            var userInfo = new UserInfo()
            {
                UserId = userId,
                UserName = user.UserName,
                UserEmail = user.UserEmail,
            };

            return userInfo;
        }

        private int UpdateUser(UserInfo user)
        {
            int rowAffected = 0;

            //bool check = MatchTheUserInfo(user);
            var userInfo = _context.Users.Where(u => u.UserId.Equals(user.UserId)).FirstOrDefault();
            if(userInfo == null)
            {
                return 0;
            }
            bool check = _hasher.CheckThePasswordInfo(user.UserId, user.Password, userInfo.GUIDSalt, userInfo.RNGSalt, userInfo.PasswordHash);

            if (check)
            {
                _context.Update(userInfo);
                userInfo.UserName = user.UserName;
                userInfo.UserEmail = user.UserEmail;

                rowAffected = _context.SaveChanges();
            }

            return rowAffected;
        }

        private bool MatchTheUserInfo(LoginInfo login)
        {
            var user = _context.Users.Where(u => u.UserId.Equals(login.UserId)).FirstOrDefault();

            if (user == null)
            {
                return false;
            }
            return _hasher.CheckThePasswordInfo(login.UserId, login.Password, user.GUIDSalt, user.RNGSalt, user.PasswordHash);
        }

        private bool CompareInfo(UserInfo user, UserInfo other)
        {
            return user.Equals(other);
        }

        private int WithdrawnUser(LoginInfo user)
        {
            int rowAffected = 0;
            var userInfo = _context.Users.Where(u => u.UserId.Equals(user.UserId)).FirstOrDefault();

            if (userInfo == null)
            {
                return 0;
            }
            bool check = _hasher.CheckThePasswordInfo(user.UserId, user.Password, userInfo.GUIDSalt, userInfo.RNGSalt, userInfo.PasswordHash);

            if (check)
            {
                _context.Remove(userInfo);
                rowAffected = _context.SaveChanges();

            }
            return rowAffected;
        }
        #endregion

        bool IUser.MatchTheUserInfo(LoginInfo login)
        {
            return MatchTheUserInfo(login);
        }

        User IUser.GetUserInfo(string userId)
        {
            return GetUserInfo(userId);
        }

        IEnumerable<UserRolesByUser> IUser.GetRolesOwnedByUser(string userId)
        {
            return GetUserRolesByUserInfos(userId);
        }

        int IUser.RegisterUser(RegisterInfo register)
        {
            return RegisterUser(register);
        }

        UserInfo IUser.GetUserInfoForUpdate(string userId)
        {
            return GetUserInfoForUpdate(userId);
        }

        int IUser.UpdateUser(UserInfo user)
        {
            return UpdateUser(user);
        }

        bool IUser.CompareInfo(UserInfo user, UserInfo other)
        {
            return CompareInfo(user, other);
        }

        int IUser.WithdrawnUser(LoginInfo user)
        {
            return WithdrawnUser(user);
        }
    }
}
