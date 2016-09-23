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
using Newtonsoft.Json;
using System.Web.Helpers;

namespace sg_budgeter.Controllers
{
    public class HouseholdsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Households
        public ActionResult Index()
        {
            return View(db.Household.ToList());
        }
        public ActionResult myHouse()
        {
            var curMem = db.Users.Find(User.Identity.GetUserId());
            var findUserhome = db.Household.Where(u => u.Users.Any(i => i.Id == curMem.Id));
            var home = findUserhome.FirstOrDefault();
            ViewBag.Name = curMem.FirstName;

            AccountUpdatesHelper membership = new AccountUpdatesHelper(db);
            if (home.Invitee.Count() > 0)
            {
                membership.removeInvited(home.Id, curMem.Id);
            }

            if (membership.isInHouse(home.Id, curMem.Id))
            {
                // For DropDown list of Household Members
                var dashModel = new DashMaster();
                var MList = home.Users.ToList();
                dashModel.householdMembers = MList;


                var myTranactions = db.Transaction.Where(u => u.EnteredById == curMem.Id).ToList();
                dashModel.transactions = myTranactions;

                //var invitation = new InviteViewModel();

                //dashModel.invitation.Add(invitation);

                return View(dashModel);
            }

            return View();
        }

        //Post for AJAX.Needs selected userid
        public ActionResult GetChart()
        {

            var curMem = db.Users.Find(User.Identity.GetUserId());
            var findUserhome = db.Household.Where(u => u.Users.Any(i => i.Id == curMem.Id));
            var home = findUserhome.FirstOrDefault();
            var currUserTran = curMem.Transactions.ToList();

            //Display transaction by Category for current user


           var tod = DateTimeOffset.Now;
            decimal totalExpense = (from a in home.Accounts
                                    select a.Transactions.Sum(c => c.Amount)).DefaultIfEmpty().Sum();
            decimal totalBudget = (from a in home.Budgets
                                   select a.BudgetItems.Sum(c => c.Amount)).DefaultIfEmpty().Sum();
                                   
            var totalAcc = (from a in home.Accounts
                            select a.Balance).DefaultIfEmpty().Sum();


            var bar = (from c in home.Accounts.SelectMany(t => t.Transactions.GroupBy(i => i.Category.Name)).SelectMany(s => s.Select(c => c.Category))
                        let aSum = (from t in c.Transactions
                                    where t.Category.Name == c.Name
                                    select t.Amount).DefaultIfEmpty().Sum()
                        let bSum = (from b in c.Transactions.SelectMany(b => b.Category.BudgetItems)
                                    where b.Categories.Name == c.Name
                                    select b.Amount).DefaultIfEmpty().Sum()

                        select new
                        {
                            y = c.Name,
                            Actual = aSum,
                            Budgeted = bSum
                        }
                        ).ToArray();


            var result = new
            {
                totalAcc = totalAcc,
                totalBudget = totalBudget,
                totalExpense = totalExpense,
                bar = bar

              

            };
            ViewData["BudT"] = totalBudget.ToString();
            ViewData["AT"] = totalAcc.ToString();
            ViewData["TT"] = totalExpense.ToString();


            return Content(JsonConvert.SerializeObject(result), "application/json");
        }

        //        public ActionResult GetChart()
        //        {
        //            public SampleBarGroup()
        //        {
        //            Chart chart = this.Chart;
        //            AxisXY axis = new AxisXY(chart);

        //            double[][] y = new double[2][] {
        //           new double[] {4, 2, 3, 9},
        //           new double[] {6, 7, 5, 2}};
        //            Bar bar = new Bar(axis, y);
        //            bar.BarType = Bar.BAR_TYPE_VERTICAL;
        //            bar.SetLabels(new string[] { "A", "B", "C", "D" });
        //            bar.GetBarSet(0).FillColor = Color.Red;
        //            bar.GetBarSet(1).FillColor = Color.Blue;
        //        }

        //        public static void Main(string[] argv)
        //        {
        //            System.Windows.Forms.Application.Run(new SampleBarGroup());
        //        }
        //    }
        //}




        //        public ActionResult GetMonthly(string userid)
        //{
        //    var curMem = db.Users.Find(userid);
        //    var findUserhome = db.Household.Where(u => u.Users.Any(i => i.Id == curMem.Id));
        //    var home = findUserhome.FirstOrDefault();
        //    var currUserTran = curMem.Transactions.ToList();
        //    var monthsToDate = Enumerable.Range(1, DateTime.Today.Month)
        //        .Select(m => new DateTime(DateTime.Today.Year, m, 1))
        //        .ToList();
        //    var sums = from month in monthsToDate
        //               select new
        //               {
        //                   month = month.ToString("MMM"),

        //                   Actual = (from account in home.Category
        //                             from transaction in account.Transactions
        //                             where transaction.Type == "Actual" &&
        //                              transaction.Date.Month == month.Month
        //                             select transaction.Amount).DefaultIfEmpty().Sum(),
        //                   Budgeted = (from account in home.Category
        //                               from transaction in account.Transactions
        //                               where transaction.Type == "Budgeted" &&
        //                                transaction.Date.Month == month.Month
        //                               select transaction.Amount).DefaultIfEmpty().Sum(),

        //                   budget = db.Budgetitem.Select(b => b.Amount).DefaultIfEmpty().Sum(),

        //               };


        //    return Content(JsonConvert.SerializeObject(sums), "application/Json");
        //}

        public ActionResult Invite()
{

    return View();
}

[HttpPost]
public async Task<ActionResult> Invite(InviteViewModel model)
{

    var code = model.AccessCode;
    model.link = "sgichie-budgeter.azurewebsites.com";
    var Email = model.Email;
    model.inviteDate = DateTime.Now;



    // find current HouseHold
    var curMem = db.Users.Find(User.Identity.GetUserId());
    var findUserhome = db.Household.Where(u => u.Users.Any(i => i.Id == curMem.Id));
    var home = findUserhome.FirstOrDefault();
    var Invitee = db.Users.Where(e => e.Email == Email).FirstOrDefault();
    home.Invitee.Add(Invitee);
    home.inviteDate = model.inviteDate;
    db.SaveChanges();


    var svc = new EmailService();
    var msg = new IdentityMessage();
    msg.Subject = "Invite";
    msg.Body = "\r\n Please considering joing my household" + "Click is Link:" + model.link + ". Use the following code" + code;

    msg.Destination = Email;
    await svc.SendAsync(msg);
    //var helper = new AccountUpdatesHelper();
    //helper.Invite(Email, Link, code);

    return View();
}



public ActionResult AddtoHouse(InviteViewModel model, int id)
{
    var house = db.Household.Find(id);
    model.Id.Equals(id);

    return View(model);
}

[HttpPost]
public ActionResult AddtoHouse(InviteViewModel model)
{
    if (User.Identity.IsAuthenticated)
    {
        AccountUpdatesHelper membership = new AccountUpdatesHelper(db);
        var curMem = db.Users.Find(User.Identity.GetUserId());
        var Email = curMem.Email;
        var house = db.Household.Find(model.Id);
        house.AccessCode = model.AccessCode;
        if (membership.isInvited(model.Id, Email))
        {
            AccountUpdatesHelper helper = new AccountUpdatesHelper(db);
            helper.AddToHouseHolds(curMem.Id, model.Id, model.AccessCode);
            RedirectToAction("myHouse", "Households");
        }
    }


    return View("Index", "Households");
}

public ActionResult LeaveHouse(int houseID)
{
    if (ModelState.IsValid)
    {
        var user = db.Users.Find(User.Identity.GetUserId());
        AccountUpdatesHelper helper = new AccountUpdatesHelper(db);
        helper.LeaveHouseHolds(user.Id, houseID);
    }

    return View();
}

// GET: Households/Details/5
public ActionResult Details(int? id)
{
    if (id == null)
    {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
    }
    Households households = db.Household.Find(id);
    if (households == null)
    {
        return HttpNotFound();
    }
    return View(households);
}

// GET: Households/Create
public ActionResult Create()
{
    return View();
}

// POST: Households/Create
// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
[HttpPost]
[ValidateAntiForgeryToken]
public ActionResult Create([Bind(Include = "Id,Name")] Households households)
{
    if (ModelState.IsValid)
    {
        var headOfHome = db.Users.Find(User.Identity.GetUserId());
        households.Users.Add(headOfHome);
        db.Household.Add(households);
        db.SaveChanges();
        return RedirectToAction("Index");
    }

    return View(households);
}

// GET: Households/Edit/5
public ActionResult Edit(int? id)
{
    if (id == null)
    {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
    }
    Households households = db.Household.Find(id);
    if (households == null)
    {
        return HttpNotFound();
    }
    return View(households);
}

// POST: Households/Edit/5
// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
[HttpPost]
[ValidateAntiForgeryToken]
public ActionResult Edit([Bind(Include = "Id,Name")] Households households)
{
    if (ModelState.IsValid)
    {
        db.Entry(households).State = EntityState.Modified;
        db.SaveChanges();
        return RedirectToAction("Index");
    }
    return View(households);
}

// GET: Households/Delete/5
public ActionResult Delete(int? id)
{
    if (id == null)
    {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
    }
    Households households = db.Household.Find(id);
    if (households == null)
    {
        return HttpNotFound();
    }
    return View(households);
}

// POST: Households/Delete/5
[HttpPost, ActionName("Delete")]
[ValidateAntiForgeryToken]
public ActionResult DeleteConfirmed(int id)
{
    Households households = db.Household.Find(id);
    db.Household.Remove(households);
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
