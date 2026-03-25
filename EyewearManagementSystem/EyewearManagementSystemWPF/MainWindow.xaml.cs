using DAL.Models;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EyewearManagementSystemWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private EyewareManagementContext _context;

        public MainWindow()
        {
            InitializeComponent();
            _context = new EyewareManagementContext();
            Window_Loaded();
        }
        private List<Customer> GetAllCustomer()
        {
            var customers = _context.Customers.ToList();
            return customers;
        }
        private void Window_Loaded()
        {
            dgCustomer.ItemsSource = GetAllCustomer();
        }
    }
}