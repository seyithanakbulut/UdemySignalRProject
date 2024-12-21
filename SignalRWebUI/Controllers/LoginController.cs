using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignalR.EntityLayer.Entities;
using SignalRWebUI.Dtos.IdentityDtos;

namespace SignalRWebUI.Controllers
{
    [AllowAnonymous] // Proje seviyesindeki tüm kurallardan muaf
    public class LoginController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;

        public LoginController(SignInManager<AppUser> signInManager)
        {
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task< IActionResult> Index(LoginDto loginDto)
        {
            // ilk false beni hattırla diğeri ban  belli dk
            var result = await _signInManager.PasswordSignInAsync(loginDto.Username, loginDto.Password,false,true);  
            if (result.Succeeded) 
            {
                return RedirectToAction("Index", "Category");
            }

            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Login");
        }
    }
}
