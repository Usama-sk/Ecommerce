
using DataModels;
using DataServiceLayer.DataContext;
using RepositoryLayer.Infrastructure.IRepository;

namespace RepositoryLayer.Infrastructure.Repository
{
    public class AppUserRepository : Repository<AppUser>, IAppUserRepository
    {
        private readonly AppDbContext _context;
        public AppUserRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }


    }
}
