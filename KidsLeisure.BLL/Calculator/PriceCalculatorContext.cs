using KidsLeisure.DAL.Entities;
using KidsLeisure.BLL.Interfaces;
namespace KidsLeisure.BLL.Calculator
{
    public class PriceCalculatorContext
    {
        private readonly IPriceCalculatorStrategy _priceCalculatorStrategy;
        public PriceCalculatorContext(IPriceCalculatorStrategy priceCalculatorStrategy)
        {
            _priceCalculatorStrategy = priceCalculatorStrategy;
        }

        public async Task<decimal> CalculatePrice(OrderEntity order, IOrderService orderService)
        {
            return await _priceCalculatorStrategy.CalculatePriceAsync(order);
        }
    }
}
