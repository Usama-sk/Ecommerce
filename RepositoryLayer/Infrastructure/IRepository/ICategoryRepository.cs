using DataModels;

namespace RepositoryLayer.Infrastructure.IRepository
{
    public interface ICartRepository : IRepositroy<Cart>
    {
        void Update(Cart cart);
    }
}
