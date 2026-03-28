using DAL.Models;
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
    }
}
