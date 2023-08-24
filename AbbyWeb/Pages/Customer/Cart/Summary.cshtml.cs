using Abby.DataAccess.Repository.IRepository;
using Abby.Models;
using Abby.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stripe.Checkout;
using System.Security.Claims;

namespace AbbyWeb.Pages.Customer.Cart
{
    [Authorize]
    [BindProperties]
    public class SummaryModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public OrderHeader OrderHeader { get; set; }
        public IEnumerable<ShoppingCart> ShoppingCartList { get; set; }

        public SummaryModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            OrderHeader = new OrderHeader();
        }
        
        public void OnGet()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if( claim != null )
            {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(filter: u => u.ApplicationUserId == claim.Value,
                    includeProperties: "MenuItem,MenuItem.Category,MenuItem.FoodType");
                foreach( var cartItem in ShoppingCartList )
                {
                    OrderHeader.OrderTotal += (cartItem.Count * cartItem.MenuItem.Price);
                }
                OrderHeader.OrderTotal = Math.Round(OrderHeader.OrderTotal, 2);
                ApplicationUser loggedInUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == claim.Value);
                OrderHeader.PickUpName = loggedInUser.FirstName + " " + loggedInUser.LastName;
                OrderHeader.PickUpPhoneNumber = loggedInUser.PhoneNumber;
            }
            

        }

		public IActionResult OnPost()
		{
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
			if (claim != null)
			{
				ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(filter: u => u.ApplicationUserId == claim.Value,
					includeProperties: "MenuItem,MenuItem.Category,MenuItem.FoodType");
				foreach (var cartItem in ShoppingCartList)
				{
					OrderHeader.OrderTotal += (cartItem.Count * cartItem.MenuItem.Price);
				}
				OrderHeader.OrderTotal = Math.Round(OrderHeader.OrderTotal, 2);
                OrderHeader.Status = SD.StatusPending;
                OrderHeader.OrderCreatedDate = DateTime.Now;
                OrderHeader.UserId = claim.Value;
                OrderHeader.PickupTime = Convert.ToDateTime(OrderHeader.PickupDate.ToShortDateString() + " " + OrderHeader.PickupTime.ToShortTimeString());
                _unitOfWork.OrderHeader.Add(OrderHeader);
                _unitOfWork.Save();

                foreach(var item in ShoppingCartList)
                {
                    OrderDetails orderDetails = new()
                        {
                            MenuItemId = item.MenuItemId,
                            OrderId = OrderHeader.Id,
                            Name = item.MenuItem.Name,
                            Price = item.MenuItem.Price,
                            Count = item.Count
                        };
                    _unitOfWork.OrderDetails.Add(orderDetails);
                }

                int quantity = ShoppingCartList.ToList().Count;
                _unitOfWork.ShoppingCart.RemoveRange(ShoppingCartList);
                _unitOfWork.Save();

				var domain = "http://localhost:4242";
                var options = new SessionCreateOptions
                {
                    LineItems = new List<SessionLineItemOptions>
                {
                  new SessionLineItemOptions
                  {
                      PriceData = new SessionLineItemPriceDataOptions
                      {
                          UnitAmount = (long)(OrderHeader.OrderTotal*100),
                          Currency = "USD",
                          ProductData = new SessionLineItemPriceDataProductDataOptions
                          {
                              Name = "Abby Food Order",
                              Description = "Total Distinct Item " + quantity
                          },
                      },
                    Quantity = 1
				  },
				},
                    
                    PaymentMethodTypes= new List<string>
                    {
                        "card"
                    },
					Mode = "payment",
					SuccessUrl = domain + $"Customer/cart/OrderConfirmation?id={OrderHeader.Id}",
					CancelUrl = domain + "customer/cart/index",
				};
				var service = new SessionService();
				Session session = service.Create(options);

				Response.Headers.Add("Location", session.Url);
				return new StatusCodeResult(303);

			}

            return Page();
		}
	}
}
