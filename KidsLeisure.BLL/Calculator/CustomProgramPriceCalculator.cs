using KidsLeisure.DAL.Entities;
using KidsLeisure.BLL.Interfaces;

namespace KidsLeisure.BLL.Calculator
{
    public class CustomProgramPriceCalculator : BaseCalculator, IPriceCalculatorStrategy
    {
        private readonly decimal additionalPrice = 100;

        public CustomProgramPriceCalculator(Lazy<IOrderService> orderService) : base(orderService)
        {
            var orderService = _lazyOrderService.Value;
            decimal totalPrice = 0;

            if (order.Zones != null && order.Zones.Count > 0)
            {
                foreach (var orderZone in order.Zones.Where(orderZone => orderZone.ZoneId != 0))
                {
                    var zone = await orderService.FindItemByIdAsync<ZoneEntity>(orderZone.ZoneId);
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
                    var attraction = await orderService.FindItemByIdAsync<AttractionEntity>(orderAttraction.AttractionId);
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
                    var character = await orderService.FindItemByIdAsync<CharacterEntity>(orderCharacter.CharacterId);
                    if (character != null)
                    {
                        totalPrice += character.Price;
                    }
                }
            }

            totalPrice += additionalPrice;

            return totalPrice + additionalPrice;
        }
    }
}