using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

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

        public List<Invoice> GetAllInvoices()
        {
            return _context.Invoices
                .Include(x => x.Customer)
                .Include(X => X.Account)
                .Include(x => x.InvoiceDetails)
                    .ThenInclude(d => d.Product)
                .OrderByDescending(x => x.CreatedDate)
                .ToList();
        }

        public List<Invoice> SearchInvoices(string keyword, string type)
        {
            var query = _context.Invoices
                .Include(x => x.Customer)
                .Include(x => x.Account)
                .Include(x => x.InvoiceDetails)
                    .ThenInclude(d => d.Product)
                .AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {   

                switch (type)
                {
                    case "Phone":
                        query = query.Where(x => x.Customer.Phone.ToLower().Contains(keyword));
                        break;

                    case "Customer Name":
                        query = query.Where(x => x.Customer.FullName.ToLower().Contains(keyword.ToLower()));
                        break;

                    case "Staff":
                        query = query.Where(x => x.Account.Username.ToLower().Contains(keyword.ToLower()));
                        break;
                }
            }

            return query
                .OrderByDescending(x => x.CreatedDate)
                .ToList();
        }

    }
}