using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BLL.Services;
using DAL.Models;

namespace EyewearManagementSystemWPF
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private readonly Staff_Service _staffService;
        public Login()
        {
            _staffService = new Staff_Service();
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            txtStatus.Text = string.Empty;
            var username = txtUsername.Text.Trim();
            var password = pwdBox.Password.Trim();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                txtStatus.Text = "Please enter both username and password.";
                return;
            }

            Account? account;
            try
            {
                account = _staffService.Authentication(username, password);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during authentication: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (account == null) 
            {
                txtStatus.Text = "Invalid username or password.";
                return;
            }
            var main = new MainWindow(account.AccountId);
            main.Show();
            this.Close();
        }
    }
}
