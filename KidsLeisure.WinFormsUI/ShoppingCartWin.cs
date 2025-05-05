using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KidsLeisure.BLL.Interfaces;
using KidsLeisure.DAL.Entities;
using KidsLeisure.DAL.Helpers;

namespace KidsLeisure.UI
{
    public partial class ShoppingCartWin : Form
    {
        private readonly IOrderService _orderService;
        public ShoppingCartWin(IOrderService orderService)
        {
            InitializeComponent();
            _orderService = orderService;
        }

        private async void ShoppingCart_Load(object sender, EventArgs e)
        {
            await UIHelper.LoadOrderItems<OrderZoneEntity>(_orderService, listBox1);
            await UIHelper.LoadOrderItems<OrderAttractionEntity>(_orderService, listBox2);
            await UIHelper.LoadOrderItems<OrderCharacterEntity>(_orderService, listBox3);
            await InitializePriceAsync();
        }

        private async Task InitializePriceAsync()
        {
            var totalPrice = await _orderService.CalculateOrderPriceAsync(ProgramType.Custom);
            label5.Text = totalPrice.ToString();
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            if (dateTimePicker1.Value < DateTime.Now)
            {
                MessageBox.Show("Виберіть дату та час, які ще не минули.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if(_orderService.CurrentOrder.Attractions.Count == 0 && _orderService.CurrentOrder.Characters.Count == 0 && _orderService.CurrentOrder.Zones.Count == 0)
            {
                MessageBox.Show("Замовлення пусте. Будь ласка, оберіть щось, щоб скласти замовлення.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _orderService.SetOrderType(ProgramType.Custom);
            var message = "Ваше замовлення складає:\n";

            message += await UIHelper.GetAttractionsMessageAsync(_orderService);
            message += await UIHelper.GetZonesMessageAsync(_orderService);
            message += await UIHelper.GetCharactersMessageAsync(_orderService);

            var totalPrice = await _orderService.CalculateOrderPriceAsync(ProgramType.Custom);
            _orderService.SetTotalPrice(totalPrice);
            DateTime dateTime = dateTimePicker1.Value;
            _orderService.SetOrderTime(dateTime);

            message += $"На дату та час: {dateTime.ToString()}\n";
            message += $"Загальна ціна: {totalPrice} грн\n";
            message += "(включно з доплатою за розгляд вашої кастомної програми)\n";
            message += "Дякуємо за ваше замовлення!";
            await _orderService.CreateCustomOrderAsync();
            _orderService.ClearCurrentOrder();
            MessageBox.Show(message, "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            UIHelper.RemoveSelectedItemFromCart(_orderService, listBox1);
            await _orderService.CalculateOrderPriceAsync(ProgramType.Custom);
            decimal price = await _orderService.CalculateOrderPriceAsync(ProgramType.Custom);
            label5.Text = $"{price} грн";
        }


        private async void button2_Click(object sender, EventArgs e)
        {
            UIHelper.RemoveSelectedItemFromCart(_orderService, listBox2);
            await _orderService.CalculateOrderPriceAsync(ProgramType.Custom);
            decimal price = await _orderService.CalculateOrderPriceAsync(ProgramType.Custom);
            label5.Text = $"{price} грн";
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            UIHelper.RemoveSelectedItemFromCart(_orderService, listBox3);
            await _orderService.CalculateOrderPriceAsync(ProgramType.Custom);
            decimal price = await _orderService.CalculateOrderPriceAsync(ProgramType.Custom);
            label5.Text = $"{price} грн";
        }
    }
}
