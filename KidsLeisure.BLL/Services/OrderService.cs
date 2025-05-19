using AutoMapper;
using KidsLeisure.BLL.Calculator;
using KidsLeisure.BLL.DTO;
using KidsLeisure.BLL.Interfaces;
using KidsLeisure.DAL.Entities;
using KidsLeisure.DAL.Helpers;
using KidsLeisure.DAL.Interfaces;
using System.Linq.Expressions;

namespace KidsLeisure.BLL.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICustomerService _customerService;
        private readonly PriceCalculatorSelector _priceCalculatorSelector;
        private readonly IMapper _mapper;

        public OrderDto CurrentOrder { get; set; } = new();

        public OrderService(
            IUnitOfWork unitOfWork,
            ICustomerService customerService,
            PriceCalculatorSelector priceCalculatorSelector,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _customerService = customerService;
            _priceCalculatorSelector = priceCalculatorSelector;
            _mapper = mapper;
        }

        public async Task<List<T>> GetAllItemsAsync<T>() where T : class, IItemEntity
        {
            return await _unitOfWork.GetRepository<T>().GetAllAsync();
        }

        public void ClearCurrentOrder() => CurrentOrder = new();

        public async Task<OrderDto> CreateCustomOrderAsync()
        {
            var customerDto = _customerService.CurrentCustomer!;
            var orderEntity = _mapper.Map<OrderEntity>(CurrentOrder);

            orderEntity.CustomerId = customerDto.Id;
            orderEntity.CustomerName = customerDto.NickName;
            orderEntity.CustomerPhone = customerDto.PhoneNumber;

            await _unitOfWork.GetRepository<OrderEntity>().AddAsync(orderEntity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<OrderDto>(orderEntity);
        }

        public async Task<OrderDto> CreateBirthdayOrderAsync()
        {
            var customerDto = _customerService.CurrentCustomer!;
            var orderEntity = new OrderEntity
            {
                ProgramType = ProgramType.Birthday,
                CustomerId = customerDto.Id,
                CustomerName = customerDto.NickName,
                CustomerPhone = customerDto.PhoneNumber
            };

            await _unitOfWork.GetRepository<OrderEntity>().AddAsync(orderEntity);

            var attractions = (await _unitOfWork.GetRepository<AttractionEntity>().GetAllAsync()).Take(3).ToList();
            var characters = (await _unitOfWork.GetRepository<CharacterEntity>().GetAllAsync()).Take(2).ToList();
            var zones = (await _unitOfWork.GetRepository<ZoneEntity>().GetAllAsync()).Take(2).ToList();

            await _unitOfWork.GetRepository<OrderAttractionEntity>().AddRangeAsync(attractions.Select(a => new OrderAttractionEntity
            {
                OrderId = orderEntity.OrderId,
                AttractionId = a.AttractionId
            }));

            await _unitOfWork.GetRepository<OrderCharacterEntity>().AddRangeAsync(characters.Select(c => new OrderCharacterEntity
            {
                OrderId = orderEntity.OrderId,
                CharacterId = c.CharacterId
            }));

            await _unitOfWork.GetRepository<OrderZoneEntity>().AddRangeAsync(zones.Select(z => new OrderZoneEntity
            {
                OrderId = orderEntity.OrderId,
                ZoneId = z.ZoneId
            }));

            orderEntity.TotalPrice = await CalculateOrderPriceAsync(ProgramType.Birthday);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<OrderDto>(orderEntity);
        }

        public async Task<OrderDto> UpdateOrderAsync()
        {
            var orderEntity = _mapper.Map<OrderEntity>(CurrentOrder);
            await _unitOfWork.GetRepository<OrderEntity>().UpdateAsync(orderEntity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<OrderDto>(orderEntity);
        }

        public async Task DeleteOrderAsync()
        {
            await _unitOfWork.GetRepository<OrderEntity>().DeleteAsync(CurrentOrder.OrderId);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<decimal> CalculateOrderPriceAsync(ProgramType orderType)
        {
            var orderEntity = _mapper.Map<OrderEntity>(CurrentOrder);
            var strategy = _priceCalculatorSelector.SelectStrategy(orderType);
            return await strategy.CalculatePriceAsync(orderEntity);
        }

        public void SetOrderTime(DateTime dateTime) => CurrentOrder.Date = dateTime;
        public void SetOrderType(ProgramType orderType) => CurrentOrder.ProgramType = _mapper.Map<ProgramTypeDto>(orderType);
        public void SetTotalPrice(decimal totalPrice) => CurrentOrder.TotalPrice = totalPrice;

        public void AddToOrderCollection(IItemEntity selectedItem)
        {
            switch (selectedItem)
            {
                case AttractionEntity attraction:
                    CurrentOrder.Attractions.Add(new OrderAttractionDto
                    {
                        AttractionId = attraction.AttractionId
                    });
                    break;

                case CharacterEntity character:
                    CurrentOrder.Characters.Add(new OrderCharacterDto
                    {
                        CharacterId = character.CharacterId
                    });
                    break;

                case ZoneEntity zone:
                    CurrentOrder.Zones.Add(new OrderZoneDto
                    {
                        ZoneId = zone.ZoneId
                    });
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
                    CurrentOrder.Attractions.RemoveAll(a => a.AttractionId == attraction.AttractionId);
                    break;

                case OrderCharacterEntity character:
                    CurrentOrder.Characters.RemoveAll(c => c.CharacterId == character.CharacterId);
                    break;

                case OrderZoneEntity zone:
                    CurrentOrder.Zones.RemoveAll(z => z.ZoneId == zone.ZoneId);
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

