using KidsLeisure.DAL.Entities;
using KidsLeisure.BLL.Interfaces;

namespace KidsLeisure.BLL.Calculator
{
    public class DefaultPriceCalculator : BaseCalculator, IPriceCalculatorStrategy
    {
        public DefaultPriceCalculator(Lazy<IOrderService> orderService) : base(orderService)
        {
        }
    }
}
