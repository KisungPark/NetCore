using Microsoft.AspNetCore.Mvc;
using Net.Data.ViewModels;
using System.Diagnostics;

namespace Net.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _logger.LogDebug(1, "NLog injected into HomeController");
        }

        public IActionResult Index()
        {
            _logger.LogInformation("홈 컨트롤러 실행");
            _logger.LogTrace("추적");
            _logger.LogDebug("디버그");
            _logger.LogInformation("정보");
            _logger.LogWarning("경고");
            _logger.LogError("에러");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}