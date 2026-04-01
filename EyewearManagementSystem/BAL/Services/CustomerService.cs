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
                if (string.IsNullOrWhiteSpace(phone)) return null;
                return _customerRepo.SearchByPhone(phone.Trim());
            }

            public List<Customer> SearchCustomersByPhone(string phone)
            {
                if (string.IsNullOrWhiteSpace(phone))
                    return GetAllCustomers();
                return _customerRepo.SearchCustomersByPhone(phone.Trim());
            }

            public string AddCustomer(Customer customer)
            {
                _customerRepo.AddCustomer(customer);
                return "Success";
            }

            public string UpdateCustomer(Customer customer)
            {
                _customerRepo.UpdateCustomer(customer);
                return "Success";
            }

            public bool HasInvoices(int customerId)
            {
                return _customerRepo.HasInvoices(customerId);
            }

            public string DeleteCustomer(int customerId)
            {
                try
                {
                    if (_customerRepo.HasInvoices(customerId))
                    {
                        return "Không thể xóa khách hàng này vì khách hàng đã có hóa đơn trên hệ thống.";
                    }

                    _customerRepo.DeleteCustomer(customerId);
                    return "Success";
                }
                catch (Exception ex)
                {
                    return "Lỗi: " + ex.Message;
                }
            }
    }
}
