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
    }
}
