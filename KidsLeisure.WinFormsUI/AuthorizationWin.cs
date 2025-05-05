using System.Text.RegularExpressions;
using KidsLeisure.BLL.Interfaces;
using KidsLeisure.DAL.Entities;

namespace KidsLeisure.UI
{
    public partial class AuthorizationWin : Form
    {
        private readonly ICustomerService _customerService;
        private readonly IOrderService _orderService;
        public AuthorizationWin(IOrderService orderService, ICustomerService customerService)
        {
            _orderService = orderService;
            _customerService = customerService;
            InitializeComponent();
        }
        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string nickname = textBox2.Text.Trim();
                string phoneNumber = textBox3.Text.Trim();

                ValidatePhoneNumber(phoneNumber);
                ValidateNickName(nickname);

                _customerService.CurrentCustomer = new CustomerEntity
                {
                    NickName = nickname,
                    PhoneNumber = phoneNumber
                };

                await _customerService.CreateCustomerAsync();

                MessageBox.Show("Користувач авторизований успішно!");

                KidsLeisureWin mainForm = new KidsLeisureWin(_orderService, _customerService);
                mainForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.Hide();
        }
        private static void ValidatePhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentException("Номер телефону не може бути пустим!");

            string pattern = @"^0\d{9}$";

            if (!Regex.IsMatch(phoneNumber, pattern))
                throw new ArgumentException("Некоректний формат номера телефону! Введіть у форматі 0XXXXXXXXX.");
        }
        private static void ValidateNickName(string nickname)
        {
            if (string.IsNullOrWhiteSpace(nickname))
                throw new ArgumentException("Ім’я не може бути порожнім!");

            if (nickname.Length < 2)
                throw new ArgumentException("Ім’я має містити щонайменше 2 символи.");
        }
    }
}
