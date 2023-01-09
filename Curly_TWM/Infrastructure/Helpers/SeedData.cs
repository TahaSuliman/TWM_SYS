using Curly_TWM.Domain.Entities;
using Curly_TWM.Infrastructure.DbaseContext;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Curly_TWM.Infrastructure.Helpers
{
    public class SeedData
    {

        internal static void SeedIdentities(TWMDB context)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            AppRole roles = new AppRole();
            AppRole Initiative_Officer = new AppRole();
            AppRole System_Manager = new AppRole();
            AppRole Team_Member = new AppRole();
            AppRole Organizational_Manager = new AppRole();


            //مسؤول الدعم
            roles.Name = "Admin";
            roles.RoleOwner = "Admin";
            //مسؤول المبادرة / المشروع
            Initiative_Officer.Name = "Initiative_Officer";
            Initiative_Officer.RoleOwner = "Initiative_Officer";
            //مدير المؤسسة
            Organizational_Manager.Name = "Organizational_Manager";
            Organizational_Manager.RoleOwner = "Organizational_Manager";
            //مدير النظام
            System_Manager.Name = "System_Manager";
            System_Manager.RoleOwner = "System_Manager";
            //عضو الفريق
            Team_Member.Name = "Team_Member";
            Team_Member.RoleOwner = "Team_Member";


            if (!roleManager.RoleExists("Admin"))
            {
                roleManager.Create(roles);
            }
            if (!roleManager.RoleExists("Initiative_Officer"))
            {
                roleManager.Create(Initiative_Officer);
            }
            if (!roleManager.RoleExists("System_Manager"))
            {
                roleManager.Create(System_Manager);
            }
            if (!roleManager.RoleExists("Team_Member"))
            {
                roleManager.Create(Team_Member);
            }   
 
            if (!roleManager.RoleExists("Organizational_Manager"))
            {
                roleManager.Create(Organizational_Manager);
            }
       


            if (!context.Users.Any(u => u.UserName == "super"))
            {
                emp_main emp = new emp_main();

                emp.emp_arnm = "super";
                //emp.nat = 1;
                emp.gender_Id = "M";

                //emp.Job_id = 1;

                emp.emp_code = 0;
                emp.emp_mobile = "0502084946";
                emp.remarks = "";
                emp.emp_class = "Initiative_Officer";
                context.emp_main.Add(emp);
                context.SaveChanges();

                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser { UserName = "super", Email = "super@gmail.com", emp_Id =emp.Id ,user_fullname= "super" };
                manager.Create(user, "abc123");
                manager.AddToRole(user.Id, "Admin");
                manager.AddToRole(user.Id, "Initiative_Officer");
                manager.AddToRole(user.Id, "Organizational_Manager");
                manager.AddToRole(user.Id, "System_Manager");
                manager.AddToRole(user.Id, "Team_Member");
            }



        }

    }
}