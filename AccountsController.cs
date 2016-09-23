using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using sg_budgeter.Models;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace sg_budgeter.Controllers
{
    public class AccountsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Accounts
        public ActionResult Index()
        {
            var account = db.Account.Include(a => a.Households);
            return View(account.ToList());
        }

    
    

        // GET: Accounts/Details/
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Accounts accounts = db.Account.Find(id);
            if (accounts == null)
            {
                return HttpNotFound();
            }
            return View(accounts);
        }

        // GET: Accounts/Create
        public ActionResult Create()
        {
            ViewBag.HouseholdsId = new SelectList(db.Household, "Id", "Name");
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Balance,ReconciledBalance")] Accounts accounts)
        {
            if (ModelState.IsValid)
            {
                var curMem = db.Users.Find(User.Identity.GetUserId());
                var findUserhome = db.Household.Where(u => u.Users.Any(i => i.Id == curMem.Id));
                var home = findUserhome.FirstOrDefault();
                accounts.HouseholdsId = home.Id;
                accounts.Owner = curMem;
                db.Account.Add(accounts);
                db.SaveChanges();
                return RedirectToAction("Index");
            }


            //ViewBag.HouseholdsId = new SelectList(db.Household, "Id", "Name", accounts.HouseholdsId);
            return View(accounts);
        }

        // Update Acount
        public async Task<ActionResult> Update (Accounts model, int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            Accounts accounts = db.Account.Find(id);

            var user = db.Users.Find(User.Identity.GetUserId());
            if (accounts.Balance < 0)
            {

                var svc = new EmailService();
                var msg = new IdentityMessage();
                msg.Subject = "Account Over Drafted";
                msg.Body = "\r\n Your account was overdrafted.";

                msg.Destination = user.Email;
                await svc.SendAsync(msg);
            }
            if (accounts == null)
            {
                return HttpNotFound();
            }
            ViewBag.HouseholdsId = new SelectList(db.Household, "Id", "Name", accounts.HouseholdsId);
            return View(accounts);
        }

        // GET: Accounts/Edit/5
        public ActionResult Edit(Accounts model, int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
          
            Accounts accounts = db.Account.Find(id);
         
            if (accounts == null)
            {
                return HttpNotFound();
            }
            ViewBag.HouseholdsId = new SelectList(db.Household, "Id", "Name", accounts.HouseholdsId);
            return View(accounts);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,HouseholdsId,Name,Balance,ReconciledBalance")] Accounts accounts)
        {
            if (ModelState.IsValid)
            {
                
                db.Entry(accounts).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HouseholdsId = new SelectList(db.Household, "Id", "Name", accounts.HouseholdsId);
            return View(accounts);
        }

        // GET: Accounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Accounts accounts = db.Account.Find(id);
            if (accounts == null)
            {
                return HttpNotFound();
            }
            return View(accounts);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Accounts accounts = db.Account.Find(id);
            db.Account.Remove(accounts);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
