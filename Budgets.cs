using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sg_budgeter.Models
{
    public class Budgets
    {
        public Budgets()
        {
            BudgetItems = new HashSet<BudgetItems>();
        }


        public int Id { get; set; }
        public string Name { get; set; }
        public int HouseholdsId { get; set; }

        public virtual ICollection<BudgetItems> BudgetItems { get; set; }
        public virtual Households Households { get; set; }
    }
}