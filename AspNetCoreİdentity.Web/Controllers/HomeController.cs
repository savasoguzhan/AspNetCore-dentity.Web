using AspNetCoreİdentity.Web.Extension;
using AspNetCoreİdentity.Repository.Models;
using AspNetCoreİdentity.Web.Services;
using AspNetCoreİdentity.Core.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using AspNetCoreİdentity.Core.Models;
using AspNetCoreİdentity.Repository.Models;
using AspNetCoreİdentity.Service.Services;

namespace AspNetCoreİdentity.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;


        private readonly UserManager<UygulamaUser> _userManager;

        private readonly SignInManager<UygulamaUser> _signInManager;

        private readonly IEmailService _emailService;

        public HomeController(ILogger<HomeController> logger, UserManager<UygulamaUser> userManager, SignInManager<UygulamaUser> signInManager, IEmailService emailService)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            var userClaims=User.Claims.ToList();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult SignUp()
        {


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel request)
        {



            if (!ModelState.IsValid)
            {
                return View();
            }

            var identityResult = await _userManager.CreateAsync(new() { UserName = request.UserName, PhoneNumber = request.Phone, Email = request.Email }, request.Password);

            if (identityResult.Succeeded)
            {

                var exchangeClaim = new Claim("ExchangeExpireDate",DateTime.Now.AddDays(10).ToString());

                var user = await _userManager.FindByNameAsync(request.UserName);
                await _userManager.AddClaimAsync(user, exchangeClaim);




                TempData["SuccessMessage"] = "Uyelik Kayit Islemi Basarili Gerceklestirilmistir ";
                return RedirectToAction(nameof(HomeController.SignUp));
            }

            ModelState.AddModelErrorList(identityResult.Errors.Select(x =>x.Description).ToList());

            


            return View();
        }


        public IActionResult SignIn()
        {
            return View();
        }


        [HttpPost]
        public  async Task<IActionResult> SignIn(SignInViewModel model, string? returnUrl=null)
        {

            
           returnUrl= returnUrl ?? Url.Action("Index", "Home");

            var isUser= await _userManager.FindByEmailAsync(model.Email);

            if(isUser == null)
            {
                ModelState.AddModelError(string.Empty, "Email Veya Sifre Yanlis");
                return View();
            }

           

           

            var result = await _signInManager.PasswordSignInAsync(isUser, model.Password, model.RememberMe,true);

            if(result.Succeeded)
            {
                if (isUser.BirthDay.HasValue)
                {
                    await _signInManager.SignInWithClaimsAsync(isUser, model.RememberMe, new[] { new Claim("birthdate",isUser.BirthDay.Value.ToString()) });
                }

               
                return RedirectToAction("Index","Member");
            }

            if(result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Has Been Locked Out for 3 Min ");
            }

           
            return View();
        }


        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel request)
        {
            var hasUser =  await _userManager.FindByEmailAsync(request.Email);

            if(hasUser == null)
            {
                   // model state hata mesaji ekleme 
                ModelState.AddModelError(String.Empty, "Bu Email adresine sahip kullanici yoktur");
                return View();
            }

            string passwordresetToken = await  _userManager.GeneratePasswordResetTokenAsync(hasUser);

            var passwordResetLink=Url.Action("ResetPassword","Home", new {userId=hasUser.Id, Token=passwordresetToken});

            await _emailService.SendResetPasswordEmail(passwordResetLink,hasUser.Email);

            TempData["success"] = "Sifre Yenileme Linkin E-Posta Adresinize Gonderilmistir";
            return RedirectToAction(nameof(ForgetPassword));
        }

































        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}