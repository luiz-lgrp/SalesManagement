using Microsoft.EntityFrameworkCore;

using TestingCRUD.Domain.Models;
using TestingCRUD.Domain.Repositories;

namespace TestingCRUD.Infra.Repositories;
public class ProductRepository : IProductRepository
{
    private readonly Context _context;

    public ProductRepository(Context context) => _context = context;

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<Product> CreateAsync(Product product, CancellationToken cancellationToken)
    {
        await _context.Products.AddAsync(product, cancellationToken);
        await _context.SaveChangesAsync();
        
        return product;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var productDelete = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        
        if (productDelete is null)
            return false;

        _context.Products.Remove(productDelete);

        await _context.SaveChangesAsync();

        return true;
    }

}
