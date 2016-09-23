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

namespace sg_budgeter.Controllers
{
    public class BudgetsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Budgets
        public ActionResult Index()
        {
            var budget = db.Budget.Include(b => b.Households);
            return View(budget.ToList());
        }

        // GET: Budgets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Budgets budgets = db.Budget.Find(id);
            if (budgets == null)
            {
                return HttpNotFound();
            }
            return View(budgets);
        }

        // GET: Budgets/Create
        public ActionResult Create()
        {
            ViewBag.HouseholdsId = new SelectList(db.Household, "Id", "Name");
            return View();
        }

        // POST: Budgets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,HouseholdsId")] Budgets budgets)
        {
            if (ModelState.IsValid)
            {
                var curMem = db.Users.Find(User.Identity.GetUserId());
                var findUserhome = db.Household.Where(u => u.Users.Any(i => i.Id == curMem.Id));
                var home = findUserhome.FirstOrDefault();
                budgets.HouseholdsId = home.Id;
                db.Budget.Add(budgets);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.HouseholdsId = new SelectList(db.Household, "Id", "Name", budgets.HouseholdsId);
            return View(budgets);
        }

        // GET: Budgets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Budgets budgets = db.Budget.Find(id);
            if (budgets == null)
            {
                return HttpNotFound();
            }
            ViewBag.HouseholdsId = new SelectList(db.Household, "Id", "Name", budgets.HouseholdsId);
            return View(budgets);
        }

        // POST: Budgets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,HouseholdsId")] Budgets budgets)
        {
            if (ModelState.IsValid)
            {
                db.Entry(budgets).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HouseholdsId = new SelectList(db.Household, "Id", "Name", budgets.HouseholdsId);
            return View(budgets);
        }

        // GET: Budgets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Budgets budgets = db.Budget.Find(id);
            if (budgets == null)
            {
                return HttpNotFound();
            }
            return View(budgets);
        }

        // POST: Budgets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Budgets budgets = db.Budget.Find(id);
            db.Budget.Remove(budgets);
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
