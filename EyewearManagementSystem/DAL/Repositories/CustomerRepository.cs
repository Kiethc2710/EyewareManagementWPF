using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class CustomerRepository
    {
        private readonly EyewareManagementContext _context;
        public CustomerRepository()
        {
            _context = new EyewareManagementContext();
        }
        public List<Customer> GetAllCustomers()
        {
            return _context.Customers.ToList();
        }
        public Customer SearchByPhone(string phone)
        {

            return _context.Customers
                .FirstOrDefault(c => c.Phone == phone);
        }
    }
}
