using KidsLeisure.BLL.Interfaces;
using KidsLeisure.DAL.Entities;
using KidsLeisure.DAL.Helpers;
using KidsLeisure.BLL.Calculator;
using System.Linq.Expressions;
using KidsLeisure.DAL.Interfaces;

namespace KidsLeisure.BLL.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICustomerService _customerService;
        private readonly PriceCalculatorSelector _priceCalculatorSelector;

        public OrderEntity CurrentOrder { get; set; } = new();

        public OrderService(
            IUnitOfWork unitOfWork,
            ICustomerService customerService,
            PriceCalculatorSelector priceCalculatorSelector)
        {
            _unitOfWork = unitOfWork;
            _customerService = customerService;
            _priceCalculatorSelector = priceCalculatorSelector;
        }

        public async Task<List<T>> GetAllItemsAsync<T>() where T : class, IItemEntity
        {
            return await _unitOfWork.GetRepository<T>().GetAllAsync();
        }

        public void ClearCurrentOrder() => CurrentOrder = new();

        public async Task<OrderEntity> CreateCustomOrderAsync()
        {
            var customer = _customerService.CurrentCustomer!;
            CurrentOrder.CustomerId = customer.CustomerId;
            CurrentOrder.CustomerName = customer.NickName;
            CurrentOrder.CustomerPhone = customer.PhoneNumber;

            await _unitOfWork.GetRepository<OrderEntity>().AddAsync(CurrentOrder);
            customer.Orders.Add(CurrentOrder);

            await _unitOfWork.SaveChangesAsync();
            return CurrentOrder;
        }

        public async Task<OrderEntity> CreateBirthdayOrderAsync()
        {
            var customer = _customerService.CurrentCustomer!;
            CurrentOrder = new OrderEntity
            {
                ProgramType = ProgramType.Birthday,
                CustomerId = customer.CustomerId,
                CustomerName = customer.NickName,
                CustomerPhone = customer.PhoneNumber
            };

            await _unitOfWork.GetRepository<OrderEntity>().AddAsync(CurrentOrder);

            var attractions = (await _unitOfWork.GetRepository<AttractionEntity>().GetAllAsync()).Take(3).ToList();
            var characters = (await _unitOfWork.GetRepository<CharacterEntity>().GetAllAsync()).Take(2).ToList();
            var zones = (await _unitOfWork.GetRepository<ZoneEntity>().GetAllAsync()).Take(2).ToList();

            await _unitOfWork.GetRepository<OrderAttractionEntity>().AddRangeAsync(attractions.Select(a => new OrderAttractionEntity
            {
                OrderId = CurrentOrder.OrderId,
                AttractionId = a.AttractionId
            }));

            await _unitOfWork.GetRepository<OrderCharacterEntity>().AddRangeAsync(characters.Select(c => new OrderCharacterEntity
            {
                OrderId = CurrentOrder.OrderId,
                CharacterId = c.CharacterId
            }));

            await _unitOfWork.GetRepository<OrderZoneEntity>().AddRangeAsync(zones.Select(z => new OrderZoneEntity
            {
                OrderId = CurrentOrder.OrderId,
                ZoneId = z.ZoneId
            }));

            CurrentOrder.TotalPrice = await CalculateOrderPriceAsync(ProgramType.Birthday);
            customer.Orders.Add(CurrentOrder);

            await _unitOfWork.SaveChangesAsync();
            return CurrentOrder;
        }

        public async Task<OrderEntity> UpdateOrderAsync()
        {
            await _unitOfWork.GetRepository<OrderEntity>().UpdateAsync(CurrentOrder);
            await _unitOfWork.SaveChangesAsync();
            return CurrentOrder;
        }

        public async Task DeleteOrderAsync()
        {
            await _unitOfWork.GetRepository<OrderEntity>().DeleteAsync(CurrentOrder.OrderId);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<decimal> CalculateOrderPriceAsync(ProgramType OrderType)
        {
            var strategy = _priceCalculatorSelector.SelectStrategy(OrderType);
            return await strategy.CalculatePriceAsync(CurrentOrder);
        }

        public void SetOrderTime(DateTime dateTime) => CurrentOrder.Date = dateTime;
        public void SetOrderType(ProgramType eOrderType) => CurrentOrder.ProgramType = eOrderType;
        public void SetTotalPrice(decimal totalPrice) => CurrentOrder.TotalPrice = totalPrice;

        public void AddToOrderCollection(IItemEntity selectedItem)
        {
            switch (selectedItem)
            {
                case AttractionEntity attraction:
                    var orderAttraction = new OrderAttractionEntity
                    {
                        OrderId = CurrentOrder.OrderId,
                        AttractionId = attraction.AttractionId,
                        Attraction = attraction
                    };
                    CurrentOrder.Attractions.Add(orderAttraction);
                    break;

                case CharacterEntity character:
                    var orderCharacter = new OrderCharacterEntity
                    {
                        OrderId = CurrentOrder.OrderId,
                        CharacterId = character.CharacterId,
                        Character = character
                    };
                    CurrentOrder.Characters.Add(orderCharacter);
                    break;

                case ZoneEntity zone:
                    var orderZone = new OrderZoneEntity
                    {
                        OrderId = CurrentOrder.OrderId,
                        ZoneId = zone.ZoneId,
                        Zone = zone
                    };
                    CurrentOrder.Zones.Add(orderZone);
                    break;

                default:
                    throw new ArgumentException("Unknown item type.");
            }
        }

        public void RemoveFromOrderCollection(IOrderItemEntity selectedItem)
        {
            switch (selectedItem)
            {
                case OrderAttractionEntity attraction:
                    CurrentOrder.Attractions.Remove(attraction);
                    break;

                case OrderCharacterEntity character:
                    CurrentOrder.Characters.Remove(character);
                    break;

                case OrderZoneEntity zone:
                    CurrentOrder.Zones.Remove(zone);
                    break;

                default:
                    throw new ArgumentException("Unknown order item type.");
            }
        }

        public async Task<T?> FindItemByAsync<T>(Expression<Func<T, bool>> predicate)
            where T : class, IItemEntity
        {
            return await _unitOfWork.GetRepository<T>().FindAsync(predicate);
        }
    }
}
