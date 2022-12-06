using DataModels;
using DataServiceLayer.DataContext;
using RepositoryLayer.Infrastructure.IRepository;

namespace RepositoryLayer.Infrastructure.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }


        public void Update(Product product)
        {
            var productDB = _context.Products.FirstOrDefault(x => x.ProductId == product.ProductId);
            if (productDB != null)
            {
                productDB.Name = product.Name;
                productDB.Description = product.Description;
                productDB.Price = product.Price;
                if (product.ImageUrl != null)
                {
                    productDB.ImageUrl = product.ImageUrl;
                }
            }

        }
    }
}
