using API.Dtos;
using AutoMapper;
using Core.Entities;

namespace API.Helpers;
// going from product to productToReturnDto and return string of picture url
// from this "images/products/boot-ang2.png" to this "https://localhost:5001/images/products/boot-ang2.png"
public class ProductUrlResolver : IValueResolver<Product, ProductToReturnDto, string>
{
    private readonly IConfiguration _config;
    public ProductUrlResolver(IConfiguration config)//using Microsoft.Extensions.Configuration;
    {
        _config = config;
    }

    public string Resolve(Product source, ProductToReturnDto destination, string destMember, ResolutionContext context)
    {
        if (!string.IsNullOrEmpty(source.PictureUrl))
        {
            return _config["ApiUrl"] + source.PictureUrl;
        }

        return null;
    }
}
