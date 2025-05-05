using KidsLeisure.BLL.Interfaces;

namespace KidsLeisure.UI
{
    public partial class ProgramOrderWin : Form
    {
        IOrderService _orderService;
        public ProgramOrderWin(string message, IOrderService orderService)
        {
            InitializeComponent();
            label1.Text = message;
            _orderService = orderService;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime dateTime = dateTimePicker1.Value;
            _orderService.SetOrderTime(dateTime);
            _orderService.UpdateOrderAsync();
            MessageBox.Show("Замовлення успішно створено!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
            _orderService.ClearCurrentOrder();
            this.Close();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

}
