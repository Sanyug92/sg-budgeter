using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sg_budgeter.Models
{
    public class Households
    {

        public Households()
        {
            Budgets = new HashSet<Budgets>();
            Accounts = new HashSet<Accounts>();
            Users = new HashSet<ApplicationUser>();
            //Category = new HashSet<Categories>();
            Invitee = new HashSet<ApplicationUser>();

        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string AccessCode { get; set; }
        public DateTimeOffset inviteDate { get; set; }



        public virtual ICollection<Budgets> Budgets { get; set; }
        public virtual ICollection<Accounts> Accounts { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
        //public virtual ICollection<Categories> Category { get; set; }
        public virtual ICollection<ApplicationUser> Invitee { get; set; }

    }
}