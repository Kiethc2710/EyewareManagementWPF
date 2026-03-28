
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL.Repositories
{
    public class AccountRepo
    {
        public Account? GetByUserNameAndPassword(string username, string password)
        {
            using var db = new EyewareManagementContext();
            return db.Accounts.FirstOrDefault(a => a.Username == username && a.Password == password);
        }
    }
}