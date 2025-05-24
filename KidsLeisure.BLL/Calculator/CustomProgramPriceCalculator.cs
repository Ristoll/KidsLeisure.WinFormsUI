using KidsLeisure.DAL.Entities;
using KidsLeisure.BLL.Interfaces;

namespace KidsLeisure.BLL.Calculator
{
    public class CustomProgramPriceCalculator : BaseCalculator, IPriceCalculatorStrategy
    {
        private readonly decimal additionalPrice = 100;

        public CustomProgramPriceCalculator(Lazy<IOrderService> orderService) : base(orderService)
        {
        }

        public override async Task<decimal> CalculatePriceAsync(OrderEntity order)
        {
            decimal totalPrice = await base.CalculatePriceAsync(order);
            return totalPrice + additionalPrice;
        }
    }
}