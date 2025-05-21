using KidsLeisure.DAL.Entities;
using KidsLeisure.DAL.Helpers;
using KidsLeisure.DAL.Interfaces;
using System.Linq.Expressions;
using KidsLeisure.BLL.DTO;

namespace KidsLeisure.BLL.Interfaces
{
    public interface IOrderService
    {
        public OrderDto CurrentOrder { get; set; }
        public void ClearCurrentOrder();
        public OrderDto GetCurrentOrder() => CurrentOrder;
        Task<List<T>> GetAllItemsAsync<T>() where T : class, IItemEntity;
        Task<OrderDto> CreateCustomOrderAsync();
        Task<OrderDto> CreateBirthdayOrderAsync();
        Task<OrderDto> UpdateOrderAsync();
        Task DeleteOrderAsync();
        Task<T?> FindItemByAsync<T>(Expression<Func<T, bool>> predicate) where T : class, IItemEntity;
        Task<decimal> CalculateOrderPriceAsync(OrderEntity orderEntity);
        Task<decimal> CalculateOrderPriceAsync(ProgramType programType);
        void AddToOrderCollection(IItemEntity selectedItem);
        void RemoveFromOrderCollection(OrderItemDto selectedItem);
        void SetOrderTime(DateTime dateTime);
        void SetTotalPrice(decimal totalPrice);
        void SetOrderType(ProgramType eOrderType);
    }
}