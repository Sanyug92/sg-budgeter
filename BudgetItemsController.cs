using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using sg_budgeter.Models;

namespace sg_budgeter.Controllers
{
    public class BudgetItemsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BudgetItems
        public ActionResult Index()
        {
            var budgetitem = db.Budgetitem.Include(b => b.Budget).Include(b => b.Categories);
            return View(budgetitem.ToList());
        }

        // GET: BudgetItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BudgetItems budgetItems = db.Budgetitem.Find(id);
            if (budgetItems == null)
            {
                return HttpNotFound();
            }
            return View(budgetItems);
        }

        // GET: BudgetItems/Create
        public ActionResult Create()
        {
            ViewBag.BudgetId = new SelectList(db.Budget, "Id", "Name");
            ViewBag.CategoriesId = new SelectList(db.Category, "Id", "Name");
            return View();
        }

        // POST: BudgetItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CategoriesId,BudgetId,Amount")] BudgetItems budgetItems)
        {
            
            if (ModelState.IsValid)
            {

                db.Budgetitem.Add(budgetItems);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BudgetId = new SelectList(db.Budget, "Id", "Name", budgetItems.BudgetId);
            ViewBag.CategoriesId = new SelectList(db.Category, "Id", "Name", budgetItems.CategoriesId);
            return View(budgetItems);
        }

        // GET: BudgetItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BudgetItems budgetItems = db.Budgetitem.Find(id);
            if (budgetItems == null)
            {
                return HttpNotFound();
            }
            ViewBag.BudgetId = new SelectList(db.Budget, "Id", "Name", budgetItems.BudgetId);
            ViewBag.CategoriesId = new SelectList(db.Category, "Id", "Name", budgetItems.CategoriesId);
            return View(budgetItems);
        }

        // POST: BudgetItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CategoriesId,BudgetId,Amount")] BudgetItems budgetItems)
        {
            if (ModelState.IsValid)
            {
                db.Entry(budgetItems).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BudgetId = new SelectList(db.Budget, "Id", "Name", budgetItems.BudgetId);
            ViewBag.CategoriesId = new SelectList(db.Category, "Id", "Name", budgetItems.CategoriesId);
            return View(budgetItems);
        }

        // GET: BudgetItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BudgetItems budgetItems = db.Budgetitem.Find(id);
            if (budgetItems == null)
            {
                return HttpNotFound();
            }
            return View(budgetItems);
        }

        // POST: BudgetItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BudgetItems budgetItems = db.Budgetitem.Find(id);
            db.Budgetitem.Remove(budgetItems);
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
