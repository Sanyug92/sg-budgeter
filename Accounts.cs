using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sg_budgeter.Models
{
    public class Accounts
    {
        public Accounts()
        {
            Transactions = new HashSet<Transactions>();
        }

        public int Id { get; set; }
        public int HouseholdsId { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public double ReconciledBalance { get; set; }
        public string OwnerId { get; set; }


        public virtual Households Households { get; set; }
        public virtual ApplicationUser Owner { get; set; }
        public virtual ICollection<Transactions> Transactions { get; set; }

    }
}