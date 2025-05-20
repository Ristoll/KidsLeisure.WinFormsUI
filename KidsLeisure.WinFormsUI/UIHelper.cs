using System.ComponentModel;
using KidsLeisure.DAL.Interfaces;
using KidsLeisure.BLL.Interfaces;
using KidsLeisure.DAL.Entities;
using KidsLeisure.DAL.Helpers;
using KidsLeisure.BLL.DTO;
using AutoMapper;

namespace KidsLeisure.UI
{
    public static class UIHelper
    {
        public static async Task LoadDBItemsAsync<T>(IOrderService entityListService, ListBox listBox)
            where T : class, IItemEntity
        {
            var entities = await entityListService.GetAllItemsAsync<T>();

            var displayItems = entities.Select(e => new DisplayItem<T>
            {
                Entity = e,
                DisplayName = $"{e.Name} - {e.Price} грн"
            }).ToList();

            listBox.DataSource = displayItems;
            listBox.DisplayMember = "DisplayName";
        }

        public static async Task LoadOrderItems<T>(IOrderService orderService, ListBox listBox, IMapper mapper)
    where T : class, IOrderItemDto
        {
            var displayItems = new BindingList<DisplayItem<T>>();

            if (typeof(T) == typeof(OrderAttractionDto))
            {
                foreach (var attraction in orderService.CurrentOrder.Attractions)
                {
                    var entity = await orderService.FindItemByAsync<AttractionEntity>(a => a.AttractionId == attraction.Id);
                    var dto = mapper.Map<OrderAttractionDto>(entity);
                    if (dto != null)
                    {
                        var displayName = $"{dto.Name} - {dto.Price} грн";
                        displayItems.Add(new DisplayItem<T> { Entity = dto as T, DisplayName = displayName });
                    }
                }
            }
            else if (typeof(T) == typeof(OrderZoneDto))
            {
                foreach (var zone in orderService.CurrentOrder.Zones)
                {
                    var entity = await orderService.FindItemByAsync<ZoneEntity>(z => z.ZoneId == zone.Id);
                    var dto = mapper.Map<OrderZoneDto>(entity);
                    if (dto != null)
                    {
                        var displayName = $"{dto.Name} - {dto.Price} грн";
                        displayItems.Add(new DisplayItem<T> { Entity = dto as T, DisplayName = displayName });
                    }
                }
            }
            else if (typeof(T) == typeof(OrderCharacterDto))
            {
                foreach (var character in orderService.CurrentOrder.Characters)
                {
                    var entity = await orderService.FindItemByAsync<CharacterEntity>(c => c.CharacterId == character.Id);
                    var dto = mapper.Map<OrderCharacterDto>(entity);
                    if (dto != null)
                    {
                        var displayName = $"{dto.Name} - {dto.Price} грн";
                        displayItems.Add(new DisplayItem<T> { Entity = dto as T, DisplayName = displayName });
                    }
                }
            }

            listBox.DataSource = displayItems;
            listBox.DisplayMember = nameof(DisplayItem<T>.DisplayName);
            listBox.ValueMember = nameof(DisplayItem<T>.Entity);
        }
        public static void AddElementInListBox(ListBox listBox1, ListBox listBox2)
        {
            if (listBox1.SelectedIndex != -1)
            {
                var selectedItem = listBox1.SelectedItem;
                listBox2.Items.Add(selectedItem!);
            }
            else
            {
                MessageBox.Show("Будь ласка, виберіть елемент у списку.");
            }
        }

        public static void RemoveElementFromListBox(ListBox listBox)
        {
            if (listBox.SelectedIndex != -1)
            {
                listBox.Items.Remove(listBox.SelectedItem!);
            }
            else
            {
                MessageBox.Show("Будь ласка, виберіть елемент у списку.");
            }
        }

        public static void AddItemsToCart(IOrderService orderService, ListBox listBox)
        {
            foreach (var item in listBox.Items)
            {
                switch (item)
                {
                    case DisplayItem<AttractionEntity> attractionItem:
                        orderService.AddToOrderCollection(attractionItem.Entity);
                        break;
                    case DisplayItem<ZoneEntity> zoneItem:
                        orderService.AddToOrderCollection(zoneItem.Entity);
                        break;
                    case DisplayItem<CharacterEntity> characterItem:
                        orderService.AddToOrderCollection(characterItem.Entity);
                        break;
                    default:
                        throw new InvalidCastException("Непідтримуваний тип елемента в списку.");
                }
            }

            listBox.Items.Clear();
            MessageBox.Show("Елементи успішно додані до замовлення.");
        }

        public static void RemoveSelectedItemFromCart(IOrderService orderService, ListBox listBox)
        {
            if (listBox.SelectedItem == null)
            {
                MessageBox.Show("Будь ласка, виберіть елемент для видалення.");
                return;
            }

            switch (listBox.SelectedItem)
            {
                case DisplayItem<OrderAttractionDto> attractionItem:
                    orderService.RemoveFromOrderCollection(attractionItem.Entity);
                    ((BindingList<DisplayItem<OrderAttractionDto>>)listBox.DataSource!).Remove(attractionItem);
                    break;

                case DisplayItem<OrderCharacterDto> characterItem:
                    orderService.RemoveFromOrderCollection(characterItem.Entity);
                    ((BindingList<DisplayItem<OrderCharacterDto>>)listBox.DataSource!).Remove(characterItem);
                    break;

                case DisplayItem<OrderZoneDto> zoneItem:
                    orderService.RemoveFromOrderCollection(zoneItem.Entity);
                    ((BindingList<DisplayItem<OrderZoneDto>>)listBox.DataSource!).Remove(zoneItem);
                    break;

                default:
                    MessageBox.Show("Неправильний тип елемента.");
                    return;
            }

            MessageBox.Show("Елемент видалено із замовлення.");
        }

        public static async Task<string> GetAttractionsMessageAsync(IOrderService orderService)
        {
            var message = string.Empty;
            List<OrderAttractionDto> items = orderService.CurrentOrder.Attractions.ToList();

            if (items != null && items.Count > 0)
            {
                message += "Атракціони:\n";
                foreach (var item in items)
                {
                    var attraction = await orderService.FindItemByAsync<AttractionEntity>(z => z.AttractionId == item.Id);
                    var name = attraction?.Name ?? "Невідомий атракціон";
                    var price = attraction?.Price ?? 0;
                    message += $"{name} - {price} грн\n";
                }
            }
            else
            {
                message += "Немає атракціонів у замовленні.\n";
            }

            return message;
        }

        public static async Task<string> GetZonesMessageAsync(IOrderService orderService)
        {
            var message = string.Empty;
            List<OrderZoneDto> items = orderService.CurrentOrder.Zones.ToList();

            if (items != null && items.Count > 0)
            {
                message += "Зони:\n";
                foreach (var item in items)
                {
                    var zone = await orderService.FindItemByAsync<ZoneEntity>(z => z.ZoneId == item.Id);
                    var name = zone?.Name ?? "Невідома зона";
                    var price = zone?.Price ?? 0;
                    message += $"{name} - {price} грн\n";
                }
            }
            else
            {
                message += "Немає зон у замовленні.\n";
            }

            return message;
        }

        public static async Task<string> GetCharactersMessageAsync(IOrderService orderService)
        {
            var message = string.Empty;
            List<OrderCharacterDto> items = orderService.CurrentOrder.Characters.ToList();

            if (items != null && items.Count > 0)
            {
                message += "Персонажі:\n";
                foreach (var item in items)
                {
                    var character = await orderService.FindItemByAsync<CharacterEntity>(z => z.CharacterId == item.Id);
                    var name = character?.Name ?? "Невідомий персонаж";
                    var price = character?.Price ?? 0;
                    message += $"{name} - {price} грн\n";
                }
            }
            else
            {
                message += "Немає персонажів у замовленні.\n";
            }

            return message;
        }
        public static bool IsInCurrentOrder(object item, IOrderService orderService)
        {
            var order = orderService.CurrentOrder;

            switch (item)
            {
                case DisplayItem <ZoneEntity> zone:
                    return order.Zones.Any(z => z.Id == zone.Entity.ZoneId);
                case DisplayItem<AttractionEntity> attraction:
                    return order.Attractions.Any(a => a.Id == attraction.Entity.AttractionId);
                case DisplayItem <CharacterEntity> character:
                    return order.Characters.Any(c => c.Id == character.Entity.CharacterId);
                default:
                    return false;
            }
        }
    }
}
