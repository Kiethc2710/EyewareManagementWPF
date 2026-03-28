using DAL.Models;
using System;
using System.Linq;

namespace DAL.Repositories
{
    public class InvoiceRepository
    {
        private readonly EyewareManagementContext _context;

        public InvoiceRepository()
        {
            _context = new EyewareManagementContext();
        }

        // CHỈ GIỮ LẠI MỘT HÀM NÀY DUY NHẤT
        public Invoice CreateInvoice(Invoice invoice)
        {
            // Sử dụng Transaction để bảo vệ dữ liệu (Atomic)
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // 1. Lưu hóa đơn (EF sẽ tự lưu InvoiceDetails bên trong invoice)
                    _context.Invoices.Add(invoice);

                    // 2. Trừ số lượng tồn kho của từng sản phẩm
                    foreach (var detail in invoice.InvoiceDetails)
                    {
                        // Tìm sản phẩm trực tiếp từ Database để trừ kho
                        var productInDb = _context.Products.Find(detail.ProductId);
                        if (productInDb != null)
                        {
                            productInDb.Quantity -= detail.Quantity;
                        }
                    }

                    // 3. Lưu tất cả thay đổi
                    _context.SaveChanges();

                    // 4. Xác nhận hoàn tất
                    transaction.Commit();
                    return invoice;
                }
                catch (Exception ex)
                {
                    // Nếu có lỗi, hủy bỏ toàn bộ quá trình
                    transaction.Rollback();
                    // Ném lỗi ra ngoài để BLL nhận diện được
                    throw new Exception("Lỗi hệ thống DAL: " + ex.Message);
                }
            }
        }
    }
}