using Abby.DataAccess.Data;
using Abby.DataAccess.Repository.IRepository;
using Abby.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection.Metadata.Ecma335;

namespace AbbyWeb.Pages.Admin.FoodTypes
{
    [BindProperties]
    public class DeleteModel : PageModel
    {

        private readonly IUnitOfWork _dbUnitOfWork;
       
        public FoodType FoodType { get; set; }

        public DeleteModel(IUnitOfWork dbUnitOfWork)
        {
            _dbUnitOfWork = dbUnitOfWork;
        }
        public void OnGet(int foodTypeId)
        {
            FoodType = _dbUnitOfWork.FoodType.GetFirstOrDefault(u => u.Id == foodTypeId);
        }

        public async Task<IActionResult> OnPost()
        {
            if(FoodType != null) 
            {
                _dbUnitOfWork.FoodType.Remove(FoodType);
                _dbUnitOfWork.Save();
                TempData["success"] = "Food Type deleted successfully";
                return RedirectToPage("Index");
            }
            return Page();
        }
    }
}
