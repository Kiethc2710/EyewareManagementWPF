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
            return _context.Products.Include(p => p.Category).ToList();
        }

        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void UpdateProduct(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();
        }

        public void DeleteProduct(int productId)
        {
            var trackedProduct = _context.Products.Find(productId);
            if (trackedProduct != null)
            {
                _context.Products.Remove(trackedProduct);
                _context.SaveChanges();
            }
        }

        public bool IsUsedInInvoice(int productId)
        {
            return _context.InvoiceDetails.Any(i => i.ProductId == productId);
        }

        public List<Category> GetCategories()
        {
            return _context.Categories.ToList();
        }

        public void AddCategory(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        public bool IsCategoryUsed(int categoryId)
        {
            return _context.Products.Any(p => p.CategoryId == categoryId);
        }

        public void DeleteCategory(int categoryId)
        {
            var cat = _context.Categories.Find(categoryId);
            if (cat != null)
            {
                _context.Categories.Remove(cat);
                _context.SaveChanges();
            }
        }

        public Product GetProductById(int productId)
        {
            // Thêm AsNoTracking() để tránh việc EF tự cập nhật dữ liệu khi check tồn kho
            return _context.Products.AsNoTracking().FirstOrDefault(p => p.ProductId == productId);
        }
    }
}
