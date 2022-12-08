namespace RepositoryLayer.Infrastructure.IRepository
{
    public interface IUnitofWork
    {
        ICategoryRepository Category { get; }
        IProductRepository Product { get; }
        ICartRepository Cart { get; }
        IAppUserRepository AppUser { get; }
        void Save();
    }
}
