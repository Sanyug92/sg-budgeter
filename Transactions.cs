using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sg_budgeter.Models
{
    public class Transactions
    {
     public int Id { get; set;}
     public int AccountId { get; set; }
     public DateTimeOffset Date { get; set; }
     public decimal Amount { get; set; }
     public string Type { get; set; }
     public string Location { get; set; }
     public int CategoryId { get; set; }
     public string EnteredById { get; set; }
     public bool Reconciled { get; set; }
     public int ReconciledAmount { get; set; }

     public virtual Accounts Account { get; set; }
     public virtual Categories Category { get; set; }
     public virtual ApplicationUser EnteredBy { get; set; }
    }
}