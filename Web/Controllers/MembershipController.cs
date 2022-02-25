using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Net.Data.ViewModels;
using Net.Service.Interfaces;
using System.Reflection;
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

        /// <summary>
        /// 탈퇴 후 접근해야 해서 [AllowAnonymous] 추가
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View(new RegisterInfo());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterInfo register, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                if(_user.RegisterUser(register) > 0)
                {
                    //사용자 가입 서비스
                    TempData["message"] = "사용자 가입 성공.";
                    return RedirectToAction("Login", "Membership");
                }
                else
                {
                    message = "사용자 가입 실패";
                }
            }
            else
            {
                message = "사용자 가입을 위한 정보를 올바르게 입력하세요.";
            }
            ModelState.AddModelError(string.Empty, message);
            return View(register);
        }

        [HttpGet]
        public IActionResult Update()
        {
            UserInfo user = _user.GetUserInfoForUpdate(_context.User.Identity.Name);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(UserInfo changeInfo)
        {
            var message = string.Empty;
            if (ModelState.IsValid)
            {
                //변경값 비교
                UserInfo originInfo = _user.GetUserInfoForUpdate(_context.User.Identity.Name);
                if (_user.CompareInfo(originInfo, changeInfo))
                {
                    message = "변경된 내용이 없습니다.";
                    ModelState.AddModelError(string.Empty, message);
                    return View(changeInfo);
                }

                //정보수정
                if(_user.UpdateUser(changeInfo) > 0)
                {
                    TempData["Message"] = "사용자 정보수정 성공";
                    return RedirectToAction("Update", "Membership");

                }
                else
                {
                    message = "사용자 정보가 수정되지 않았습니다.";
                }
            }
            else
            {
                message = "사용자 정보를 올바르게 입력하세요.";
            }
            ModelState.AddModelError(string.Empty, message);
            return View(changeInfo);
        }

        [HttpPost("/Withdrawn")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> WithdrawnAsync(LoginInfo user)
        {
            var message = string.Empty;
            if (ModelState.IsValid)
            {
                //정보수정
                if (_user.WithdrawnUser(user) > 0)
                {
                    TempData["Message"] = "탈퇴 성공";
                    await _context.SignOutAsync(scheme: CookieAuthenticationDefaults.AuthenticationScheme);
                    return RedirectToAction("Index", "Membership");

                }
                else
                {
                    message = "비밀번호를 확인해주세요.";
                }
            }
            else
            {
                message = "사용자 정보를 올바르게 입력하세요.";
            }
            ViewData["message"] = message;
            return View("index",user);
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
                        new Claim(type: ClaimTypes.Name, value: userInfo.UserId),
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
        public IActionResult Forbidden([FromServices] ILogger<MembershipController> logger)
        {
            StringValues paramReturnUrl;
            bool exists = _context.Request.Query.TryGetValue("returnUrl", out paramReturnUrl);
            //온전한 Url 형태로 만듦
            paramReturnUrl = exists ? _context.Request.Host.Value + paramReturnUrl[0] : string.Empty;

            logger.LogTrace($"{MethodBase.GetCurrentMethod().Name} 매서드 권한이 없는 사람이 접근. returnUrl : {paramReturnUrl}");

            ViewData["Message"] = $"귀하는 {paramReturnUrl} 경로로 접근했습니다만,<br/>"+
                                  "해당 페이지에 대한 접근권한이 없습니다.<br/>"+
                                  "담당자에게 문의해주세요.";
            return View();
        }
    }
}
