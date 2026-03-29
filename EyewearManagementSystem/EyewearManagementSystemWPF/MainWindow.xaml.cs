using BLL.Services;
using DAL.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace EyewearManagementSystemWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly int _currentAccountId;
        private readonly ProductService _productService;
        private readonly CustomerService _customerService;
        private readonly InvoiceService _invoiceService;
        ObservableCollection<InvoiceDetail> invoiceDetails = new ObservableCollection<InvoiceDetail>();
        public MainWindow(int accountID)
        {
            InitializeComponent();
            _productService = new ProductService();
            _customerService = new CustomerService();
            _invoiceService = new InvoiceService();
            _currentAccountId = accountID;
            dgProductInvoice.ItemsSource = invoiceDetails;
            Window_Loaded();

        }
        private void Window_Loaded()
        {
            cbProduct.ItemsSource = _productService.GetAllProducts();
        }

        private void Button_Click_AddProduct(object sender, RoutedEventArgs e)
        {
            var product = cbProduct.SelectedItem as Product;

            if (product == null)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm!");
                return;
            }

            var existing = invoiceDetails
                .FirstOrDefault(x => x.ProductId == product.ProductId);

            if (existing != null)
            {
                existing.Quantity += 1; // 🔥 mỗi lần add +1
            }
            else
            {
                invoiceDetails.Add(new InvoiceDetail
                {
                    ProductId = product.ProductId,
                    Product = product,
                    Quantity = 1, // 🔥 mặc định = 1
                    Price = product.Price
                });
            }

            dgProductInvoice.Items.Refresh(); // 🔥 đảm bảo UI update
            UpdateTotal(); // 🔥 thêm dòng này
        }


        private void CalculateTotal()
        {
            var list = dgProductInvoice.ItemsSource as List<InvoiceDetail>;

            if (list == null || list.Count == 0)
            {
                txtTotal.Text = "0";
                return;
            }

            decimal total = (decimal)list.Sum(x => x.Price * x.Quantity);

            txtTotal.Text = total.ToString("N0"); // format 1,000
        }
        private void ClearForm()
        {
            invoiceDetails.Clear();
            dgProductInvoice.Items.Refresh();

            txtPhoneSearch.Text = "";
            txtTotal.Text = "";
        }


        private void RemoveItem_Click(object sender, RoutedEventArgs e)
        {
            // 1. Xác định dòng (item) nào đang được click
            var button = sender as Button;
            var item = button?.DataContext as InvoiceDetail;

            if (item != null)
            {
                // 2. Hiện MessageBox xác nhận (Yes/No)
                MessageBoxResult result = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa sản phẩm '{item.Product?.ProductName}' khỏi hóa đơn?",
                    "Xác nhận xóa",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                // 3. Nếu người dùng chọn Yes thì mới xóa
                if (result == MessageBoxResult.Yes)
                {
                    // Xóa trực tiếp từ ObservableCollection (biến toàn cục)
                    invoiceDetails.Remove(item);

                    // Cập nhật lại tổng tiền hiển thị trên UI
                    UpdateTotal();

                    // Lưu ý: Không cần dgProductInvoice.Items.Refresh() vì ObservableCollection tự làm việc đó
                }
            }
        }
        private void ClearCustomer()
        {
            txtCustomerName.Text = "";
        }
        private void txtPhoneSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string phone = txtPhoneSearch.Text.Trim();

            if (string.IsNullOrEmpty(phone))
            {
                ClearCustomer();
                return;
            }

            var customer = _customerService.SearchByPhone(phone);

            if (customer != null)
            {
                txtCustomerName.Text = customer.FullName;

            }
            else
            {
                ClearCustomer();
            }
        }
        private void UpdateTotal()
        {
            decimal total = invoiceDetails
                .Sum(x => (x.Quantity ?? 0) * (x.Price ?? 0));

            txtTotal.Text = total.ToString("N0"); // format đẹp: 10,000
        }
        private void Button_Click_CreateInvoice_Click(object sender, RoutedEventArgs e)
        {
            // 1. Kiểm tra giỏ hàng trống
            if (invoiceDetails.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất một sản phẩm!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 2. Kiểm tra thông tin khách hàng
            var customer = _customerService.SearchByPhone(txtPhoneSearch.Text);
            if (customer == null)
            {
                MessageBox.Show("Vui lòng nhập số điện thoại khách hàng hợp lệ!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // 3. Khởi tạo đối tượng hóa đơn (Invoice)
            var invoice = new Invoice
            {
                CreatedDate = DateTime.Now,
                CustomerId = customer.CustomerId,
                AccountId = _currentAccountId, // Tạm thời fix, sau này lấy từ thông tin đăng nhập

                TotalAmount = invoiceDetails.Sum(x => (x.Quantity ?? 0) * (x.Price ?? 0)),

                // Chuyển đổi từ danh sách hiển thị sang danh sách chi tiết lưu DB
                InvoiceDetails = invoiceDetails.Select(x => new InvoiceDetail
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    Price = x.Price
                }).ToList()
            };

            // 4. Gọi BLL xử lý và nhận kết quả trả về
            string result = _invoiceService.CreateInvoice(invoice);

            // 5. Kiểm tra kết quả
            if (result == "Success")
            {
                MessageBox.Show("Tạo hóa đơn và cập nhật kho hàng thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);

                // Làm sạch form để tiếp tục bán đơn tiếp theo
                ClearForm();

                // QUAN TRỌNG: Load lại danh sách sản phẩm để cập nhật số lượng mới trên UI
                cbProduct.ItemsSource = _productService.GetAllProducts();
            }
            else
            {
                // Hiển thị lỗi từ BLL (Ví dụ: "Sản phẩm A không đủ hàng")
                MessageBox.Show(result, "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Button_Click_OrderList(object sender, RoutedEventArgs e)
        {
            InvoiceSelectionBorder.Visibility = Visibility.Visible;
            CustomerInfoBorder.Visibility = Visibility.Visible;
        }

        private void Button_Click_OpenInvoiceManagement(object sender, RoutedEventArgs e)
        {
            // Add logic here to open invoice management window
        }

        private void Button_Click_ProductHome(object sender, RoutedEventArgs e)
        {
            var win = new ProductManagerWindow();
            win.ShowDialog();

            // Reload product combo box when returned
            cbProduct.ItemsSource = _productService.GetAllProducts();
            dgProductInvoice.Items.Refresh();
        }

        private void Button_Click_CustomerHome(object sender, RoutedEventArgs e)
        {

        }

    }
}