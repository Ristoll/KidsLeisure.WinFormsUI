using Microsoft.EntityFrameworkCore;
using KidsLeisure.DAL.DBContext;
using KidsLeisure.BLL.Interfaces;
using KidsLeisure.DAL.Entities;
using KidsLeisure.DAL.Helpers;
using KidsLeisure.DAL.Interfaces;
using KidsLeisure.BLL.Calculator;
using System.Linq.Expressions;

namespace KidsLeisure.BLL.Services
{
    public class OrderService : IOrderService
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly ICustomerService _customerService;
        private readonly PriceCalculatorSelector _priceCalculatorSelector;

        public OrderEntity CurrentOrder { get; set; } = new();

        public OrderService(
            IRepositoryFactory repositoryFactory,
            ICustomerService customerService,
            PriceCalculatorSelector priceCalculatorSelector)
        {
            _repositoryFactory = repositoryFactory;
            _customerService = customerService;
            _priceCalculatorSelector = priceCalculatorSelector;
        }

        public async Task<List<T>> GetAllItemsAsync<T>() where T : class, IItemEntity
        {
            return await _repositoryFactory.GetRepository<T>().GetAllAsync();
        }

        public void ClearCurrentOrder() => CurrentOrder = new();

        public async Task<OrderEntity> CreateCustomOrderAsync()
        {
            var customer = _customerService.CurrentCustomer!;
            CurrentOrder.CustomerId = customer.CustomerId;
            CurrentOrder.CustomerName = customer.NickName;
            CurrentOrder.CustomerPhone = customer.PhoneNumber;

            await _repositoryFactory.GetRepository<OrderEntity>().AddAsync(CurrentOrder);
            customer.Orders.Add(CurrentOrder);
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

            await _repositoryFactory.GetRepository<OrderEntity>().AddAsync(CurrentOrder);

            var attractionRepo = _repositoryFactory.GetRepository<AttractionEntity>();
            var characterRepo = _repositoryFactory.GetRepository<CharacterEntity>();
            var zoneRepo = _repositoryFactory.GetRepository<ZoneEntity>();

            var orderAttractionRepo = _repositoryFactory.GetRepository<OrderAttractionEntity>();
            var orderCharacterRepo = _repositoryFactory.GetRepository<OrderCharacterEntity>();
            var orderZoneRepo = _repositoryFactory.GetRepository<OrderZoneEntity>();

            var attractions = (await attractionRepo.GetAllAsync()).Take(3).ToList();
            var characters = (await characterRepo.GetAllAsync()).Take(2).ToList();
            var zones = (await zoneRepo.GetAllAsync()).Take(2).ToList();

            await orderAttractionRepo.AddRangeAsync(attractions.Select(a => new OrderAttractionEntity
            {
                OrderId = CurrentOrder.OrderId,
                AttractionId = a.AttractionId
            }));

            await orderCharacterRepo.AddRangeAsync(characters.Select(c => new OrderCharacterEntity
            {
                OrderId = CurrentOrder.OrderId,
                CharacterId = c.CharacterId
            }));

            await orderZoneRepo.AddRangeAsync(zones.Select(z => new OrderZoneEntity
            {
                OrderId = CurrentOrder.OrderId,
                ZoneId = z.ZoneId
            }));

            CurrentOrder.TotalPrice = await CalculateOrderPriceAsync(ProgramType.Birthday);
            customer.Orders.Add(CurrentOrder);

            return CurrentOrder;
        }

        public async Task<OrderEntity> UpdateOrderAsync()
        {
            await _repositoryFactory.GetRepository<OrderEntity>().UpdateAsync(CurrentOrder);
            return CurrentOrder;
        }

        public async Task DeleteOrderAsync()
        {
            await _repositoryFactory.GetRepository<OrderEntity>().DeleteAsync(CurrentOrder.OrderId);
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
            var repo = _repositoryFactory.GetRepository<T>();
            return await repo.FindAsync(predicate);
        }
    }

}
