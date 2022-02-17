using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Net.Data.ViewModels;
using Net.Service.Interfaces;
using System.Security.Claims;

namespace Net.Web.Controllers
{
    [Authorize(Roles ="AssociateUser, GeneralUser, SuperUser, SystemUser")]
    public class MembershipController : Controller
    {
        //의존성 주입
        private IUser _user;
        private IPasswordHasher _hasher;
        private HttpContext _context;

        public MembershipController(IHttpContextAccessor accessor, IPasswordHasher hasher, IUser user)
        {
            _context = accessor.HttpContext;
            _hasher  = hasher;
            _user    = user;
        }

        #region private method

        /// <summary>
        /// 로컬URL인지 외부URL인지 체크
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                // "Index" 문자나 nameof(MembershipController.Index)이거나 결국 같음
                return RedirectToAction(nameof(MembershipController.Index),"Membership");
            }
        }
        #endregion

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View(new LoginInfo());
        }

        [HttpPost("/{controller}/Login")]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync(LoginInfo login, string? returnUrl=null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            string message = String.Empty;
            if (ModelState.IsValid)
            {
                //if(_hasher.MatchTheUserInfo(login.UserId, login.Password))
                if(_user.MatchTheUserInfo(login)) //UserService
                {
                    //신원보증과 승인권한
                    var userInfo = _user.GetUserInfo(login.UserId);
                    var roles = _user.GetRolesOwnedByUser(login.UserId);
                    var userTopRole = roles.FirstOrDefault();
                    string userDataInfo = userTopRole.UserRole.RoleName + "|" +
                                        userTopRole.UserRole.RolePriority.ToString() + "|" +
                                        userInfo.UserName + "|" +
                                        userInfo.UserEmail;

                    //_context.User.Identity.Name => 사용자 아이디

                    var identity = new ClaimsIdentity(claims: new[] { 
                        new Claim(type: ClaimTypes.Name, value: userInfo.UserName),
                        new Claim(type: ClaimTypes.Role, value: userTopRole.RoleId),
                        new Claim(type: ClaimTypes.UserData, value: userDataInfo)
                    }, authenticationType: CookieAuthenticationDefaults.AuthenticationScheme);
                    await _context.SignInAsync(
                        scheme:CookieAuthenticationDefaults.AuthenticationScheme, 
                        principal: new ClaimsPrincipal(identity:identity), 
                        properties: new AuthenticationProperties()
                        {
                            IsPersistent = login.RemenberMe,
                            ExpiresUtc = login.RemenberMe ? DateTime.UtcNow.AddDays(7) : DateTime.UtcNow.AddMinutes(30)
                        });

                    TempData["Message"] = "로그인 성공!";
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    message = "로그인 정보 불일치";
                }
            }
            else
            {
                message = "로그인 실패...";
            }
            ModelState.AddModelError(string.Empty, message);
            return View("Login",login);
        }

        [HttpGet("/LogOut")]
        public async Task<IActionResult> LogOutAsync()
        {
            await _context.SignOutAsync(scheme:CookieAuthenticationDefaults.AuthenticationScheme);
            TempData["Message"] = "로그아웃이 성공적으로 이루어졌습니다.";
            return RedirectToAction("index", "membership");
        }

        [HttpGet]
        public IActionResult Forbidden()
        {
            StringValues paramReturnUrl;
            bool exists = _context.Request.Query.TryGetValue("returnUrl", out paramReturnUrl);
            //온전한 Url 형태로 만듦
            paramReturnUrl = exists ? _context.Request.Host.Value + paramReturnUrl[0] : string.Empty;
            ViewData["Message"] = $"귀하는 {paramReturnUrl} 경로로 접근했습니다만,<br/>"+
                                  "해당 페이지에 대한 접근권한이 없습니다.<br/>"+
                                  "담당자에게 문의해주세요.";
            return View();
        }
    }
}
