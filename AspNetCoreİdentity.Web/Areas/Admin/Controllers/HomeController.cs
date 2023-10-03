using AspNetCoreİdentity.Web.Areas.Admin.Models;
using AspNetCoreİdentity.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreİdentity.Web.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class HomeController : Controller
    {

        private readonly UserManager<UygulamaUser> _userManager;

        public HomeController(UserManager<UygulamaUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> UserList()
        {

            var userlList = await  _userManager.Users.ToListAsync();

            var userListViewModel = userlList.Select(x => new UserViewModel()
            {
                UserId = x.Id,
                UserEmail=x.Email,
                UserName=x.UserName
            }).ToList();


            return View(userListViewModel);
        }
    }
}
