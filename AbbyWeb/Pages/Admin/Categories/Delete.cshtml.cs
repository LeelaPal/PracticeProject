using Abby.DataAccess.Data;
using Abby.DataAccess.Repository.IRepository;
using Abby.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection.Metadata.Ecma335;

namespace AbbyWeb.Pages.Admin.Categories
{
    [BindProperties]
    public class DeleteModel : PageModel
    {

        private readonly IUnitOfWork _dbUnitOfWork;
       
        public Category Category { get; set; }

        public DeleteModel(IUnitOfWork dbUnitOfWork)
        {
            _dbUnitOfWork = dbUnitOfWork;
        }
        public void OnGet(int categoryId)
        {
            Category = _dbUnitOfWork.Category.GetFirstOrDefault(u => u.Id == categoryId);
        }

        public async Task<IActionResult> OnPost()
        {
            if(Category != null) 
            {
                _dbUnitOfWork.Category.Remove(Category);
                _dbUnitOfWork.Save();
                TempData["success"] = "Category deleted successfully";
                return RedirectToPage("Index");
            }
            return Page();
        }
    }
}
