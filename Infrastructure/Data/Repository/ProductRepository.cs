using Core.Interfaces;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repository;

public class ProductRepository: IProductRepository
{
    private readonly StoreContext _context;
    public ProductRepository(StoreContext context)
    {
        _context = context;
    }
    public async Task<IReadOnlyList<Product>> GetProductListAsync()
    {
        return await _context.Products.ToListAsync();
    }
    public async Task<Product> GetProductByIdAsync(int id)
    {
        var product =  await _context.Products.FindAsync(id);
        
        return product!;   
    }
}
