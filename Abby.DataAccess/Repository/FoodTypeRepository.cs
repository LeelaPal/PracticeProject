using Abby.DataAccess.Data;
using Abby.DataAccess.Repository.IRepository;
using Abby.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abby.DataAccess.Repository
{
    public class FoodTypeRepository : Repository<FoodType>, IFoodTypeRepository
    {
        private readonly ApplicationDbContext _db;

        public FoodTypeRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }

        public void Update(FoodType foodType)
        {
            var categoryFromDB = _db.Category.FirstOrDefault(u=>u.Id == foodType.Id);
            if (categoryFromDB != null)
            {
                categoryFromDB.Name = foodType.Name;
                
            }
        }
    }
}
