using Microsoft.EntityFrameworkCore;

using TestingCRUD.Domain.Models;
using TestingCRUD.Domain.Repositories;

namespace TestingCRUD.Infra.Repositories;
public class ProductReadRepository : IProductReadRepository
{
    private readonly Context _productContext;

    public ProductReadRepository(Context productContext) => _productContext = productContext;

    public async Task<IEnumerable<Product>> GetAll(CancellationToken cancellationToken)
    {
        var products = await _productContext.Products.ToListAsync();

        if (products.Any())
            return Enumerable.Empty<Product>();
        
        return products;
    }

    public async Task<Product?> GetById(Guid id, CancellationToken cancellationToken)
    {
        var product = await _productContext.Products.FirstOrDefaultAsync(p => p.Id == id);

        if (product is null)
            return null;
        
        return product;
    }
}
