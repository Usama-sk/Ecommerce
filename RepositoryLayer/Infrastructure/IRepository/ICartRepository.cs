using DataModels;

namespace RepositoryLayer.Infrastructure.IRepository
{
    public interface ICartRepository : IRepositroy<Cart>
    {
        int IncrementCartItem(Cart cart, int count);
    }
}
