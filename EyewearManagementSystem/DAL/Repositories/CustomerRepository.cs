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

        public List<Customer> SearchCustomersByPhone(string phone)
        {
            return _context.Customers
                .Where(c => c.Phone.Contains(phone))
                .ToList();
        }

        public void AddCustomer(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
        }

        public void UpdateCustomer(Customer customer)
        {
            var existing = _context.Customers.Find(customer.CustomerId);
            if (existing != null)
            {
                existing.FullName = customer.FullName;
                existing.Phone = customer.Phone;
                existing.Address = customer.Address;
                _context.SaveChanges();
            }
        }

        public bool HasInvoices(int customerId)
        {
            return _context.Invoices.Any(i => i.CustomerId == customerId);
        }

        public void DeleteCustomer(int customerId)
        {
            var customer = _context.Customers.Find(customerId);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                _context.SaveChanges();
            }
        }
    }
}
