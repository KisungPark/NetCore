using Net.Data.Classes;
using Net.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Service.Data
{
    public class DBFirstDbInitializer
    {
        private DBFirstDbContext _context;
        private IPasswordHasher _hasher;

        public DBFirstDbInitializer(DBFirstDbContext context, IPasswordHasher hasher)
        {
            _context = context;
            _hasher = hasher;
        }

        /// <summary>
        /// 최초 실행 시 초기 데이터 생성
        /// 테이블에 데이터가 하나도 없으면 실행
        /// 여러 건 추가, 업데이트, 삭제: AddRange(추가), UpdateRange(업데이트), RemoveRange(삭제)
        /// </summary>
        public int PlantSeedData()
        {
            int rowAffected = 0;

            string userId = "admin";
            string password = "admin1!";
            var utcNow = DateTime.UtcNow;
            var passwordInfo = _hasher.GetPasswordInfo(userId, password);

            _context.Database.EnsureCreated();

            if (!_context.Users.Any())
            {
                var users = new List<User>()
                {
                    new User()
                    {
                        UserId = userId.ToLower(),
                        UserName = "관리자",
                        UserEmail = "admin@gmail.com",
                        GUIDSalt = passwordInfo.GUIDSalt,
                        RNGSalt = passwordInfo.RNGSalt,
                        PasswordHash = passwordInfo.PasswordHash,
                        AccessFailedCount = 0,
                        CreatedUtcDate = utcNow,
                    }
                };

                _context.Users.AddRange(users);
                rowAffected += _context.SaveChanges();
            }

            if (!_context.UserRoles.Any())
            {
                var userRoles = new List<UserRole>()
                {
                    new UserRole()
                    {
                        RoleId = "AssociateUser",
                        RoleName = "준사용자",
                        RolePriority = 1,
                        CreatedUtcDate = utcNow,
                    },
                    new UserRole()
                    {
                        RoleId = "GeneralUser",
                        RoleName = "일반사용자",
                        RolePriority = 2,
                        CreatedUtcDate = utcNow,
                    },
                    new UserRole()
                    {
                        RoleId = "SuperUser",
                        RoleName = "향상된 사용자",
                        RolePriority = 3,
                        CreatedUtcDate = utcNow,
                    },
                    new UserRole()
                    {
                        RoleId = "SystemUser",
                        RoleName = "시스템 사용자",
                        RolePriority = 4,
                        CreatedUtcDate = utcNow,
                    },
                };

                _context.UserRoles.AddRange(userRoles);
                rowAffected += _context.SaveChanges();
            }

            if (!_context.UserRolesByUsers.Any())
            {
                var userRolesByUsers = new List<UserRolesByUser>()
                {
                    new UserRolesByUser()
                    {
                        UserId = userId.ToLower(),
                        RoleId = "GeneralUser",
                        OwnedUtcDate = utcNow,
                    },
                    new UserRolesByUser()
                    {
                        UserId = userId.ToLower(),
                        RoleId = "SuperUser",
                        OwnedUtcDate = utcNow,
                    },
                    new UserRolesByUser()
                    {
                        UserId = userId.ToLower(),
                        RoleId = "SystemUser",
                        OwnedUtcDate = utcNow,
                    },
                };

                _context.UserRolesByUsers.AddRange(userRolesByUsers);
                rowAffected += _context.SaveChanges();
            }

            return rowAffected;
        }
    }
}
