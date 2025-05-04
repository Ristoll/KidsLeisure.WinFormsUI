using KidsLeisure.DAL.Entities;

namespace KidsLeisure.BLL.Interfaces
{
    public interface IPriceCalculatorStrategy
    {
        Task<decimal> CalculatePriceAsync(OrderEntity order);
    }
}
