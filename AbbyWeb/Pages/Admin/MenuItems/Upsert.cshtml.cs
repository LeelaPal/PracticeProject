using Abby.DataAccess.Data;
using Abby.DataAccess.Repository.IRepository;
using Abby.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection.Metadata.Ecma335;

namespace AbbyWeb.Pages.Admin.MenuItems
{
    [BindProperties]
    public class UpsertModel : PageModel
    {

        private readonly IUnitOfWork _dbUnitOfWork;

        private readonly IWebHostEnvironment _webHostEnvironment;
       
        public MenuItem MenuItem { get; set; }

        public IEnumerable<SelectListItem> CategoryList { get; set; }

        public IEnumerable<SelectListItem> FoodTypeList { get; set; }

        public UpsertModel(IUnitOfWork dbUnitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _dbUnitOfWork = dbUnitOfWork;
            _webHostEnvironment = webHostEnvironment;
            MenuItem = new();
        }

        
        public void OnGet(int? id)
        {
            if (id != null)
            {
                MenuItem = _dbUnitOfWork.MenuItem.GetFirstOrDefault(x => x.Id == id);
            }
            CategoryList = _dbUnitOfWork.Category.GetAll().Select(i => new SelectListItem()
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });

            FoodTypeList = _dbUnitOfWork.FoodType.GetAll().Select(i => new SelectListItem()
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
        }

        public async Task<IActionResult> OnPost()
        {
            GetNewImageFilePath();
            if (MenuItem.Id == 0)
            {
                _dbUnitOfWork.MenuItem.Add(MenuItem);
                TempData["success"] = "Menu Item created successfully";
            }
            else
            {
                _dbUnitOfWork.MenuItem.Update(MenuItem);
                TempData["success"] = "Menu Item updated successfully";
            }
            _dbUnitOfWork.Save();
            return RedirectToPage("./Index");
           
        }

        private void GetNewImageFilePath()
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            var imageFiles = HttpContext.Request.Form.Files;
            if (imageFiles.Count > 0)
            {
                //generate a Guid for file name to be unique
                string newFileName = Guid.NewGuid().ToString();
                var uploadPath = Path.Combine(webRootPath, @"Images\MenuItems");
                var extension = Path.GetExtension(imageFiles[0].FileName);

                //Delete existing image from wwwroot/images/menuitem folder
                if(MenuItem.Id != 0) 
                {
                    var menuItemFromDB = _dbUnitOfWork.MenuItem.GetFirstOrDefault(u => u.Id == MenuItem.Id);
                    var existingImagePath = Path.Combine(webRootPath, menuItemFromDB.ImageURL.TrimStart('\\'));
                    if(System.IO.File.Exists(existingImagePath)) 
                    {
                        System.IO.File.Delete(existingImagePath);
                    }
                }

                //Add new image to wwwroot/images/menuitem folder
                using (var fileStream = new FileStream(Path.Combine(uploadPath, newFileName + extension), FileMode.Create))
                {
                    imageFiles[0].CopyTo(fileStream);
                }

                MenuItem.ImageURL = @"\images\menuItems\" + newFileName + extension;
            }
            else
            {
                if(MenuItem.Id != 0)
                {
                    var menuItemFromDB = _dbUnitOfWork.MenuItem.GetFirstOrDefault(u => u.Id == MenuItem.Id);
                    MenuItem.ImageURL = menuItemFromDB.ImageURL;
                }
            }
        }
    }
}
