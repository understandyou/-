using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using webmvc.Models;

namespace webmvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        [Authorize(policy: "claim")]//policy为startup中自定义的授权策略
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> testUrlRec(string biaoshi,string ReturnUrl)
        {
            //UserManager<IdentityUser>
            //SignInManager<IdentityUser>



            if (biaoshi == "1")
            {
                //存储登录信息的对象
                var info = new List<Claim>()
                {
                    //claim的参数为key,value
                    new Claim(ClaimTypes.Name,"thisValue1"),
                    new Claim(ClaimTypes.Email,"abc@qq.com")
                };
                //创建一个身份(类似于许可证,系统标识)
                var ident = new ClaimsIdentity(info,"Cookies");
                //此对象用于做多因子身份验证，
                //一个用户有多个info信息,,,通常来说用户都是一个身份
                var principal = new ClaimsPrincipal(ident);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return Redirect(ReturnUrl);
            }
            return Json(new {Name ="hello" });
        }
    }
}
