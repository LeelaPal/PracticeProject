using Abby.DataAccess.Repository;
using Abby.DataAccess.Repository.IRepository;
using Abby.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace AbbyWeb.Pages.Customer.Cart
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _dbUnitOfWork;

        public IEnumerable<ShoppingCart> ShoppingCartList { get; set; }

        public double CartTotal { get; set;}

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _dbUnitOfWork = unitOfWork;
            CartTotal = 0;
        }

        public void OnGet()
        {
            ClaimsIdentity claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if(claims != null) 
            {
                ShoppingCartList = _dbUnitOfWork.ShoppingCart.GetAll(filter: u => u.ApplicationUserId == claims.Value,
                    includeProperties:"MenuItem,MenuItem.FoodType,MenuItem.Category");
                foreach(var cartItem in ShoppingCartList)
                {
                    CartTotal += cartItem.MenuItem.Price * cartItem.Count;
                }

                CartTotal = Math.Round(CartTotal, 2);
            }

        }

        public IActionResult OnPostPlus(int cartId)
        {
            ShoppingCart shoppingCartFromDB = _dbUnitOfWork.ShoppingCart.GetFirstOrDefault(u=>u.Id == cartId);
            _dbUnitOfWork.ShoppingCart.IncrementCount(shoppingCartFromDB, 1);
            return RedirectToPage("Index");
        }

        public IActionResult OnPostMinus(int cartId)
        {
            ShoppingCart shoppingCartFromDB = _dbUnitOfWork.ShoppingCart.GetFirstOrDefault(u=>u.Id==cartId);
            if(shoppingCartFromDB.Count == 1)
            {
                _dbUnitOfWork.ShoppingCart.Remove(shoppingCartFromDB);
                _dbUnitOfWork.Save();
            }
            else
            {
                _dbUnitOfWork.ShoppingCart.DecrementCount(shoppingCartFromDB, 1);
            }
            return RedirectToPage("Index");
        } 

        public IActionResult OnPostRemove(int cartId)
        {
            ShoppingCart shoppingCartFromDb = _dbUnitOfWork.ShoppingCart.GetFirstOrDefault(u=>u.Id==cartId);
            _dbUnitOfWork.ShoppingCart.Remove(shoppingCartFromDb);
            _dbUnitOfWork.Save();
            return RedirectToPage("Index");
        }
    }
}
