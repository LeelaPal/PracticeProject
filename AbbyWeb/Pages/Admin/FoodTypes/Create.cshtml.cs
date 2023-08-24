using Abby.DataAccess.Data;
using Abby.DataAccess.Repository.IRepository;
using Abby.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection.Metadata.Ecma335;

namespace AbbyWeb.Pages.Admin.FoodTypes
{
    [BindProperties]
    public class CreateModel : PageModel
    {

        private readonly IUnitOfWork _dbUnitOfWork;
       
        public FoodType FoodType { get; set; }

        public CreateModel(IUnitOfWork dbUnitOfWork)
        {
            _dbUnitOfWork = dbUnitOfWork;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
           
            if(ModelState.IsValid) 
            {
                _dbUnitOfWork.FoodType.Add(FoodType);
                _dbUnitOfWork.Save();
                TempData["success"] = "Food Type created successfully";
                return RedirectToPage("Index");
            }
            return Page();
        }
    }
}
