using KidsLeisure.DAL.Entities;
using KidsLeisure.DAL.Helpers;
using KidsLeisure.DAL.Interfaces;

namespace KidsLeisure.BLL.Interfaces
{
    public interface IOrderService
    {
        public OrderEntity CurrentOrder { get; set; }

        public void SetOrder(OrderEntity order)
        {
            CurrentOrder = order;
        }
        public void ClearCurrentOrder();
        public OrderEntity GetCurrentOrder() => CurrentOrder;
        Task<List<T>> GetAllItemsAsync<T>() where T : class, IItemEntity;
        Task<OrderEntity> CreateCustomOrderAsync();
        Task<OrderEntity> CreateBirthdayOrderAsync();
        Task<OrderEntity> UpdateOrderAsync();
        Task DeleteOrderAsync();
        Task<IItemEntity?> FindItemByIdAsync<TItem>(int id) where TItem : class, IItemEntity;
        //Task<decimal> CalculateOrderPriceAsync(ProgramType OrderType);
        void AddToOrderCollection(IItemEntity selectedItem);
        void RemoveFromOrderCollection(IOrderItemEntity selectedItem);
        void SetOrderTime(DateTime dateTime);
        void SetTotalPrice(decimal totalPrice);
        void SetOrderType(ProgramType eOrderType);
    }
}