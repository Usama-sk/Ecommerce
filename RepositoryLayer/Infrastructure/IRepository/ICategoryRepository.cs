using DataModels;

namespace RepositoryLayer.Infrastructure.IRepository
{
    public interface ICategoryRepository : IRepositroy<Category>
    {
        void Update(Category category);
    }
}
