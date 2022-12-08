
using DataModels;
using DataServiceLayer.DataContext;
using RepositoryLayer.Infrastructure.IRepository;

namespace RepositoryLayer.Infrastructure.Repository
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        private readonly AppDbContext _context;
        public CartRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }


    }
}
