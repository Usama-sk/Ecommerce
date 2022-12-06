
using DataModels;
using DataServiceLayer.DataContext;
using RepositoryLayer.Infrastructure.IRepository;

namespace RepositoryLayer.Infrastructure.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly AppDbContext _context;
        public CategoryRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Category category)
        {
            var categoryDB = _context.Categories.FirstOrDefault(x => x.CategoryId == category.CategoryId);
            if (categoryDB != null)
            {
                categoryDB.Name = category.Name;
                categoryDB.Description = category.Description;
            }

        }
    }
}
