using KidsLeisure.BLL.Interfaces;
using KidsLeisure.DAL.Entities;
namespace KidsLeisure.BLL.Calculator
{
    public class BaseCalculator
    {
        private readonly Lazy<IOrderService> _lazyOrderService;

        public BaseCalculator(Lazy<IOrderService> orderService)
        {
            _lazyOrderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
        }

        public async Task<decimal> CalculatePriceAsync(OrderEntity order)
        {
            var orderService = _lazyOrderService.Value;
            decimal totalPrice = 0;

            if (order.Zones != null && order.Zones.Count > 0)
            {
                foreach (var orderZone in order.Zones.Where(orderZone => orderZone.ZoneId != 0))
                {
                    var zone = await orderService.FindItemByAsync<ZoneEntity>(z => z.ZoneId == orderZone.ZoneId);
                    if (zone != null)
                    {
                        totalPrice += zone.Price;
                    }
                }
            }

            if (order.Attractions != null && order.Attractions.Count > 0)
            {
                foreach (var orderAttraction in order.Attractions.Where(orderAttraction => orderAttraction.AttractionId != 0))
                {
                    var attraction = await orderService.FindItemByAsync<AttractionEntity>(z => z.AttractionId == orderAttraction.AttractionId);
                    if (attraction != null)
                    {
                        totalPrice += attraction.Price;
                    }
                }
            }

            if (order.Characters != null && order.Characters.Count > 0)
            {
                foreach (var orderCharacter in order.Characters.Where(orderCharacter => orderCharacter.CharacterId != 0))
                {
                    var character = await orderService.FindItemByAsync<CharacterEntity>(z => z.CharacterId == orderCharacter.CharacterId);
                    if (character != null)
                    {
                        totalPrice += character.Price;
                    }
                }
            }

            return totalPrice;
        }
    }
}
