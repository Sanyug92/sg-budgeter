using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using sg_budgeter.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace sg_budgeter.Controllers
{
    public class AccountUpdatesHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserManager<ApplicationUser> userManager;
        private RoleManager<IdentityRole> roleManager;

        public AccountUpdatesHelper(ApplicationDbContext context)
        {
            userManager = new UserManager<ApplicationUser>(
            new UserStore<ApplicationUser>(context));
            roleManager = new RoleManager<IdentityRole>(
            new RoleStore<IdentityRole>(context));
            db = context;

        }

        //public void UpdateAccount(decimal currentAmount, decimal newtransionAmount)
        //{
        //    var account = new Accounts();

        //    decimal update = currentAmount + newtransionAmount;

        //    account.Balance = update;

        //    db.Entry(account).State = EntityState.Modified;
        //    db.SaveChanges();
        //}

        //public async Task<ActionResult> Invite(string Email, string Link, string code)
        //{
        //    var account = new Accounts();
        //    var svc = new EmailService();
        //    var msg = new IdentityMessage();
        //    msg.Subject = "Invite";
        //    msg.Body = "\r\n Please considering joinging my household" + "Click is Link:" + Link + ". Use the following code" + code;

        //    msg.Destination = Email;
        //    await svc.SendAsync(msg);

        //    //db.Entry(account).State = EntityState.Modified;
        //    //db.SaveChanges();

        //    var result = account.;

        //    return result.ToString(); .Succeeded;
        //}

        public void AddToHouseHolds(string userid, int houseID, string code)
        {          
            var house = db.Household.Find(houseID);
            house.AccessCode = code;
            if (house.AccessCode == code)
            {
                house.Users.Add(db.Users.Find(userid));
                db.SaveChanges();

            }

        }
        public bool isInHouse(int houseID, string userid)
        {
            var house = db.Household.Find(houseID);
            if (house.Users.Any(u => u.Id == userid)){
                return true;
            }
            return false;

        }

        public bool isInAHouse(string userid)
        {
            bool contains = db.Household.Any(i => i.Users.Any(u => u.Id == userid));
            if (contains == true)
            {
                return true;
            } 
            return false;

        }
        public bool isInvited(int houseID, string Email)
        {
            var house = db.Household.Find(houseID);
           
            bool contains = house.Invitee.Any(e => e.Email ==  Email);
            if (contains == true)
            {
                return true;
            }
            return false;

        }

        public bool isInvitee(string email)
        {
            

            bool contains = db.Household.Any(i => i.Invitee.Any(e => e.Email == email));
            if (contains == true)
            {
                return true;
            }
            return false;

        }

        public void removeInvited(int houseID, string userid)
        {

            var house = db.Household.Find(houseID);
            if ((DateTime.Now - house.inviteDate).TotalHours > 3 || house.Users.Contains(db.Users.Find(userid)))
            {
                house.Invitee.Remove(db.Users.Find(userid));
                db.SaveChanges();

            }
        }
        
           
        public void LeaveHouseHolds(string userid, int houseId)
        {
            var house = db.Household.Find(houseId);
            var user = db.Users.Find(userid);
            house.Users.Remove(db.Users.Find(userid));
            db.SaveChanges();

        }
    }
}