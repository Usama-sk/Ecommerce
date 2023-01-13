using DataServiceLayer.DataContext;
using RepositoryLayer.Infrastructure.IRepository;

namespace RepositoryLayer.Infrastructure.Repository
{
    public class UnitofWork : IUnitofWork
    {

        private AppDbContext _context;
        public ICategoryRepository Category { get; private set; }
        public IProductRepository Product { get; private set; }
        public ICartRepository Cart { get; private set; }
        public IAppUserRepository AppUser { get; private set; }

        public IOrderHeaderRepository OrderHeader { get; private set; }

        public IOrderDetailRepository OrderDetail { get; private set; }

        public UnitofWork(AppDbContext context)
        {
            _context = context;
            Category = new CategoryRepository(context);
            Cart = new CartRepository(context);
            AppUser = new AppUserRepository(context);
            Product = new ProductRepository(context);
            OrderHeader = new OrderHeaderRepository(context);
            OrderDetail = new OrderDetailRepository(context);
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
