using Abby.DataAccess.Data;
using Abby.DataAccess.Repository.IRepository;
using Abby.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection.Metadata.Ecma335;

namespace AbbyWeb.Pages.Admin.Categories
{
    [BindProperties]
    public class EditModel : PageModel
    {

        private readonly IUnitOfWork _dbUnitOfWork;
       
        public Category Category { get; set; }

        public EditModel(IUnitOfWork dbUnitOfWork)
        {
            _dbUnitOfWork = dbUnitOfWork;
        }
        public void OnGet(int categoryId)
        {
            Category = _dbUnitOfWork.Category.GetFirstOrDefault(u => u.Id == categoryId);
        }

        public async Task<IActionResult> OnPost()
        {
            if (Category.Name == Category.DisplayOrder.ToString())
            {
                ModelState.AddModelError(Category.Name, "Name and Display Order cannot be same");
            }
            if(ModelState.IsValid) 
            {
                _dbUnitOfWork.Category.Update(Category);
                _dbUnitOfWork.Save();
                TempData["success"] = "Category updated successfully";
                return RedirectToPage("Index");
            }
            return Page();
        }
    }
}
