using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class ProductRepository
    {
        private readonly EyewareManagementContext _context;
        public ProductRepository() { 
            _context = new EyewareManagementContext();
        }
        public List<Product> GetAllProducts()
        {
            return _context.Products.ToList();
        }


        public Product GetProductById(int productId)
        {
            // Thêm AsNoTracking() để tránh việc EF tự cập nhật dữ liệu khi check tồn kho
            return _context.Products.AsNoTracking().FirstOrDefault(p => p.ProductId == productId);
        }
    }
}
