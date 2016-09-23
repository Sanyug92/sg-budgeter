using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sg_budgeter.Models
{
    public class DashMaster
    {
        public IEnumerable<Transactions> transactions { get; set; }
        public IEnumerable<Households> households { get; set; }
        public IList<ApplicationUser> householdMembers { get; set; }
        public IList<InviteViewModel> invitation { get; set; }


    }
}