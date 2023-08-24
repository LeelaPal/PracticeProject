using Abby.DataAccess.Data;
using Abby.DataAccess.Repository.IRepository;
using Abby.Models;


namespace Abby.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }

        public void Update(Category category)
        {
            var categoryFromDB = _db.Category.FirstOrDefault(u=>u.Id == category.Id);
            if (categoryFromDB != null)
            {
                categoryFromDB.Name = category.Name;
                categoryFromDB.DisplayOrder = category.DisplayOrder;
            }
        }
    }
}
