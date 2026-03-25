using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Invoice
{
    public int InvoiceId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int CustomerId { get; set; }

    public int AccountId { get; set; }

    public decimal? TotalAmount { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; } = new List<InvoiceDetail>();
}
