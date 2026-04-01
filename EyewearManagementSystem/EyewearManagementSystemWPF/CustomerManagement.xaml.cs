using BLL.Services;
using DAL.Models;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace EyewearManagementSystemWPF
{
    public partial class CustomerManagement : Window
    {
        private readonly CustomerService _customerService;

        public CustomerManagement()
        {
            InitializeComponent();
            _customerService = new CustomerService();
            LoadData();
        }

        private void LoadData()
        {
            dgCustomers.ItemsSource = _customerService.GetAllCustomers();
        }

        private bool ValidateInput(bool isUpdate = false, int? currentCustomerId = null)
        {
            string fullName = txtFullName.Text.Trim();
            string phone = txtPhone.Text.Trim();

            if (string.IsNullOrWhiteSpace(fullName))
            {
                MessageBox.Show("Vui lòng nhập họ và tên khách hàng!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(phone))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!phone.All(char.IsDigit))
            {
                MessageBox.Show("Số điện thoại chỉ được chứa định dạng số!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (phone.Length < 10 || phone.Length > 11)
            {
                MessageBox.Show("Số điện thoại phải từ 10 đến 11 số!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            // Kiểm tra trùng lặp số điện thoại
            var existingCustomer = _customerService.SearchByPhone(phone);
            if (existingCustomer != null)
            {
                // Nếu là thêm mới HOẶC (là cập nhật nhưng số điện thoại lại thuộc về ID khách hàng khác)
                if (!isUpdate || (isUpdate && existingCustomer.CustomerId != currentCustomerId))
                {
                    MessageBox.Show("Số điện thoại này đã tồn tại trong hệ thống. Vui lòng nhập số khác!", "Trùng dữ liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
            }

            return true;
        }

        private void ClearForm()
        {
            txtCustomerId.Clear();
            txtFullName.Clear();
            txtPhone.Clear();
            txtAddress.Clear();
            dgCustomers.SelectedItem = null;
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private void dgCustomers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgCustomers.SelectedItem is Customer selectedCustomer)
            {
                txtCustomerId.Text = selectedCustomer.CustomerId.ToString();
                txtFullName.Text = selectedCustomer.FullName;
                txtPhone.Text = selectedCustomer.Phone;
                txtAddress.Text = selectedCustomer.Address;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput(isUpdate: false)) return;

            var confirm = MessageBox.Show("Bạn có chắc chắn muốn thêm khách hàng này không?", "Xác nhận thêm", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (confirm == MessageBoxResult.Yes)
            {
                var newCustomer = new Customer
                {
                    FullName = txtFullName.Text.Trim(),
                    Phone = txtPhone.Text.Trim(),
                    Address = txtAddress.Text.Trim()
                };

                _customerService.AddCustomer(newCustomer);
                MessageBox.Show("Thêm khách hàng thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                ClearForm();
                LoadData();
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCustomerId.Text))
            {
                MessageBox.Show("Vui lòng chọn khách hàng trên lưới để cập nhật!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int currentId = int.Parse(txtCustomerId.Text);

            if (!ValidateInput(isUpdate: true, currentCustomerId: currentId)) return;

            var confirm = MessageBox.Show("Bạn có chắc chắn muốn cập nhật thông tin khách hàng này không?", "Xác nhận cập nhật", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (confirm == MessageBoxResult.Yes)
            {
                var updateCustomer = new Customer
                {
                    CustomerId = currentId,
                    FullName = txtFullName.Text.Trim(),
                    Phone = txtPhone.Text.Trim(),
                    Address = txtAddress.Text.Trim()
                };

                _customerService.UpdateCustomer(updateCustomer);
                MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                ClearForm();
                LoadData();
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCustomerId.Text))
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần xóa!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int customerId = int.Parse(txtCustomerId.Text);

            // Chặn ngay lập tức nếu có khóa ngoại (đã có hóa đơn) trước khi hỏi xác nhận
            if (_customerService.HasInvoices(customerId))
            {
                MessageBox.Show("Không thể xóa khách hàng này vì khách hàng đã có hóa đơn trên hệ thống.", "Không thể xóa", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var confirm = MessageBox.Show("Bạn có chắc chắn muốn xóa khách hàng này không?", "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (confirm == MessageBoxResult.Yes)
            {
                string result = _customerService.DeleteCustomer(customerId);

                if (result == "Success")
                {
                    MessageBox.Show("Xóa khách hàng thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    ClearForm();
                    LoadData();
                }
                else
                {
                    MessageBox.Show(result, "Lỗi khi xóa", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchKeyword = txtSearchPhone.Text.Trim();
            var results = _customerService.SearchCustomersByPhone(searchKeyword);

            if (results == null || results.Count == 0)
            {
                MessageBox.Show("Không tìm thấy khách hàng nào với số điện thoại này!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            dgCustomers.ItemsSource = results;
        }
    }
}