using AspNetCoreİdentity.Web.Areas.Admin.Models;
using AspNetCoreİdentity.Web.Extension;
using AspNetCoreİdentity.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreİdentity.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : Controller
    {
        private readonly UserManager<UygulamaUser> _userManager;
        private readonly RoleManager<UygulamaRole> _roleManager;

        public RoleController(UserManager<UygulamaUser> userManager, RoleManager<UygulamaRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        


        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.Select(x => new RoleViewModel()
            {
                Id = x.Id,
                Name = x.Name!
            }).ToListAsync();

            return View(roles);
        }


        public IActionResult RoleAdd()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RoleAdd(RoleCreateViewModel request)
        {
            var result = await _roleManager.CreateAsync(new UygulamaRole() { Name = request.Name });

            if(!result.Succeeded)
            {
                ModelState.AddModelErrorList(result.Errors);
                return View();
            }



            return RedirectToAction(nameof(RoleController.Index));
        }


        public async  Task<IActionResult> RoleUpdate(string id)
        {

            var roleToUpdate = await _roleManager.FindByIdAsync(id);

            if(roleToUpdate == null)
            {
                throw new Exception("Guncellenicek Rol Bulanamamistir");
            }

            var updateRole = new RoleUpdateViewModel()
            {
                Id=roleToUpdate.Id,
                Name=roleToUpdate.Name
            };
            return View(updateRole );
        }

        [HttpPost]
        public async Task<IActionResult> RoleUpdate(RoleUpdateViewModel request)
        {
            var roleToUpdate = await _roleManager.FindByIdAsync(request.Id);


            if(roleToUpdate == null)
            {
                throw new Exception("Guncellenicek Rol Bulanamamistir");
            }
            roleToUpdate.Name= request.Name;

            await _roleManager.UpdateAsync(roleToUpdate);

            return View();
        }


        public async Task<IActionResult> RoleDelete(string id)
        {
            var roleToDelete = await _roleManager.FindByIdAsync(id);

            if(roleToDelete == null)
            {
                throw new Exception("silinecek Rol bulanamamistir");
            }
            var result = await _roleManager.DeleteAsync(roleToDelete);

            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.Select(x => x.Description).First());

            }

            return RedirectToAction(nameof(RoleController.RoleDelete));


        }


        public async Task<IActionResult> AssingRoleToUser(string id)
        {
            var currentUser = await _userManager.FindByIdAsync(id);
            var roles = await _roleManager.Roles.ToListAsync();
            ViewBag.userId = id;

            var roleViewModelList = new List<AssignRoleToUserViewModel>();
            var userRoles = await _userManager.GetRolesAsync(currentUser);
            foreach (var role in roles)
            {
                var assignRoleToUserViewModel = new AssignRoleToUserViewModel()
                {
                    Id = role.Id,
                    Name = role.Name!
                };

                if(userRoles.Contains(role.Name))
                {
                    assignRoleToUserViewModel.Exist = true;
                }


                roleViewModelList.Add(assignRoleToUserViewModel);

            }

            return View(roleViewModelList);
        }


        [HttpPost]
        public async Task<IActionResult> AssingRoleToUser(string UserId,List<AssignRoleToUserViewModel> requestList)
        {

            var userToAssignRole = await _userManager.FindByIdAsync(UserId);

            foreach (var role in requestList)
            {
                if(role.Exist)
                {
                     await _userManager.AddToRoleAsync(userToAssignRole,role.Name);
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(userToAssignRole,role.Name);
                }
            }




            return RedirectToAction(nameof(HomeController.UserList));
        }
    }
}
