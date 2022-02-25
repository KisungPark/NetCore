using Net.Data.Classes;
using Net.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Service.Interfaces
{
    public interface IUser
    {
        bool MatchTheUserInfo(LoginInfo login);
        User GetUserInfo(string userId);
        IEnumerable<UserRolesByUser> GetRolesOwnedByUser(string userId);
        int RegisterUser(RegisterInfo register);
        /// <summary>
        /// 기존 사용자 정보 검색
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        UserInfo GetUserInfoForUpdate(string userId);
        /// <summary>
        /// 사용자 정보 수정
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        int UpdateUser(UserInfo user);
        /// <summary>
        /// 사용자 정보 변경여부 확인
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        bool CompareInfo(UserInfo user, UserInfo other);
        /// <summary>
        /// 사용자 탈퇴
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        int WithdrawnUser(LoginInfo user);
    }
}
