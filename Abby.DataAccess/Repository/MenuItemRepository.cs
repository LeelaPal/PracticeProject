using Abby.DataAccess.Data;
using Abby.DataAccess.Repository.IRepository;
using Abby.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abby.DataAccess.Repository
{
    public class MenuItemRepository:Repository<MenuItem>, IMenuItemRepository
    {
        private readonly ApplicationDbContext _db;

        public MenuItemRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
            
        }

        public void Update(MenuItem menuItem)
        {
            var menuItemFromDB = _db.MenuItem.FirstOrDefault(u => u.Id == menuItem.Id);
            if (menuItemFromDB != null)
            {
                menuItemFromDB.Name = menuItem.Name;
                menuItemFromDB.Description = menuItem.Description;
                menuItemFromDB.Price = menuItem.Price;
                if (menuItemFromDB.ImageURL != null)
                { 
                    menuItemFromDB.ImageURL = menuItem.ImageURL; 
                }
                menuItemFromDB.CategoryId = menuItem.CategoryId;
                menuItemFromDB.FoodTypeId = menuItem.FoodTypeId;
            }
        }
    }
}
