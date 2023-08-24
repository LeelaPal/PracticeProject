using Abby.DataAccess.Repository.IRepository;
using Abby.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AbbyWeb.Pages.Customer.Home
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _dbUnitOfWork;

        public IEnumerable<MenuItem> MenuItemList { get; set; }
        public IEnumerable<Category> CategoryList { get; set; }

        public IndexModel(IUnitOfWork dbUnitOfWork)
        {
            _dbUnitOfWork = dbUnitOfWork;
        }

        public void OnGet()
        {
            MenuItemList = _dbUnitOfWork.MenuItem.GetAll(includeProperties: "Category,FoodType");
            CategoryList = _dbUnitOfWork.Category.GetAll(orderby:u=>u.OrderBy(c=>c.DisplayOrder));
        }
    }
}
