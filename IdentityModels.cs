using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace sg_budgeter.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        //public string Role { get; set; }

        public ApplicationUser()
        {
            //Budgets = new HashSet<Budgets>();
            //Accounts = new HashSet<Accounts>();
            Transactions = new HashSet<Transactions>();
            //BudgetItems = new HashSet<BudgetItems>();

            //ApplicationUser = new HashSet<ApplicationUser>();

        }

        //public virtual ICollection<Budgets> Budgets { get; set; }
        //public virtual ICollection<Accounts> Accounts { get; set; }
        public virtual ICollection<Transactions> Transactions { get; set; }
        //public virtual ICollection<BudgetItems> BudgetItems { get; set; }
        //public virtual ICollection<ApplicationUser> ApplicationUser { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Budgets> Budget { get; set; }
        public DbSet<BudgetItems> Budgetitem{ get; set; }
        public DbSet<Categories> Category { get; set; }
        public DbSet<Accounts> Account { get; set; }
        public DbSet<Households> Household { get; set; }
        public DbSet<Transactions> Transaction { get; set; }
        
    }
}