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
    public class OrderDetailsRepository : Repository<OrderDetails>, IOrderDetailsRepository
    {
        private readonly ApplicationDbContext _dbContext;
        
        public OrderDetailsRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public void Update(OrderDetails orderDetails)
        {
            //OrderDetails orderDetaislFromDB = _dbContext.OrderDetails.FirstOrDefault(u=>u.Id == orderDetails.Id);
            //if(orderDetaislFromDB != null)
            //{
            //    orderDetaislFromDB.Name = orderDetails.Name;
            //    orderDetaislFromDB.OrderId = orderDetails.OrderId;
            //    orderDetaislFromDB.MenuItemId = orderDetails.MenuItemId;
            //    orderDetaislFromDB.Count = orderDetails.Count;
            //    orderDetaislFromDB.Price = orderDetails.Price;
            //}
            _dbContext.OrderDetails.Update(orderDetails);
        }
    }
}
