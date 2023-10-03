using AspNetCoreİdentity.Web.Extension;
using AspNetCoreİdentity.Web.Models;
using AspNetCoreİdentity.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;

namespace AspNetCoreİdentity.Web.Controllers
{


    [Authorize]
    public class MemberController : Controller
    {

        private readonly SignInManager<UygulamaUser> _signInManager;
        private readonly UserManager<UygulamaUser> _userManager;
        private readonly IFileProvider _fileProvider;
        public MemberController(SignInManager<UygulamaUser> signInManager, UserManager<UygulamaUser> userManager, IFileProvider fileProvider)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _fileProvider = fileProvider;
        }



        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name!);

            var userViewModel = new UserViewModel 
            {Email=currentUser!.Email,
             Phone=currentUser.PhoneNumber,
            UserName=currentUser.UserName,
            PictureUrl=currentUser.Picture
            };

            return View(userViewModel);
        }

        public async Task<IActionResult> PasswordChange()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PasswordChange(PasswordChangeViewModel request)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name!);

            var checkOldPassword = await _userManager.CheckPasswordAsync(currentUser, request.PasswordOld);

            if(!checkOldPassword)
            {
                ModelState.AddModelError(string.Empty, "Eski sifreniz yanlis");
            }

            var result = await _userManager.ChangePasswordAsync(currentUser, request.PasswordOld,request.PasswordNew);


            await _userManager.UpdateSecurityStampAsync(currentUser);
            await _signInManager.SignOutAsync();
            await _signInManager.PasswordSignInAsync(currentUser, request.PasswordNew, true, false);

            TempData["SuccessMessage"] = "Sifreniz Basari ile Degistirilmistir";

            return View();
        }




        public async  Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }


        public async Task<IActionResult> UserEdit()
        {
            ViewBag.gender= new SelectList(Enum.GetNames(typeof(Gender)));

            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
            var userEditViewModel = new UserEditViewModel()
            {
                UserName = currentUser.UserName,
                Email = currentUser.Email,
                Phone = currentUser.PhoneNumber,
                BirthDay = currentUser.BirthDay,
                City = currentUser.City,



            };
            return View(userEditViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UserEdit(UserEditViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name!);
            currentUser.UserName = request.UserName;
            currentUser.Email = request.Email;
            currentUser.BirthDay = request.BirthDay;
            currentUser.City = request.City;
            currentUser.Gender = request.Gender;
            currentUser.PhoneNumber = request.Phone;




            if (request.Picture != null && request.Picture.Length > 0)
            {
                var wwwrootFolder = _fileProvider.GetDirectoryContents("wwwroot");
                var randomFileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(request.Picture.FileName)}";

                var newPicturePath = Path.Combine(wwwrootFolder!.First(x => x.Name == "userpictures").PhysicalPath!, randomFileName);

                using var stream = new FileStream(newPicturePath, FileMode.Create);
                await request.Picture.CopyToAsync(stream);
                currentUser.Picture = randomFileName;

            }

            var updateToUserResult = await _userManager.UpdateAsync(currentUser);

            if (!updateToUserResult.Succeeded)
            {
                ModelState.AddModelErrorList(updateToUserResult.Errors);
                return View();
            }

            await _userManager.UpdateSecurityStampAsync(currentUser);

            await _signInManager.SignOutAsync();
            await _signInManager.SignInAsync(currentUser, true);


            TempData["SuccessMessage"] = "Uye Bilgileri Basariyla Degistirilmistir";
            var userEditViewModel = new UserEditViewModel()
            {
                UserName = currentUser.UserName,
                Email = currentUser.Email,
                Phone = currentUser.PhoneNumber,
                BirthDay = currentUser.BirthDay,
                City = currentUser.City,



            };
            return View(userEditViewModel);
            

        }

        [HttpGet]
        public async  Task<IActionResult> Claims()
        {

            var userClaimList = User.Claims.Select(x => new ClaimViewModel()
            {
                Issuer = x.Issuer,
                Type = x.Type,
                Value = x.Value,
            }).ToList();

            return View(userClaimList);
        }


        [Authorize(Policy ="AnkaraPolicy")]
        [HttpGet]
        public IActionResult Ankara()
        {
            return View();
        }


        [Authorize(Policy = "ExchangePolicy")]
        [HttpGet]
        public IActionResult Exchange()
        {
            return View();
        }
    }
}
