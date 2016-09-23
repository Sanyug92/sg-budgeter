using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sg_budgeter.Models
{
    public class BudgetItems
    {
        public int Id { get; set; }
        public int CategoriesId { get; set; }
        public int BudgetId { get; set; }
        public decimal Amount { get; set; }

        public virtual Budgets Budget { get; set; }
        public virtual Categories Categories { get; set; }
    }
}