using DataServiceLayer.DataContext;
using RepositoryLayer.Infrastructure.IRepository;

namespace RepositoryLayer.Infrastructure.Repository
{
    public class UnitofWork : IUnitofWork
    {

        private AppDbContext _context;
        public ICategoryRepository Category { get; private set; }
        public IProductRepository Product { get; private set; }

        public UnitofWork(AppDbContext context)
        {
            _context = context;
            Category = new CategoryRepository(context);
            Product = new ProductRepository(context);
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
