@ -0,0 + 1,28 @@
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;

namespace BLL
{
    public class Staff_Service
    {
        private readonly DAL.AccountRepo _accountRepo;

        public Staff_Service()
        {
            _accountRepo = new DAL.AccountRepo();
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