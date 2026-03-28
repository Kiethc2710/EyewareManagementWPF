using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class InvoiceRepository
    {
        private readonly EyewareManagementContext _context;
        public InvoiceRepository()
        {
            _context = new EyewareManagementContext();
        }
        public Invoice CreateInvoice(Invoice invoice)
        {
            _context.Invoices.Add(invoice);
            _context.SaveChanges();
            return invoice;
        }
    }
}
