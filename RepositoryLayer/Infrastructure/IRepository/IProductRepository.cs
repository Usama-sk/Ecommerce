using DataModels;

namespace RepositoryLayer.Infrastructure.IRepository
{
    public interface IProductRepository : IRepositroy<Product>
    {
        void Update(Product product);
    }
}
