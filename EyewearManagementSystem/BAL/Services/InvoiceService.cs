using DAL.Models;
using DAL.Repositories;

namespace BLL.Services
{
    public class InvoiceService
    {
        private readonly InvoiceRepository _invoiceRepo;
        private readonly ProductRepository _productRepo; // Cần Repo này để check kho

        public InvoiceService()
        {
            _invoiceRepo = new InvoiceRepository();
            _productRepo = new ProductRepository();
        }

        public string CreateInvoice(Invoice invoice)
        {
            // 1. Check tồn kho (Giữ nguyên logic foreach của bạn)
            foreach (InvoiceDetail detail in invoice.InvoiceDetails)
            {
                var product = _productRepo.GetProductById(detail.ProductId.Value);
                if (product == null) return "Sản phẩm không tồn tại!";
                if (product.Quantity <= 0)
                {
                    return $"Sản phẩm {product.ProductName} đã hết hàng!";
                }
                if (product.Quantity < detail.Quantity)
                {
                    return $"Sản phẩm {product.ProductName} không đủ hàng!";
                }
            }

            // 2. Gọi DAL và hứng kết quả bằng một object Invoice
            var savedInvoice = _invoiceRepo.CreateInvoice(invoice);

            // 3. Kiểm tra: Nếu khác null nghĩa là lưu thành công
            if (savedInvoice != null)
            {
                return "Success";
            }
            else
            {
                return "Lỗi khi lưu hóa đơn vào Database";
            }
        }
    }
}