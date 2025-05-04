using Microsoft.EntityFrameworkCore;
using KidsLeisure.DAL.DBContext;
using KidsLeisure.BLL.Interfaces;
using KidsLeisure.DAL.Entities;
using KidsLeisure.DAL.Helpers;
using KidsLeisure.DAL.Interfaces;

namespace KidsLeisure.Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<OrderEntity> _orderRepository;
        private readonly LeisureDbContext _context;
        //private readonly PriceCalculatorSelector _priceCalculatorSelector;
        private readonly ICustomerService _customerService;

        public OrderService(
            IRepository<OrderEntity> orderRepository,
            LeisureDbContext context,
            //PriceCalculatorSelector priceCalculatorSelector,
            ICustomerService customerService)
        {
            _orderRepository = orderRepository;
            _context = context ?? throw new ArgumentNullException(nameof(context));
            //_priceCalculatorSelector = priceCalculatorSelector;
            _customerService = customerService;
        }

        public OrderEntity CurrentOrder { get; set; } = new OrderEntity();
        public OrderEntity GetCurrentOrder() => CurrentOrder;

        public async Task<List<T>> GetAllItemsAsync<T>() where T : class, IItemEntity
        {
            return await _context.Set<T>().ToListAsync();
        }

        public void ClearCurrentOrder()
        {
            CurrentOrder = new OrderEntity();
        }

        public async Task<OrderEntity> CreateCustomOrderAsync()
        {
            CurrentOrder.CustomerName = _customerService.CurrentCustomer.NickName;
            CurrentOrder.CustomerId = _customerService.CurrentCustomer.CustomerId;
            CurrentOrder.CustomerPhone = _customerService.CurrentCustomer.PhoneNumber;
            await _orderRepository.AddAsync(CurrentOrder);
            _customerService.CurrentCustomer.Orders.Add(CurrentOrder);
            return CurrentOrder;
        }

        public async Task<OrderEntity> CreateBirthdayOrderAsync()
        {
            CurrentOrder = new OrderEntity
            {
                ProgramType = ProgramType.Birthday,
                CustomerId = _customerService.CurrentCustomer.CustomerId,
                CustomerName = _customerService.CurrentCustomer.NickName,
                CustomerPhone = _customerService.CurrentCustomer.PhoneNumber
            };

            await _context.Set<OrderEntity>().AddAsync(CurrentOrder);
            await _context.SaveChangesAsync();

            var attractions = await _context.Set<AttractionEntity>().Take(3).ToListAsync();
            var characters = await _context.Set<CharacterEntity>().Take(2).ToListAsync();
            var zones = await _context.Set<ZoneEntity>().Take(2).ToListAsync();

            var orderAttractions = attractions.Select(a => new OrderAttractionEntity
            {
                OrderId = CurrentOrder.OrderId,
                AttractionId = a.AttractionId
            }).ToList();
            await _context.Set<OrderAttractionEntity>().AddRangeAsync(orderAttractions);

            var orderCharacters = characters.Select(c => new OrderCharacterEntity
            {
                OrderId = CurrentOrder.OrderId,
                CharacterId = c.CharacterId
            }).ToList();
            await _context.Set<OrderCharacterEntity>().AddRangeAsync(orderCharacters);

            var orderZones = zones.Select(z => new OrderZoneEntity
            {
                OrderId = CurrentOrder.OrderId,
                ZoneId = z.ZoneId
            }).ToList();
            await _context.Set<OrderZoneEntity>().AddRangeAsync(orderZones);

            //CurrentOrder.TotalPrice = await CalculateOrderPriceAsync(ProgramType.Birthday);

            await _context.SaveChangesAsync();
            _customerService.CurrentCustomer.Orders.Add(CurrentOrder);
            return CurrentOrder;
        }

        public async Task<OrderEntity> UpdateOrderAsync()
        {
            await _orderRepository.UpdateAsync(CurrentOrder);
            return CurrentOrder;
        }

        public async Task DeleteOrderAsync()
        {
            await _orderRepository.DeleteAsync(CurrentOrder.OrderId);
        }

        /*public async Task<decimal> CalculateOrderPriceAsync(ProgramType OrderType)
        {
            var priceCalculator = _priceCalculatorSelector.SelectStrategy(OrderType);
            return await priceCalculator.CalculatePriceAsync(CurrentOrder);
        }*/

        public void SetOrderTime(DateTime dateTime)
        {
            CurrentOrder.Date = dateTime;
        }

        public void SetOrderType(ProgramType eOrderType)
        {
            CurrentOrder.ProgramType = eOrderType;
        }

        public void SetTotalPrice(decimal totalPrice)
        {
            CurrentOrder.TotalPrice = totalPrice;
        }

        public void AddToOrderCollection(IItemEntity selectedItem)
        {
            switch (selectedItem)
            {
                case ZoneEntity zone:
                    CurrentOrder.Zones!.Add(new OrderZoneEntity
                    {
                        ZoneId = zone.ZoneId,
                        OrderId = CurrentOrder.OrderId
                    });
                    break;
                case AttractionEntity attraction:
                    CurrentOrder.Attractions!.Add(new OrderAttractionEntity
                    {
                        AttractionId = attraction.AttractionId,
                        OrderId = CurrentOrder.OrderId
                    });
                    break;
                case CharacterEntity character:
                    CurrentOrder.Characters!.Add(new OrderCharacterEntity
                    {
                        CharacterId = character.CharacterId,
                        OrderId = CurrentOrder.OrderId
                    });
                    break;
                default:
                    throw new InvalidOperationException("Невідомий тип елемента");
            }
        }

        public void RemoveFromOrderCollection(IOrderItemEntity selectedItem)
        {
            switch (selectedItem)
            {
                case OrderAttractionEntity attraction:
                    var attractionToRemove = CurrentOrder.Attractions
                        .FirstOrDefault(a => a.AttractionId == attraction.AttractionId);
                    if (attractionToRemove != null)
                        CurrentOrder.Attractions!.Remove(attractionToRemove);
                    else
                        throw new InvalidOperationException("Атракціон не знайдений у замовленні");
                    break;
                case OrderZoneEntity zone:
                    var zoneToRemove = CurrentOrder.Zones
                        .FirstOrDefault(z => z.ZoneId == zone.ZoneId);
                    if (zoneToRemove != null)
                        CurrentOrder.Zones!.Remove(zoneToRemove);
                    else
                        throw new InvalidOperationException("Зона не знайдена у замовленні");
                    break;
                case OrderCharacterEntity character:
                    var characterToRemove = CurrentOrder.Characters
                        .FirstOrDefault(c => c.CharacterId == character.CharacterId);
                    if (characterToRemove != null)
                        CurrentOrder.Characters!.Remove(characterToRemove);
                    else
                        throw new InvalidOperationException("Персонаж не знайдений у замовленні");
                    break;
                default:
                    throw new InvalidOperationException("Невідомий тип елемента");
            }
        }

        public async Task<IItemEntity?> FindItemByIdAsync<TItem>(int id) where TItem : class, IItemEntity
        {
            if (typeof(TItem) == typeof(AttractionEntity))
                return await _context.Set<AttractionEntity>().FirstOrDefaultAsync(a => a.AttractionId == id) as IItemEntity;

            if (typeof(TItem) == typeof(ZoneEntity))
                return await _context.Set<ZoneEntity>().FirstOrDefaultAsync(z => z.ZoneId == id) as IItemEntity;

            if (typeof(TItem) == typeof(CharacterEntity))
                return await _context.Set<CharacterEntity>().FirstOrDefaultAsync(c => c.CharacterId == id) as IItemEntity;

            throw new InvalidOperationException($"Невідомий тип елемента: {typeof(TItem).FullName}");
        }
    }
}
