using Abby.DataAccess.Data;
using Abby.DataAccess.Repository.IRepository;
using Abby.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection.Metadata.Ecma335;

namespace AbbyWeb.Pages.Admin.Roles
{
    [BindProperties]
    public class EditModel : PageModel
    {

        private readonly RoleManager<IdentityRole> _roleManager;
        private UserManager<IdentityUser> _userManager;

        public IdentityRole Role { get; set; }
        public List<ApplicationUser> RoleMembers { get; set; }
        public List<ApplicationUser> NonMembers { get; set; }

        public EditModel(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async void OnGet(string roleId)
        {
            Role = await _roleManager.FindByIdAsync(roleId);
            var users = _userManager.Users;
            //RoleMembers = new List<ApplicationUser>();
            //NonMembers = new List<ApplicationUser>();
            foreach (ApplicationUser user in users)
            {
                var list = await _userManager.IsInRoleAsync(user, Role.Name) ? RoleMembers : NonMembers;
                list.Add(user);
            }

        }

        public async Task<IActionResult> OnPost()
        {
        //    if (Category.Name == Category.DisplayOrder.ToString())
        //    {
        //        ModelState.AddModelError(Category.Name, "Name and Display Order cannot be same");
        //    }
        //    if(ModelState.IsValid) 
        //    {
        //        _dbUnitOfWork.Category.Update(Category);
        //        _dbUnitOfWork.Save();
        //        TempData["success"] = "Category updated successfully";
        //        return RedirectToPage("Index");
        //    }
            return Page();
        }
    }
}
