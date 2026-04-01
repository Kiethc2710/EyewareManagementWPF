using BLL.Services;
using DAL.Models;
using System.Windows;
using System.Windows.Controls;

namespace EyewearManagementSystemWPF
{
    public partial class ProductManagerWindow : Window
    {

        private readonly ProductService _productService;

        public ProductManagerWindow()
        {
            InitializeComponent();
            _productService = new ProductService();
            LoadManData();
        }

        private void LoadManData()
        {
            cboManCategory.ItemsSource = _productService.GetCategories();
            dgManProducts.ItemsSource = _productService.GetAllProducts();
        }

        private void btnAddCategory_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNewCategory.Text))
            {
                MessageBox.Show("Vui lòng nhập tên danh mục", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string result = _productService.AddCategory(txtNewCategory.Text.Trim());
            if (result == "OK")
            {
                MessageBox.Show("Thêm danh mục thành công.", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                txtNewCategory.Clear();
                cboManCategory.ItemsSource = _productService.GetCategories();
            }
            else
            {
                MessageBox.Show(result, "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnDeleteCategory_Click(object sender, RoutedEventArgs e)
        {
            if (cboManCategory.SelectedItem is Category selectedCat)
            {
                var res = MessageBox.Show(
                    $"Bạn có chắc muốn xóa danh mục '{selectedCat.CategoryName}' không?",
                    "Xác nhận",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (res == MessageBoxResult.Yes)
                {
                    string result = _productService.DeleteCategory(selectedCat.CategoryId);

                    if (result == "OK")
                    {
                        MessageBox.Show("Xóa danh mục thành công.",
                            "Thành công",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);

                        cboManCategory.ItemsSource = _productService.GetCategories();
                    }
                    else
                    {
                        MessageBox.Show(result,
                            "Lỗi",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn danh mục để xóa",
                    "Cảnh báo",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }

        private void dgManProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgManProducts.SelectedItem is Product selected)
            {
                txtManProductName.Text = selected.ProductName;
                txtManBrand.Text = selected.Brand;
                txtManPrice.Text = selected.Price?.ToString("G29");
                txtManQuantity.Text = selected.Quantity?.ToString();
                cboManCategory.SelectedValue = selected.CategoryId;
            }
        }

        private void btnManAdd_Click(object sender, RoutedEventArgs e)
        {
            var resDialog = MessageBox.Show("Bạn có chắc muốn thêm sản phẩm không?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (resDialog == MessageBoxResult.Yes)
            {
                if (string.IsNullOrWhiteSpace(txtManProductName.Text))
                {
                    MessageBox.Show("Tên sản phẩm không được để trống", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (!decimal.TryParse(txtManPrice.Text, out decimal p) || p <= 0)
                {
                    MessageBox.Show("Giá phải lớn hơn 0", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (!int.TryParse(txtManQuantity.Text, out int q) || q < 0)
                {
                    MessageBox.Show("Số lượng phải >= 0", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (cboManCategory.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn danh mục", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var product = new Product
                {
                    ProductName = txtManProductName.Text.Trim(),
                    Brand = txtManBrand.Text,
                    Price = p,
                    Quantity = q,
                    CategoryId = (int?)cboManCategory.SelectedValue
                };

                string result = _productService.AddProduct(product);
                if (result == "OK")
                {
                    MessageBox.Show("Thêm thành công.");
                    LoadManData();
                }
                else
                {
                    MessageBox.Show(result, "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void btnManUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (dgManProducts.SelectedItem is Product selected)
            {
                var resDialog = MessageBox.Show("Bạn có chắc muốn cập nhật sản phẩm không?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (resDialog == MessageBoxResult.Yes)
                {
                    if (string.IsNullOrWhiteSpace(txtManProductName.Text))
                    {
                        MessageBox.Show("Tên sản phẩm không được để trống", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    if (!decimal.TryParse(txtManPrice.Text, out decimal p) || p <= 0)
                    {
                        MessageBox.Show("Giá phải lớn hơn 0", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    if (!int.TryParse(txtManQuantity.Text, out int q) || q < 0)
                    {
                        MessageBox.Show("Số lượng phải >= 0", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    if (cboManCategory.SelectedValue == null)
                    {
                        MessageBox.Show("Vui lòng chọn danh mục", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    selected.ProductName = txtManProductName.Text.Trim();
                    selected.Brand = txtManBrand.Text;
                    selected.Price = p;
                    selected.Quantity = q;
                    selected.CategoryId = (int?)cboManCategory.SelectedValue;

                    string result = _productService.UpdateProduct(selected);
                    if (result == "OK")
                    {
                        MessageBox.Show("Cập nhật thành công.");
                        LoadManData();
                    }
                    else
                    {
                        MessageBox.Show(result, "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
        }

        private void btnManDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgManProducts.SelectedItem is Product selected)
            {
                var resDialog = MessageBox.Show("Bạn có chắc muốn xóa sản phẩm không?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (resDialog == MessageBoxResult.Yes)
                {
                    string result = _productService.DeleteProduct(selected.ProductId);
                    if (result == "FK_ERROR")
                    {
                        MessageBox.Show("Không thể xóa sản phẩm vì đã tồn tại trong hóa đơn!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else if (result == "OK")
                    {
                        MessageBox.Show("Xóa thành công.");
                        LoadManData();
                    }
                    else
                    {
                        MessageBox.Show("Lỗi khi xóa.");
                    }
                }
            }
        }
    }
}