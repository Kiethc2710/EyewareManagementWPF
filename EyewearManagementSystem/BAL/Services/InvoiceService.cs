using DAL.Models;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class InvoiceService
    {
        private readonly InvoiceRepository _invoiceRepo;
        public InvoiceService()
        {
            _invoiceRepo = new InvoiceRepository();
        }
        public Invoice CreateInvoice(Invoice invoice)
        {
            if (invoice == null)
            {
                throw new ArgumentNullException(nameof(invoice), "Invoice cannot be null.");
            }
            return _invoiceRepo.CreateInvoice(invoice);
        }
    }
}
