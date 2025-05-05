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

namespace KidsLeisure.UI
{
    public partial class KidsLeisureWin : Form
    {
        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerService;
        public KidsLeisureWin(IOrderService orderService, ICustomerService customerService)
        {
            InitializeComponent();
            _orderService = orderService;
            panel2.Visible = false;
            _customerService = customerService;
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            panel2.Visible = true;
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            listBox2.Items.Clear();

            ToolStripMenuItem? clickedItem = e.ClickedItem as ToolStripMenuItem;

            if (clickedItem != null)
            {
                if (clickedItem.Text == "Головна" || clickedItem.Text == "Програми")
                {
                    panel2.Visible = false;
                }
                else if (clickedItem.Text == "Кошик")
                {
                    //Відкриття вікна Кошика
                    //ShoppingCartWin shoppingCart = new ShoppingCartWin(_orderService);
                    //shoppingCart.Show();
                }
                else
                {
                    panel2.Visible = true;
                    switch (clickedItem.Text)
                    {
                        case "Атракціони":
                            _ = UIHelper.LoadDBItemsAsync<AttractionEntity>(_orderService, listBox1);
                            break;
                        case "Зони":
                            _ = UIHelper.LoadDBItemsAsync<ZoneEntity>(_orderService, listBox1);
                            break;
                        case "Аніматори":
                            _ = UIHelper.LoadDBItemsAsync<CharacterEntity>(_orderService, listBox1);
                            break;
                        default:
                            listBox1.DataSource = null;
                            break;
                    }
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
            {
                MessageBox.Show("Оберіть елемент для додавання.", "Попередження", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedItem = listBox1.SelectedItem;

            if (listBox2.Items.Contains(selectedItem) || UIHelper.IsInCurrentOrder(selectedItem, _orderService))
            {
                MessageBox.Show("Цей елемент уже додано!", "Увага", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            UIHelper.AddElementInListBox(listBox1, listBox2);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            UIHelper.RemoveElementFromListBox(listBox2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox2.Items.Count == 0)
            {
                MessageBox.Show("Виберіть хоч щось, щоб додати його до замовлення.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            UIHelper.AddItemsToCart(_orderService, listBox2);
        }
        //Метод для зображення всіх складових програми.
        private async void деньНародженняToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await _orderService.CreateBirthdayOrderAsync();
            var message = "Програма для дня народження складає:\n";

            message += await UIHelper.GetAttractionsMessageAsync(_orderService);
            message += await UIHelper.GetZonesMessageAsync(_orderService);
            message += await UIHelper.GetCharactersMessageAsync(_orderService);
            message += $"Загальна ціна: {_orderService.CurrentOrder.TotalPrice} грн\n";
            message += $"Для замовлення оберіть дату та час:\n";
            //Відкриття вікна замовлення програми
            //ProgramOrderWin programOrderWin = new ProgramOrderWin(message, _orderService);
            //programOrderWin.Show();
        }
    }
}
