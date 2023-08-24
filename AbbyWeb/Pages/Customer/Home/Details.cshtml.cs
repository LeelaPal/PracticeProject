using Abby.DataAccess.Repository.IRepository;
using Abby.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NuGet.Protocol;
using System.Security.Claims;

namespace AbbyWeb.Pages.Customer.Home
{
    [Authorize]
    
    public class DetailsModel : PageModel
    {
        private readonly IUnitOfWork _dbUnitOfWork;
        [BindProperty]
        public ShoppingCart ShoppingCart { get; set; }

        public DetailsModel(IUnitOfWork dbUnitOfWork)
        {
            _dbUnitOfWork = dbUnitOfWork;
        }


        public void OnGet(int id)
        {
            ClaimsIdentity loggedInUser = (ClaimsIdentity)User.Identity;
            var loggedInUserClaim = loggedInUser.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCart = new()
            {
                ApplicationUserId = loggedInUserClaim.Value,
                MenuItem = _dbUnitOfWork.MenuItem.GetFirstOrDefault(u => u.Id == id, includeProperties: "Category,FoodType"),
                MenuItemId = id
               
            };
            ShoppingCart shoppingCartFromDB = _dbUnitOfWork.ShoppingCart.GetFirstOrDefault(
               filter: u => u.ApplicationUserId == ShoppingCart.ApplicationUserId &&
                u.MenuItemId == ShoppingCart.MenuItemId);
            if (shoppingCartFromDB != null)
            {
                ShoppingCart.Count = shoppingCartFromDB.Count;
            }
        }

        public IActionResult OnPost()
        {

            ShoppingCart shoppingCartFromDB = _dbUnitOfWork.ShoppingCart.GetFirstOrDefault(
                filter: u => u.ApplicationUserId == ShoppingCart.ApplicationUserId &&
                 u.MenuItemId == ShoppingCart.MenuItemId);

            if (ModelState.IsValid)
            {
                if (shoppingCartFromDB == null)
                {
                    _dbUnitOfWork.ShoppingCart.Add(ShoppingCart);
                    _dbUnitOfWork.Save();
                   
                }
                else
                {
                    _dbUnitOfWork.ShoppingCart.UpdateCount(shoppingCartFromDB, ShoppingCart.Count);
                    //if (ShoppingCart.Count > shoppingCartFromDB.Count)
                    //{
                    //    _dbUnitOfWork.ShoppingCart.IncrementCount(shoppingCartFromDB, ShoppingCart.Count);
                    //}
                    //else if (ShoppingCart.Count < shoppingCartFromDB.Count)
                    //{
                    //    _dbUnitOfWork.ShoppingCart.DecrementCount(shoppingCartFromDB, ShoppingCart.Count);
                    //}
                }
                return RedirectToPage("Index");
            }

            return Page();
        }
    }
}
