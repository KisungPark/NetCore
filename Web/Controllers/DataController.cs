using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Net.Data.ViewModels;
using Net.Service.Interfaces;
using Net.Web.Extensions;

namespace Net.Web.Controllers
{
    public class DataController : Controller
    {
        private IDataProtector _protector;
        private IPasswordHasher _hasher;
        private HttpContext _context;
        private string _sessionKeyCartName = "_sessionCartKey";

        public DataController(IDataProtectionProvider provider, IPasswordHasher hasher, IHttpContextAccessor accessor)
        {
            _protector = provider.CreateProtector("Net.Data.v1");
            _hasher = hasher;
            _context = accessor.HttpContext;
        }

        #region private method
        private void SetCartInfos(ItemInfo item, List<ItemInfo> cartInfos = null)
        {
            if(cartInfos == null)
            {
                cartInfos = _context.Session.Get<List<ItemInfo>>(_sessionKeyCartName);
;               
                if(cartInfos == null)
                {
                    cartInfos= new List<ItemInfo>();
                }
            }

            cartInfos.Add(item);

            _context.Session.Set<List<ItemInfo>>(_sessionKeyCartName, cartInfos);
        }

        private List<ItemInfo> GetCartInfos(ref string message)
        {
            var cartInfos = _context.Session.Get<List<ItemInfo>>(key: _sessionKeyCartName);

            if (cartInfos == null || cartInfos.Count() < 1)
            {
                message = "장바구니에 담긴 상품이 없습니다.";
            }

            return cartInfos;
        }
        #endregion

        #region AES
        [HttpGet]
        [Authorize(Roles = "SuperUser, SystemUser")]
        public IActionResult AES()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "SuperUser, SystemUser")]
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
        #endregion

        #region Hash
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
        #endregion

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddCart()
        {
            SetCartInfos(new ItemInfo() { ItemNo = Guid.NewGuid(), ItemName = DateTime.UtcNow.Ticks.ToString() });
            return RedirectToAction("Cart", "Data");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult removeCart()
        {
            string message = string.Empty;
            var cartInfo = GetCartInfos(ref message);

            if (cartInfo != null && cartInfo.Count() > 0)
            {
                _context.Session.Remove(key: _sessionKeyCartName);
            }

            return RedirectToAction("Cart", "Data");
        }

        public IActionResult Cart()
        {
            string message = string.Empty;
            var cartInfos = GetCartInfos(ref message);

            ViewData["Message"] = message;
            return View(cartInfos);
        }
    }
}