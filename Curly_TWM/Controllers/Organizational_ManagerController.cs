
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Curly_TWM.Domain.Entities;
using Curly_TWM.Infrastructure.DbaseContext;
using Curly_TWM.Infrastructure.Repositories;
using Curly_TWM.Models;

namespace Curly_TWM.Controllers
{
    public class Organizational_ManagerController : Controller
    {
        private UnitOfWork unitfw = null;
        //private IGenericRepository<qualifications> unitfw33 = null;
        //-----------------------------------
        public Organizational_ManagerController()
        {
            unitfw = new UnitOfWork();
        }
        public Organizational_ManagerController(UnitOfWork _unitfw)
        {
            this.unitfw = _unitfw;
        }
        //-------------------%%%%%%%%%%%%%%%%%%%%%%----------------
        private TWMDB db = new TWMDB();
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult Main()
        {
            ViewBag.ActionName = "متابعة الانجاز";

            //var userId = User.Identity.GetUserId();
            if ((string)Session["userId"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }

            string userId = (string)Session["userId"];
            var user = db.Users.Find(userId);
            Session["user"] = user.user_fullname;
            var emp_id = user.emp_Id;
            var empcode = unitfw.emp_main.SingleOrDefault(x => x.Id == emp_id);
            Session["empcode"] = empcode.emp_code;
            var jobtitle = unitfw.emp_main.SingleOrDefault(x => x.Id == emp_id);
            Session["jobtitle"] = jobtitle.Jobs.JobName;
            var avatar = unitfw.GenericRepos<uploaddocs>().SingleOrDefault(c => (c.empid == emp_id) && (c.docu_type.Contains("الصورة الشخصية")));
            if (avatar != null)
            {
                Session["Avatar"] = avatar.doc_url;
            }
            else
            {
                Session["Avatar"] = "noimage.png";
            }


            //اجمالي المبادرات و المشاريع
            ViewBag.InitsCou = unitfw.Initiatives.GetAll().ToList().Count();

            //قيد التنفيذ
            ViewBag.InitsRunCou = unitfw.Initiatives.Find(c => (c.InitiativeStat == "قيد التنفيذ")).ToList().Count();

            //تم الانجاز
            ViewBag.InitsDoneCou = unitfw.Initiatives.Find(c => (c.InitiativeStat == "تم الانجاز")).ToList().Count();

            //قيد التخطيط
            ViewBag.InitsPlanCou = unitfw.Initiatives.Find(c => (c.InitiativeStat == null)).ToList().Count();

            var TeamsTb = unitfw.GenericRepos<Teams>().GetAll().ToList();
            int z = 0;
            int n = 0;
            int e = 0;
            int d = 0;
            int o = 0;
            for (int x = 0; x < TeamsTb.Count; x++)
            {
                int s = 0;
                s = TeamsTb[x].Id;
                //عدد المهام
                TeamsTb[x].remarks = unitfw.GenericRepos<Team_Tasks>().Find(c => (c.empid == s)).Count().ToString();
                z = z + Convert.ToInt32(TeamsTb[x].remarks);
                //عدد المهام جديد
                TeamsTb[x].emp_main.remarks = unitfw.GenericRepos<Team_Tasks>().Find(c => (c.empid == s && c.TaskStat == null)).Count().ToString();
                n = n + Convert.ToInt32(TeamsTb[x].emp_main.remarks);
                //عدد المهام تم التنفيذ
                TeamsTb[x].emp_main.address = unitfw.GenericRepos<Team_Tasks>().Find(c => (c.empid == s && c.TaskStat == "قيد التنفيذ")).Count().ToString();
                e = e + Convert.ToInt32(TeamsTb[x].emp_main.address);
                //عدد المهام قيد التنفيذ
                TeamsTb[x].emp_main.emp_class = unitfw.GenericRepos<Team_Tasks>().Find(c => (c.empid == s && c.TaskStat == "تم التنفيذ")).Count().ToString();
                d = d + Convert.ToInt32(TeamsTb[x].emp_main.emp_class);
                //عدد المهام خارج الوقت
                TeamsTb[x].emp_main.emp_mobile = unitfw.GenericRepos<Team_Tasks>().Find(c => (c.empid == s && c.TaskStat == "خارج الوقت")).Count().ToString();
                o = o + Convert.ToInt32(TeamsTb[x].emp_main.emp_mobile);

            }
            ViewBag.taskCou = z;
            ViewBag.NtaskCou = n;
            ViewBag.EtaskCou = e;
            ViewBag.DtaskCou = d;
            ViewBag.OtaskCou = o;

            return View(TeamsTb);
        }
        //قائمة المبادرات
        public ActionResult Initiatives()
        {
            ViewBag.ActionName = "الرئيسية";
            //var userId = User.Identity.GetUserId();
            if ((string)Session["userId"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }

            string userId = (string)Session["userId"];
            var user = db.Users.Find(userId);
            Session["user"] = user.user_fullname;
            var emp_id = user.emp_Id;
            var empcode = unitfw.emp_main.SingleOrDefault(x => x.Id == emp_id);
            Session["empcode"] = empcode.emp_code;
            var jobtitle = unitfw.emp_main.SingleOrDefault(x => x.Id == emp_id);
            Session["jobtitle"] = jobtitle.Jobs.JobName;
            var avatar = unitfw.GenericRepos<uploaddocs>().SingleOrDefault(c => (c.empid == emp_id) && (c.docu_type.Contains("الصورة الشخصية")));
            if (avatar != null)
            {
                Session["Avatar"] = avatar.doc_url;
            }
            else
            {
                Session["Avatar"] = "noimage.png";
            }


            ViewBag.InitList = unitfw.GenericRepos<Initiatives>().GetAll().ToList();
            ViewBag.InitCount = unitfw.GenericRepos<Initiatives>().GetAll().ToList().Count();


            return View();

        }
        //تفاصيل المبادرات
        public ActionResult DetailsInitiatives(int id)
        {
            if ((string)Session["userId"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }

            
            Initiatives maindata = unitfw.Initiatives.GetObjByID(id);
            ViewBag.Uploads = unitfw.GenericRepos<InitiativesUploas>().Find(x => x.Initiativeid == id).ToList();

            var avatar = unitfw.GenericRepos<uploaddocs>().SingleOrDefault(c => (c.empid == maindata.empid) && (c.docu_type.Contains("الصورة الشخصية")));
            if (avatar != null)
            {
                Session["Avatar"] = avatar.doc_url;
            }
            else
            {
                Session["Avatar"] = "noimage.png";
            }

            var TeamsTb = unitfw.GenericRepos<Teams>().Find(c => (c.Initiatives.emp_main.Id == maindata.empid && c.Initiativeid == id)).ToList();
            int z = 0;
            int n = 0;
            int e = 0;
            int d = 0;
            int o = 0;
            for (int x = 0; x < TeamsTb.Count; x++)
            {
                int s = 0;
                s = TeamsTb[x].Id;
                //عدد المهام
                TeamsTb[x].remarks = unitfw.GenericRepos<Team_Tasks>().Find(c => (c.empid == s)).Count().ToString();
                z = z + Convert.ToInt32(TeamsTb[x].remarks);
                //عدد المهام جديد
                TeamsTb[x].emp_main.remarks = unitfw.GenericRepos<Team_Tasks>().Find(c => (c.empid == s && c.TaskStat == null)).Count().ToString();
                n = n + Convert.ToInt32(TeamsTb[x].emp_main.remarks);
                //عدد المهام تم التنفيذ
                TeamsTb[x].emp_main.address = unitfw.GenericRepos<Team_Tasks>().Find(c => (c.empid == s && c.TaskStat == "قيد التنفيذ")).Count().ToString();
                e = e + Convert.ToInt32(TeamsTb[x].emp_main.address);
                //عدد المهام قيد التنفيذ
                TeamsTb[x].emp_main.emp_class = unitfw.GenericRepos<Team_Tasks>().Find(c => (c.empid == s && c.TaskStat == "تم التنفيذ")).Count().ToString();
                d = d + Convert.ToInt32(TeamsTb[x].emp_main.emp_class);
                //عدد المهام خارج الوقت
                TeamsTb[x].emp_main.emp_mobile = unitfw.GenericRepos<Team_Tasks>().Find(c => (c.empid == s && c.TaskStat == "خارج الوقت")).Count().ToString();
                o = o + Convert.ToInt32(TeamsTb[x].emp_main.emp_mobile);

            }
            ViewBag.taskCou = z;
            ViewBag.NtaskCou = n;
            ViewBag.EtaskCou = e;
            ViewBag.DtaskCou = d;
            ViewBag.OtaskCou = o;
            //المهام
            ViewBag.Tasks = unitfw.Team_Tasks.Find(c => (c.Initiativeid == id)).ToList();
            //الفريق
            List<Empviewmodel> xv = new List<Empviewmodel>();
            List<Teams> em = unitfw.Teams.Find(c => (c.Initiativeid == id)).ToList();

            foreach (Teams i in em)
            {

                uploaddocs obj = unitfw.GenericRepos<uploaddocs>().SingleOrDefault(c => c.empid == i.emp_main.Id && (c.docu_type.Contains("الصورة الشخصية")));

                if (obj != null)
                {
                    //t = obj.doc_url;
                    Empviewmodel tmp = new Empviewmodel { P = i, FileId = obj.doc_url };
                    xv.Add(tmp);
                }
                else
                {

                    //break;
                    Empviewmodel tmp = new Empviewmodel { P = i, FileId = "noimage.png" };
                    xv.Add(tmp);
                }
            }

            ViewBag.tms = unitfw.Teams.Find(c => (c.Initiativeid == id)).ToList().Count();
            // الشركاء
            ViewBag.Partner = unitfw.Partners.Find(c => (c.Initiativeid == id)).ToList();
            // المخاطر
            ViewBag.Risk = unitfw.RiskList.Find(c => (c.Initiativeid == id)).ToList();
            // المؤشرات
            ViewBag.OperationalIndicator = unitfw.OperationalIndicators.Find(c => (c.Initiativeid == id)).ToList();
            if (maindata == null)
            {
                return HttpNotFound();
            }
            ViewBag.emp_arnm = maindata.emp_main.emp_arnm;
            ViewBag.Initiative_Sdt = maindata.Initiative_Sdt;
            ViewBag.Initiative_Edt = maindata.Initiative_Edt;
            ViewBag.InitiativeName = maindata.InitiativeName;
            ViewBag.InitiativeDeatails = maindata.InitiativeDeatails;
            ViewBag.TargetName = maindata.Strategic_Targets.TargetName;

            //المرفقات
            ViewBag.Iuploads = unitfw.GenericRepos<InitiativesUploas>().Find(cz => cz.Initiativeid == id).ToList();
            ViewBag.Tuploads = unitfw.GenericRepos<InitiativesTasksUploas>().Find(ax => ax.Initiativeid == id).ToList();

            return View(xv);
        }









    }
}