using Curly_TWM.Domain.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace Curly_TWM.Infrastructure.DbaseContext
{
    public class TWMDB : IdentityDbContext<ApplicationUser>
    {
        public TWMDB() : base("dbconnection")
        {

        }
        public static TWMDB Create()
        {
            return new TWMDB();
        }
        public DbSet<emp_main> emp_main { get; set; }
        public DbSet<uploaddocs> uploaddocs { get; set; }
        public DbSet<Jobs> Jobs { get; set; }
        public DbSet<Branchs> Branchs { get; set; }
        public DbSet<Sections> Sections { get; set; }
        public DbSet<Departments> Departments { get; set; }
        public DbSet<MainDepartments> MainDepartments { get; set; }
        public DbSet<countries> countries { get; set; }
        public DbSet<Strategic_Plans> Strategic_Plans { get; set; }
        public DbSet<Strategic_Targets> Strategic_Targets { get; set; }
        public DbSet<Initiatives> Initiatives { get; set; }
        public DbSet<Teams> Teams { get; set; }
        public DbSet<Team_Tasks> Team_Tasks { get; set; }
        public DbSet<InitiativesUploas> InitiativesUploas { get; set; }
        public DbSet<Partners> Partners { get; set; }
        public DbSet<RiskList> RiskList { get; set; }
        public DbSet<OperationalIndicators> OperationalIndicators { get; set; }
        public DbSet<InitiativesTasksUploas> InitiativesTasksUploas { get; set; }


    }

    //**************************************************
    public class ApplicationUser : IdentityUser
    {
        //add your custom properties which have not included in IdentityUser before
        [ForeignKey("emp_main")]
        [Display(Name = "الموظف")]
        public int emp_Id { get; set; }
        public virtual emp_main emp_main { get; set; }
        public string user_fullname { get; set; }
        public string Avatar { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
    //public class AppUserManager : UserManager<ApplicationUserManager>
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        // this method is called by Owin therefore best place to configure your User Manager
        public static ApplicationUserManager Create(
            IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(
                new UserStore<ApplicationUser>(context.Get<TWMDB>()));

            // optionally configure your manager
            // ...

            return manager;
        }
    }
    //*************************************************

    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
    //*************************************************

    //----------------@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@------------------
    public class AppRole : IdentityRole
    {
        public string RoleOwner { get; set; }

        public AppRole() : base() { }
        public AppRole(string name) : base(name) { }
        // extra properties here 
    }

    public class ApplicationRoleManager : RoleManager<AppRole>
    {
        public ApplicationRoleManager(RoleStore<AppRole> store) : base(store) { }

        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            return new ApplicationRoleManager(new RoleStore<AppRole>(context.Get<TWMDB>()));
        }
    }

    //----------------@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@------------------
}