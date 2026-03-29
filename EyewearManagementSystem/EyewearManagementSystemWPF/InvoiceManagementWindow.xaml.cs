using BLL.Services;
using DAL.Models;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace EyewearManagementSystemWPF
{
    public partial class InvoiceManagementWindow : Window
    {
        private readonly InvoiceService _invoiceService;

        public InvoiceManagementWindow()
        {
            InitializeComponent();
            _invoiceService = new InvoiceService();
            LoadInvoices();
        }

        // LOAD DATA
        private void LoadInvoices()
        {
            dgInvoices.ItemsSource = _invoiceService.GetAllInvoices();
        }

        // CLICK ROW → SHOW DETAIL
        private void dgInvoices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgInvoices.SelectedItem is Invoice selected)
            {
                icDetails.ItemsSource = selected.InvoiceDetails;
            }
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            string keyword = txtSearch.Text.Trim();

            var selectedItem = cbSearchType.SelectedItem as ComboBoxItem;
            string type = selectedItem.Content.ToString();

            if (string.IsNullOrEmpty(keyword))
            {
                LoadInvoices();
            }
            else
            {   
                var result = _invoiceService.SearchInvoices(keyword, type);
                if (result == null || result.Count == 0)
                {
                    MessageBox.Show("❌ Không tìm thấy kết quả!",
                                    "Thông báo",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning);

                    dgInvoices.ItemsSource = null;
                    icDetails.ItemsSource = null;
                } else
                {
                    dgInvoices.ItemsSource = _invoiceService.SearchInvoices(keyword, type);
                }
            }
        }
    }
}