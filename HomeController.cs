using Microsoft.AspNet.Identity;
using sg_budgeter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sg_budgeter.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                AccountUpdatesHelper membership = new AccountUpdatesHelper(db);
                var curMem = db.Users.Find(User.Identity.GetUserId());
                var email = curMem.Email;

                if (membership.isInvitee(email))
                {
                    var findInviteehome = db.Household.Where(u => u.Invitee.Any(i => i.Id == curMem.Id));
                    var guesthome = findInviteehome.FirstOrDefault().Id;
                    ViewBag.Guest = guesthome;
                    return View(db.Household.ToList());
                }

                if (membership.isInAHouse(curMem.Id))
                {                   
                    var findUserhome = db.Household.Where(u => u.Users.Any(i => i.Id == curMem.Id));
                    var home = findUserhome.FirstOrDefault().Id;
                    ViewBag.Member = home;
                    return View(db.Household.ToList());
                }
                    
            }

            return View("Login","Account");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}