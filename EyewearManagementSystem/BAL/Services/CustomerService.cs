using DAL.Models;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class CustomerService
    {
            private readonly CustomerRepository _customerRepo;
            public CustomerService()
            {
                _customerRepo = new CustomerRepository();
            }
            public List<Customer> GetAllCustomers()
            {
                return _customerRepo.GetAllCustomers();
        
            }
            public Customer SearchByPhone(string phone)
            {
                return _customerRepo.SearchByPhone(phone);
            }
    }
}
