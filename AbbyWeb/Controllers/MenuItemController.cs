using Abby.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace AbbyWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemController : Controller
    {
        IUnitOfWork _dbUnitOfWork;
        public IWebHostEnvironment _webHostEnvironment;

        public MenuItemController(IUnitOfWork dbUnitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _dbUnitOfWork = dbUnitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var menuItemsList = _dbUnitOfWork.MenuItem.GetAll(includeProperties: "Category,FoodType");

            return Json(new { data = menuItemsList });
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var menuItemFromDB = _dbUnitOfWork.MenuItem.GetFirstOrDefault(u => u.Id == id);
           
            _dbUnitOfWork.MenuItem.Remove(menuItemFromDB);
            _dbUnitOfWork.Save();
            var existingImagePath = Path.Combine(_webHostEnvironment.WebRootPath, menuItemFromDB.ImageURL.TrimStart('\\'));
            if (System.IO.File.Exists(existingImagePath))
            {
                System.IO.File.Delete(existingImagePath);
            }
            return Json(new {success = true, message = "Delete Successful" });
        }
    }
}
