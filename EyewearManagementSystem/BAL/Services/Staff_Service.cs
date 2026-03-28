
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;
using DAL.Repositories;

namespace BLL.Services
{
    public class Staff_Service
    {
        private readonly AccountRepo _accountRepo;

        public Staff_Service()
        {
            _accountRepo = new AccountRepo();
        }

        public Account? Authentication(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return null;
            }
            return _accountRepo.GetByUserNameAndPassword(username, password);
        }
    }
}