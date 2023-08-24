using Abby.DataAccess.Data;
using Abby.DataAccess.Repository.IRepository;
using Abby.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace AbbyWeb.Pages.Admin.Roles
{
    [BindProperties]
    public class CreateModel : PageModel
    {

        private readonly RoleManager<IdentityRole> _roleManager;

        public IdentityRole Role { get; set; }

        public CreateModel(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if(ModelState.IsValid) 
            {
                if (!await _roleManager.RoleExistsAsync(Role.Name))
                {
                    await _roleManager.CreateAsync(new IdentityRole(Role.Name));
                    TempData["success"] = "Role created successfully";
                    return RedirectToPage("Index");
                }
                else
                {
                    ModelState.AddModelError(Role.Name, "Role Name already exists");
                }
            }
            return Page();
        }
    }
}
