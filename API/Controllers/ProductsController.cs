
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IGenericRepository<Product> _prodactsRepo;
    private readonly IGenericRepository<ProductBrand> _productBrandRepo;
    private readonly IGenericRepository<ProductType> _productTypeRepo;
    public ProductsController(
     IGenericRepository<Product> prodactsRepo,
     IGenericRepository<ProductBrand> productBrandRepo,
     IGenericRepository<ProductType> productTypeRepo
     )
    {
        _productBrandRepo = productBrandRepo;
        _productTypeRepo = productTypeRepo;
        _prodactsRepo = prodactsRepo;
    }


    [HttpGet]
    public async Task<ActionResult<List<Product>>> GetProducts()
    {
        var products = await _prodactsRepo.ListAllAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        return await _prodactsRepo.GetByIdAsync(id);
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
    {
        return Ok(await _productBrandRepo.ListAllAsync());
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
    {
        return Ok(await _productTypeRepo.ListAllAsync());
    }

}
