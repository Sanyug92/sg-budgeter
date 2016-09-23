using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sg_budgeter.Models
{
    public class Categories
    {
        public Categories()
        {
            BudgetItems = new HashSet<BudgetItems>();
            Transactions = new HashSet<Transactions>();
            Household = new HashSet<Households>();
        }
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<BudgetItems> BudgetItems { get; set; }
        public virtual ICollection<Transactions> Transactions { get; set; }
        public virtual ICollection<Households> Household { get; set; }
    }
}