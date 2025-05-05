using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;
using KidsLeisure.DAL.Interfaces;
using KidsLeisure.BLL.Interfaces;
using KidsLeisure.DAL.Entities;
using KidsLeisure.DAL.Helpers;


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

        public static async Task LoadOrderItems<T>(IOrderService orderService, ListBox listBox) where T : class
        {
            var displayItems = new BindingList<DisplayItem<T>>();

            if (typeof(T) == typeof(OrderAttractionEntity))
            {
                foreach (var attraction in orderService.CurrentOrder.Attractions ?? Enumerable.Empty<OrderAttractionEntity>())
                {
                    var entity = await orderService.FindItemByIdAsync<AttractionEntity>(attraction.AttractionId);
                    var displayName = entity != null ? $"{entity.Name} - {entity.Price} грн" : "(Невідомий атракціон)";
                    displayItems.Add(new DisplayItem<T> { Entity = (T)(object)attraction, DisplayName = displayName });
                }
            }
            else if (typeof(T) == typeof(OrderZoneEntity))
            {
                foreach (var zone in orderService.CurrentOrder.Zones ?? Enumerable.Empty<OrderZoneEntity>())
                {
                    var entity = await orderService.FindItemByIdAsync<ZoneEntity>(zone.ZoneId);
                    var displayName = entity != null ? $"{entity.Name} - {entity.Price} грн" : "(Невідома зона)";
                    displayItems.Add(new DisplayItem<T> { Entity = (T)(object)zone, DisplayName = displayName });
                }
            }
            else if (typeof(T) == typeof(OrderCharacterEntity))
            {
                foreach (var character in orderService.CurrentOrder.Characters ?? Enumerable.Empty<OrderCharacterEntity>())
                {
                    var entity = await orderService.FindItemByIdAsync<CharacterEntity>(character.CharacterId);
                    var displayName = entity != null ? $"{entity.Name} - {entity.Price} грн" : "(Невідомий аніматор)";
                    displayItems.Add(new DisplayItem<T> { Entity = (T)(object)character, DisplayName = displayName });
                }
            }
            else
            {
                throw new InvalidOperationException($"Невідомий тип елемента: {typeof(T).FullName}");
            }

            listBox.DataSource = displayItems;
            listBox.DisplayMember = "DisplayName";
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
                case DisplayItem<OrderZoneEntity> zoneItem:
                    orderService.RemoveFromOrderCollection(zoneItem.Entity);
                    ((BindingList<DisplayItem<OrderZoneEntity>>)listBox.DataSource!).Remove(zoneItem);
                    break;

                case DisplayItem<OrderAttractionEntity> attractionItem:
                    orderService.RemoveFromOrderCollection(attractionItem.Entity);
                    ((BindingList<DisplayItem<OrderAttractionEntity>>)listBox.DataSource!).Remove(attractionItem);
                    break;

                case DisplayItem<OrderCharacterEntity> characterItem:
                    orderService.RemoveFromOrderCollection(characterItem.Entity);
                    ((BindingList<DisplayItem<OrderCharacterEntity>>)listBox.DataSource!).Remove(characterItem);
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
            var items = orderService.CurrentOrder.Attractions;

            if (items != null && items.Count > 0)
            {
                message += "Атракціони:\n";
                foreach (var item in items)
                {
                    var attraction = await orderService.FindItemByIdAsync<AttractionEntity>(item.AttractionId);
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
            var items = orderService.CurrentOrder.Zones;

            if (items != null && items.Count > 0)
            {
                message += "Зони:\n";
                foreach (var item in items)
                {
                    var zone = await orderService.FindItemByIdAsync<ZoneEntity>(item.ZoneId);
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
            var items = orderService.CurrentOrder.Characters;

            if (items != null && items.Count > 0)
            {
                message += "Персонажі:\n";
                foreach (var item in items)
                {
                    var character = await orderService.FindItemByIdAsync<CharacterEntity>(item.CharacterId);
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
                    return order.Zones.Any(z => z.ZoneId == zone.Entity.ZoneId);
                case DisplayItem<AttractionEntity> attraction:
                    return order.Attractions.Any(a => a.AttractionId == attraction.Entity.AttractionId);
                case DisplayItem <CharacterEntity> character:
                    return order.Characters.Any(c => c.CharacterId == character.Entity.CharacterId);
                default:
                    return false;
            }
        }
    }
}
