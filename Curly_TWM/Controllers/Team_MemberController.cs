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
    public class Team_MemberController : Controller
    {
        private UnitOfWork unitfw = null;
        //private IGenericRepository<qualifications> unitfw33 = null;
        //-----------------------------------
        public Team_MemberController()
        {
            unitfw = new UnitOfWork();
        }
        public Team_MemberController(UnitOfWork _unitfw)
        {
            this.unitfw = _unitfw;
        }
        //-------------------%%%%%%%%%%%%%%%%%%%%%%----------------
        private TWMDB db = new TWMDB();
        // GET: Team_Member
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Main()
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

            ViewBag.ActionName = "الرئيسية";

            List<string> Inits = new List<string>();

            //كل ارقام المبادرات
            var AllInits = unitfw.GenericRepos<Initiatives>().GetAll().ToList();
            for (int x = 0; x < AllInits.Count; x++)
            {
                Inits.Add(AllInits[x].Id.ToString());
            }

            //اجمالي المبادرات و المشاريع
            ViewBag.InitsCou = unitfw.GenericRepos<Teams>().Find(c => Inits.Contains(c.Initiativeid.ToString()) && c.empid == user.emp_Id).ToList().Count();
            ViewBag.InitsNames = unitfw.GenericRepos<Teams>().Find(c => Inits.Contains(c.Initiativeid.ToString()) && c.empid == user.emp_Id).ToList();

            //قيد التنفيذ
            ViewBag.InitsRunCou = unitfw.GenericRepos<Teams>().Find(c => Inits.Contains(c.Initiativeid.ToString()) && c.empid == user.emp_Id && c.Initiatives.InitiativeStat == "قيد التنفيذ").ToList().Count();

            //تم الانجاز
            ViewBag.InitsDoneCou = unitfw.GenericRepos<Teams>().Find(c => Inits.Contains(c.Initiativeid.ToString()) && c.empid == user.emp_Id && c.Initiatives.InitiativeStat == "تم الانجاز").ToList().Count();

            //قيد التخطيط
            ViewBag.InitsPlanCou = unitfw.GenericRepos<Teams>().Find(c => Inits.Contains(c.Initiativeid.ToString()) && c.empid == user.emp_Id && c.Initiatives.InitiativeStat == null).ToList().Count();

            //قائمة زملاء المبادرات
            List<Teamsviewmodel> xv = new List<Teamsviewmodel>();
            List<Teams> tm = unitfw.Teams.GetAll().ToList();

            foreach (Teams i in tm)
            {

                Initiatives obj = unitfw.GenericRepos<Initiatives>().SingleOrDefault(c => c.Id == i.Initiativeid);

                if (obj != null)
                {
                    //t = obj.doc_url;
                    Teamsviewmodel tmp = new Teamsviewmodel { P = i, FileId = obj.InitiativeName };
                    xv.Add(tmp);
                }
                else
                {

                    //break;
                    Teamsviewmodel tmp = new Teamsviewmodel { P = i, FileId = obj.InitiativeName };
                    xv.Add(tmp);
                }
            }
            ViewBag.initiParticipation = xv;



            var TeamsTb = unitfw.GenericRepos<Teams>().Find(c => (c.empid == user.emp_Id)).ToList();
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
            //return View(unitfw.Teams.Find(c => (c.Initiatives.emp_main.Id == user.emp_Id)).ToList());
            return View();

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


            var TeamsTb = unitfw.GenericRepos<Teams>().Find(c => (c.empid == user.emp_Id)).ToList();
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

            return View(TeamsTb);
        }
        //قائمة المهام
        public ActionResult TasksList(int id)
        {
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
            return View(unitfw.Team_Tasks.Find(c => (c.empid == id)).ToList());
        } 
        //قائمة زملاء الفريق
        public ActionResult MembersList(int id)
        {
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
            var init = unitfw.Initiatives.GetObjByID(id);
            ViewBag.TeamMan =init.emp_main.emp_arnm;
            ViewBag.TeamManDep =init.emp_main.Branchs.Sections.Departments.DepartmentName;
            var Manavatar = unitfw.GenericRepos<uploaddocs>().SingleOrDefault(c => (c.empid == init.empid) && (c.docu_type.Contains("الصورة الشخصية")));
            if (Manavatar != null)
            {
                ViewBag.TeamManImg = Manavatar.doc_url;
            }
            else
            {
                ViewBag.TeamManImg = "noimage.png";
            }
        

            return View(xv);
        }
        //تفاصيل المهمة
        public ActionResult TaskDetails(int id)
        {
            if ((string)Session["userId"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }

           
            Team_Tasks maindata = unitfw.Team_Tasks.GetObjByID(id);
            if (maindata == null)
            {
                return HttpNotFound();
            }
            return View(maindata);
        }
        //OnRunning
        public ActionResult TaskStat(int id)
        {
            //unitfw.Commit();

            var datareturn = unitfw.Team_Tasks.GetObjByID(id);
            datareturn.TaskStat = "قيد التنفيذ";
            unitfw.Team_Tasks.UpdateObj(datareturn);
            unitfw.Commit();
            TempData["title"] = "Success Message ";
            TempData["SuccessMsg"] = "تم التحديث بنجاح";
            TempData["type"] = "success";
            return RedirectToAction("TaskDetails" , new { id=datareturn.Id});
        }   
        //مرفقات المهمة
        public ActionResult InitiativesTasksUploas(int id ,int inT)
        {
            ViewBag.Uploads = unitfw.GenericRepos<InitiativesTasksUploas>().Find(n => n.TeamTasksid == id).ToList();
            ViewBag.Initiativeid = inT;
            ViewBag.TeamTasksid = id;
            var ini = unitfw.Initiatives.GetObjByID(inT);
            var tsk = unitfw.Team_Tasks.GetObjByID(id);
            ViewBag.InitiativeName = ini.InitiativeName;
            ViewBag.TeamTaskName = tsk.TaskName;
            ViewBag.empid = tsk.empid;
            return View();
        }
        //مرفقات المهمة
        [HttpPost]
        public ActionResult InitiativesTasksUploas(InitiativesTasksUploas collection)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {

                    collection.doc_url = unitfw.GenericRepos<InitiativesTasksUploas>().UploadFile(collection.doc_file, "Documents");
                    unitfw.GenericRepos<InitiativesTasksUploas>().InsertObj(collection);

                    var datareturn = unitfw.Team_Tasks.GetObjByID(collection.TeamTasksid);
                    datareturn.TaskStat = "تم التنفيذ";
                    unitfw.Team_Tasks.UpdateObj(datareturn);

                    unitfw.Commit();

                    TempData["title"] = "Success Message ";
                    TempData["SuccessMsg"] = "تم الحفظ بنجاح";
                    //TempData["SuccessMsg"] = "تم التحديث بنجاح";
                    TempData["type"] = "success";
                    return RedirectToAction("InitiativesTasksUploas", new { id = collection.TeamTasksid, inT= collection.Initiativeid });
                }
                ViewBag.Uploads = unitfw.GenericRepos<InitiativesTasksUploas>().Find(n => n.TeamTasksid == collection.TeamTasksid).ToList();

                return View(collection);
            }
            catch
            {
                return View(collection);
            }

        }
        


    }
}