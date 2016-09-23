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
    public class TransactionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Transactions
        public ActionResult Index()
        {
            var transaction = db.Transaction.Include(t => t.Account).Include(t => t.Category).Include(t => t.EnteredBy);
            return View(transaction.ToList());
        }

        // GET: Transactions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transactions transactions = db.Transaction.Find(id);
            if (transactions == null)
            {
                return HttpNotFound();
            }
            return View(transactions);
        }

        // GET: Transactions/Create
        public ActionResult Create()
        {
            ViewBag.AccountId = new SelectList(db.Account, "Id", "Name");
            ViewBag.CategoryId = new SelectList(db.Category, "Id", "Name");
            ViewBag.EnteredById = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,AccountId,Date,Amount,Type,CategoryId,EnteredById,Reconciled,ReconciledAmount")] Transactions transactions)
        {
            var account = new Accounts();

            if (ModelState.IsValid)
            {
                var curMem = db.Users.Find(User.Identity.GetUserId());
                var curaccount = db.Account.FirstOrDefault(u => u.OwnerId == curMem.Id);
                transactions.AccountId = curaccount.Id;

                transactions.EnteredById = curMem.Id;
                transactions.Type = "Actual";


                transactions.Date = DateTime.Now;
                if (transactions.Reconciled == true)
                {
                    curaccount.Balance = curaccount.Balance + transactions.Amount;

                    db.Entry(account).State = EntityState.Modified;
                    db.SaveChanges();
                }

                if(transactions.Reconciled == false)
                {
                    db.Transaction.Add(transactions);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }

            ViewBag.AccountId = new SelectList(db.Account, "Id", "Name", transactions.AccountId);
            ViewBag.CategoryId = new SelectList(db.Category, "Id", "Name", transactions.CategoryId);
            ViewBag.EnteredById = new SelectList(db.Users, "Id", "FirstName", transactions.EnteredById);
            return View(transactions);
        }

        // GET: Transactions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transactions transactions = db.Transaction.Find(id);
            if (transactions == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccountId = new SelectList(db.Account, "Id", "Name", transactions.AccountId);
            ViewBag.CategoryId = new SelectList(db.Category, "Id", "Name", transactions.CategoryId);
            ViewBag.EnteredById = new SelectList(db.Users, "Id", "FirstName", transactions.EnteredById);
            return View(transactions);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update([Bind(Include = "Id,AccountId,Date,Amount,Type,CategoryId,EnteredById,Reconciled,ReconciledAmount")] Transactions transactions)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transactions).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AccountId = new SelectList(db.Account, "Id", "Name", transactions.AccountId);
            ViewBag.CategoryId = new SelectList(db.Category, "Id", "Name", transactions.CategoryId);
            ViewBag.EnteredById = new SelectList(db.Users, "Id", "FirstName", transactions.EnteredById);
            return View(transactions);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,AccountId,Date,Amount,Type,CategoryId,EnteredById,Reconciled,ReconciledAmount")] Transactions transactions)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transactions).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AccountId = new SelectList(db.Account, "Id", "Name", transactions.AccountId);
            ViewBag.CategoryId = new SelectList(db.Category, "Id", "Name", transactions.CategoryId);
            ViewBag.EnteredById = new SelectList(db.Users, "Id", "FirstName", transactions.EnteredById);
            return View(transactions);
        }

        // GET: Transactions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transactions transactions = db.Transaction.Find(id);
            if (transactions == null)
            {
                return HttpNotFound();
            }
            return View(transactions);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Transactions transactions = db.Transaction.Find(id);
            db.Transaction.Remove(transactions);
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
