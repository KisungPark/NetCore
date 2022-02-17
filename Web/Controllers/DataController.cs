using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Net.Data.ViewModels;
using Net.Service.Interfaces;

namespace Net.Web.Controllers
{
    public class DataController : Controller
    {
        private IDataProtector _protector;
        private IPasswordHasher _hasher;

        public DataController(IDataProtectionProvider provider, IPasswordHasher hasher)
        {
            _protector = provider.CreateProtector("Net.Data.v1");
            _hasher = hasher;
        }

        [HttpGet]
        [Authorize(Roles = "GeneralUser, SuperUser, SystemUser")]
        public IActionResult AES()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "GeneralUser, SuperUser, SystemUser")]
        [ValidateAntiForgeryToken]
        public IActionResult AES(AESInfo aes)
        {
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                string userInfo = aes.UserId + aes.Password;
                aes.EncUserInfo = _protector.Protect(userInfo); //암호화 정보
                aes.DecUserInfo = _protector.Unprotect(aes.EncUserInfo); //복호화 정보
                ViewData["Message"] = "암복호화가 성공적으로 이루어졌습니다.";
                return View(aes);
            }
            else
            {
                message = "암복호화를 위한 정보를 올바르게 입력하세요.";
            }
            ModelState.AddModelError(string.Empty, message);
            return View(aes);
        }

        [HttpGet]
        public IActionResult Hash()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Hash(HashInfo hash)
        {
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                hash.GUIDSalt = _hasher.GetGUIDSalt();
                hash.RNGSalt = _hasher.GetRNGSalt();
                hash.PasswordHash = _hasher.GetPasswordHash(hash.UserId, hash.Password, hash.GUIDSalt, hash.RNGSalt);
                ViewData["Message"] = "암호 해쉬가 성공적으로 생성되었습니다.";
                return View(hash);
            }
            else
            {
                message = "암호 해쉬를 위한 정보를 올바르게 입력하세요.";
            }
            ModelState.AddModelError(string.Empty, message);
            return View(hash);
        }
    }
}
