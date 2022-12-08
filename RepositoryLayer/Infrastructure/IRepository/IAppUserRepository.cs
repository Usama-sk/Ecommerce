using DataModels;

namespace RepositoryLayer.Infrastructure.IRepository
{
    public interface IAppUserRepository : IRepositroy<AppUser>
    {
        void Update(AppUser appUser);
    }
}
