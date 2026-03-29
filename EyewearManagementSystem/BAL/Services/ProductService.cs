using DAL.Models;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class ProductService
    {
        private ProductRepository _productRepository;
        public ProductService() { 
            _productRepository = new ProductRepository();
        }
        public List<Product> GetAllProducts()
        {
            return _productRepository.GetAllProducts();
        }

        public List<Category> GetCategories()
        {
            return _productRepository.GetCategories();
        }

        public string AddCategory(string categoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName)) return "Tên danh mục không được để trống";
            var exists = _productRepository.GetCategories()
                .Any(x => string.Equals(x.CategoryName, categoryName, StringComparison.OrdinalIgnoreCase));
            if (exists) return "Tên danh mục không được trùng";

            try
            {
                _productRepository.AddCategory(new Category { CategoryName = categoryName });
                return "OK";
            }
            catch (Exception)
            {
                return "Lỗi khi thêm danh mục";
            }
        }

        public string DeleteCategory(int categoryId)
        {
            if (_productRepository.IsCategoryUsed(categoryId))
            {
                return "Không thể xóa danh mục này vì đang có sản phẩm thuộc danh mục!";
            }

            try
            {
                _productRepository.DeleteCategory(categoryId);
                return "OK";
            }
            catch (Exception ex)
            {
                return "Lỗi khi xóa danh mục: " + ex.Message;
            }
        }

        public string AddProduct(Product product)
        {
            if (string.IsNullOrWhiteSpace(product.ProductName)) return "Tên sản phẩm không được để trống";
            if (product.Price == null || product.Price <= 0) return "Giá phải lớn hơn 0";
            if (product.Quantity == null || product.Quantity < 0) return "Số lượng phải >= 0";
            if (product.CategoryId == null) return "Vui lòng chọn danh mục";

            var exists = _productRepository.GetAllProducts()
                .Any(x => string.Equals(x.ProductName, product.ProductName, StringComparison.OrdinalIgnoreCase));
            if (exists) return "Tên sản phẩm không được trùng";

            try
            {
                _productRepository.AddProduct(product);
                return "OK";
            }
            catch (Exception)
            {
                return "ERROR";
            }
        }

        public string UpdateProduct(Product product)
        {
            if (string.IsNullOrWhiteSpace(product.ProductName)) return "Tên sản phẩm không được để trống";
            if (product.Price == null || product.Price <= 0) return "Giá phải lớn hơn 0";
            if (product.Quantity == null || product.Quantity < 0) return "Số lượng phải >= 0";
            if (product.CategoryId == null) return "Vui lòng chọn danh mục";

            var exists = _productRepository.GetAllProducts()
                .Any(x => x.ProductId != product.ProductId && string.Equals(x.ProductName, product.ProductName, StringComparison.OrdinalIgnoreCase));
            if (exists) return "Tên sản phẩm không được trùng";

            try
            {
                _productRepository.UpdateProduct(product);
                return "OK";
            }
            catch (Exception)
            {
                return "ERROR";
            }
        }

        public string DeleteProduct(int productId)
        {
            try
            {
                bool isUsed = _productRepository.IsUsedInInvoice(productId);
                if (isUsed) return "FK_ERROR";

                _productRepository.DeleteProduct(productId);
                return "OK";
            }
            catch (Exception ex)
            {
                // In case of any database errors like foreign key constraints or tracking errors.
                return "ERROR: " + ex.Message;
            }
        }
    }
}
