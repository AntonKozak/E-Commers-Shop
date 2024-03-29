
using API.Dtos;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specification;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ProductsController : BaseApiController
{
    private readonly IGenericRepository<Product> _prodactsRepo;
    private readonly IGenericRepository<ProductBrand> _productBrandRepo;
    private readonly IGenericRepository<ProductType> _productTypeRepo;
    private readonly IMapper _mapper;
    public ProductsController(
     IGenericRepository<Product> prodactsRepo,
     IGenericRepository<ProductBrand> productBrandRepo,
     IGenericRepository<ProductType> productTypeRepo,
     IMapper mapper
     )
    {
        _productBrandRepo = productBrandRepo;
        _productTypeRepo = productTypeRepo;
        _prodactsRepo = prodactsRepo;
        _mapper = mapper;
    }

    [Cached(600)]
    [HttpGet]
    public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery]ProductSpecParams productParams)
    {
        var spec = new ProductsWithTypesAndBrandsSpecification(productParams);

        var countSpec = new ProductWithFiltersForCountSpecification(productParams);

        var totalItems = await _prodactsRepo.CountAsync(countSpec);

        var products = await _prodactsRepo.ListAsync(spec);

        var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

        return Ok(new Pagination<ProductToReturnDto>(productParams.PageIndex, productParams.PageSize, totalItems, data));
    }

    [Cached(600)]
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]//swagger documentation for the response status code
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]// swagger gonna recognize response type and status code
    public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
    {
        // spec= x.Id, x.IncludeBrand, x.IncludeType
        var spec = new ProductsWithTypesAndBrandsSpecification(id);

        var product = await _prodactsRepo.GetEntityWithSpec(spec);

        if (product == null) return NotFound(new ApiResponse(404));

        return _mapper.Map<Product, ProductToReturnDto>(product);
    }
    
    [Cached(600)]
    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
    {
        return Ok(await _productBrandRepo.ListAllAsync());
    }

    [Cached(600)]
    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
    {
        return Ok(await _productTypeRepo.ListAllAsync());
    }

}
