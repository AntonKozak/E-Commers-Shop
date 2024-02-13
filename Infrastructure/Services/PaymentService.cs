using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Stripe;

namespace Infrastructure.Services;
/// <summary>
/// Service responsible for handling payments and creating/updating payment intents.
/// </summary>

public class PaymentService : IPaymentService
{
    private readonly IBasketRepository _basketRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;
    public PaymentService(IBasketRepository basketRepository, IUnitOfWork unitOfWork, IConfiguration configuration)
    {
        _configuration = configuration;
        _unitOfWork = unitOfWork;
        _basketRepository = basketRepository;
    }
    /// The ID of the basket for which to create or update the payment intent
    /// The updated customer basket with payment intent details
    public async Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId)
    {
        StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];
        var basket = await _basketRepository.GetBasketAsync(basketId);
        if (basket == null) return null;

        var shippingPrice = 0m;

        if (basket.DeliveryMethodId.HasValue)
        {
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync((int)basket.DeliveryMethodId);
            shippingPrice = deliveryMethod.Price;
        }
        // Ensure item prices are up-to-date with the latest database values
        foreach (var item in basket.Items)
        {
            var productItem = await _unitOfWork.Repository<Core.Entities.Product>().GetByIdAsync(item.Id);
            if (item.Price != productItem.Price)
            {
                item.Price = productItem.Price;
            }
        }
        var service = new PaymentIntentService();

        // Create or update the payment intent based on whether it already exists
        PaymentIntent intent;
        if (string.IsNullOrEmpty(basket.PaymentIntentId))
        {
            // If a payment intent doesn't exist, create a new one
            var options = new PaymentIntentCreateOptions
            {// Calculate the total amount for the payment intent, including items and shipping
                Amount = (long)basket.Items.Sum(i => i.Quantity * (i.Price * 100)) + (long)shippingPrice * 100,
                Currency = "usd",
                PaymentMethodTypes = new List<string> { "card" }// Specify the payment method type as "card"
            };
            intent = await service.CreateAsync(options);// Create the payment intent
            basket.PaymentIntentId = intent.Id;// Set the PaymentIntentId in the basket object
            basket.ClientSecret = intent.ClientSecret;// Set the ClientSecret in the basket object
        }
        else
        {// If a payment intent already exists, update it with the latest amount
            var options = new PaymentIntentUpdateOptions
            {// Calculate the updated amount for the payment intent, including items and shipping
                Amount = (long)basket.Items.Sum(i => i.Quantity * (i.Price * 100)) + (long)shippingPrice * 100
            };
            await service.UpdateAsync(basket.PaymentIntentId, options);// Update the existing payment intent
        }
        await _basketRepository.UpdateBasketAsync(basket);

        return basket;
    }
}
