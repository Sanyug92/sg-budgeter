using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sg_budgeter.Models
{
    public class InviteViewModel
    {
        public int Id { get; set; }
        public string AccessCode { get; set; }
        public string Email { get; set; }
        public string link { get; set; }
        public DateTimeOffset inviteDate { get; set; }
    }
}