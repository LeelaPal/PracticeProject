using Abby.DataAccess.Repository.IRepository;
using Abby.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AbbyWeb.Pages.Admin.MenuItems
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _dbUnitOfWork;

        public IEnumerable<MenuItem> MenuItems { get; set; }

        public IndexModel(IUnitOfWork dbUnitOfWork)
        {
            _dbUnitOfWork = dbUnitOfWork;
        }

        public void OnGet()
        {
            MenuItems = _dbUnitOfWork.MenuItem.GetAll();
        }
    }
}
