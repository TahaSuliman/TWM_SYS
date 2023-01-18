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
    public class Initiative_OfficerController : Controller
    {
        private UnitOfWork unitfw = null;

        //-----------------------------------
        public Initiative_OfficerController()
        {
            unitfw = new UnitOfWork();
        }
        public Initiative_OfficerController(UnitOfWork _unitfw)
        {
            this.unitfw = _unitfw;
        }
        //-------------------%%%%%%%%%%%%%%%%%%%%%%----------------
        private TWMDB db = new TWMDB();
        // GET: Initiative_Officer
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Main()
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

            ViewBag.ActionName = "الرئيسية";

            //اجمالي المبادرات و المشاريع
            ViewBag.InitsCou = unitfw.Initiatives.Find(c => (c.empid == user.emp_Id)).ToList().Count();

            //قيد التنفيذ
            ViewBag.InitsRunCou = unitfw.Initiatives.Find(c => (c.empid == user.emp_Id && c.InitiativeStat == "قيد التنفيذ")).ToList().Count();

            //تم الانجاز
            ViewBag.InitsDoneCou = unitfw.Initiatives.Find(c => (c.empid == user.emp_Id && c.InitiativeStat == "تم الانجاز")).ToList().Count();

            //قيد التخطيط
            ViewBag.InitsPlanCou = unitfw.Initiatives.Find(c => (c.empid == user.emp_Id && c.InitiativeStat == null)).ToList().Count();


            var TeamsTb = unitfw.GenericRepos<Teams>().Find(c => (c.Initiatives.emp_main.Id == user.emp_Id)).ToList();
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
                n= n + Convert.ToInt32(TeamsTb[x].emp_main.remarks);
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
            return View(TeamsTb);

        }
        //المبادرات 
        public ActionResult Initiatives()
        {
            //var userId = User.Identity.GetUserId();


            if ((string)Session["userId"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }

            string userId = (string)Session["userId"];


            var user = db.Users.Find(userId);

            var grpman = unitfw.GenericRepos<Initiatives>().Find(c => (c.empid == user.emp_Id)).ToList();
            int z = 0;
            int f = 0;
            for (int x = 0; x < grpman.Count; x++)
            {
                int s = 0;
                s = grpman[x].Id;
                grpman[x].remarks = unitfw.GenericRepos<Teams>().Find(c => (c.Initiativeid == s)).Count().ToString();
                z = z + Convert.ToInt32(grpman[x].remarks);
                //عدد المهام
                grpman[x].InitiativeDeatails = unitfw.GenericRepos<Team_Tasks>().Find(c => (c.Initiativeid == s)).Count().ToString();
                f = f + Convert.ToInt32(grpman[x].InitiativeDeatails);

            }

            ViewBag.myt = grpman;
            ViewBag.cou = unitfw.GenericRepos<Initiatives>().Find(c => (c.empid == user.emp_Id)).ToList().Count();

            return View();
        }
        // اضافة المبادرات 
        public ActionResult CreateInitiatives()
        {
            ViewBag.ActionName = "اضافة المبادرات";
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);

            ViewBag.empid = user.emp_Id;
            ViewBag.Targetid = new SelectList(unitfw.Strategic_Targets.GetAll(), "Id", "TargetName").ToList();

            return View();
        }
        [HttpPost]
        public ActionResult CreateInitiatives(Initiatives collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    unitfw.Initiatives.InsertObj(collection);
                    unitfw.Commit();

                    TempData["title"] = "Success Message ";
                    TempData["SuccessMsg"] = "تم الحفظ بنجاح";
                    //TempData["SuccessMsg"] = "تم التحديث بنجاح";
                    TempData["type"] = "success";
                    return RedirectToAction("Initiatives");
                }
                ViewBag.empid = new SelectList(unitfw.emp_main.Find(c => (c.emp_arnm != "super")), "Id", "emp_arnm").ToList();
                ViewBag.Targetid = new SelectList(unitfw.Strategic_Targets.GetAll(), "Id", "TargetName").ToList();
                return View(collection);
            }
            catch
            {
                return View(collection);
            }

        }
        //
        // تعديل المبادرات 
        public ActionResult EditInitiatives(int id)
        {

            Initiatives maindata = unitfw.Initiatives.GetObjByID(id);
            ViewBag.empid = maindata.empid;
            ViewBag.Targetid = new SelectList(unitfw.Strategic_Targets.GetAll(), "Id", "TargetName" , maindata.Targetid).ToList();
            if (maindata == null)
            {
                return HttpNotFound();
            }

            return View(maindata);
        }
        [HttpPost]
        public ActionResult EditInitiatives(Initiatives maindata)
        {
            try
            {
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    unitfw.Initiatives.UpdateObj(maindata);
                    unitfw.Commit();
                    TempData["title"] = "Success Message ";
                    TempData["SuccessMsg"] = "تم التحديث بنجاح";
                    TempData["type"] = "success";

                    return RedirectToAction("Initiatives");
                }

                ViewBag.empid = maindata.empid;
                ViewBag.Targetid = new SelectList(unitfw.Strategic_Targets.GetAll(), "Id", "TargetName").ToList();

                return View(maindata);
            }
            catch
            {
                return View();
            }
        }
        //مرفقات المبادرات
        public ActionResult InitiativesUploas(int id)
        {
            ViewBag.Uploads = unitfw.GenericRepos<InitiativesUploas>().Find(n => n.Initiativeid == id).ToList();
            ViewBag.Initiativeid = id;
            var ini = unitfw.Initiatives.GetObjByID(id);
            ViewBag.InitiativeName = ini.InitiativeName;
            return View();
        }
        //مرفقات المبادرات
        [HttpPost]
        public ActionResult InitiativesUploas(InitiativesUploas collection)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {

                    collection.doc_url = unitfw.GenericRepos<InitiativesUploas>().UploadFile(collection.doc_file, "Documents");
                    unitfw.GenericRepos<InitiativesUploas>().InsertObj(collection);
                    unitfw.Commit();

                    TempData["title"] = "Success Message ";
                    TempData["SuccessMsg"] = "تم الحفظ بنجاح";
                    //TempData["SuccessMsg"] = "تم التحديث بنجاح";
                    TempData["type"] = "success";
                    return RedirectToAction("InitiativesUploas" ,new { id=collection.Initiativeid});
                }
                //ViewBag.assignmentsId = new SelectList(unitfw.assignments.GetAll(), "Id", "Id").ToList();
                ViewBag.Uploads = unitfw.GenericRepos<InitiativesUploas>().Find(n => n.Initiativeid == collection.Initiativeid).ToList();

                return View(collection);
            }
            catch
            {
                return View(collection);
            }

        }
        //تفاصيل المبادرات
        public ActionResult DetailsInitiatives(int id)
        {
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

            var TeamsTb = unitfw.GenericRepos<Teams>().Find(c => (c.Initiatives.emp_main.Id == maindata.empid && c.Initiativeid==id)).ToList();
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
        //فرق العمل
        public ActionResult Teams(int iId)
        {
            //var userId = User.Identity.GetUserId();

            if ((string)Session["userId"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }

            string userId = (string)Session["userId"];


            var user = db.Users.Find(userId);

            var iNm = unitfw.Initiatives.SingleOrDefault(c => (c.Id == iId));

            ViewBag.Initiativeid = iId;
            ViewBag.InitiativeName = iNm.InitiativeName;

            return View(unitfw.Teams.Find(c => (c.Initiativeid== iId)).ToList());

            //return Content(xv.ToString());

        }
        // اضافة فرق العمل 
        public ActionResult CreateTeams(int iId)
        {
            ViewBag.ActionName = "اضافة فرق العمل";
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);

            var iNm = unitfw.Initiatives.SingleOrDefault(c => (c.Id == iId));

            ViewBag.empid = new SelectList(unitfw.emp_main.Find(c => (c.emp_arnm != "super")), "Id", "emp_arnm").ToList();
            ViewBag.Initiativeid = iId;
            ViewBag.InitiativeName = iNm.InitiativeName;

            return View();
        }
        [HttpPost]
        public ActionResult CreateTeams(Teams collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    unitfw.Teams.InsertObj(collection);
                    unitfw.Commit();

                    TempData["title"] = "Success Message ";
                    TempData["SuccessMsg"] = "تم الحفظ بنجاح";
                    //TempData["SuccessMsg"] = "تم التحديث بنجاح";
                    TempData["type"] = "success";
                    return RedirectToAction("Initiatives");
                }
                var userId = User.Identity.GetUserId();
                var user = db.Users.Find(userId);
                
                ViewBag.empid = new SelectList(unitfw.emp_main.Find(c => (c.emp_arnm != "super")), "Id", "emp_arnm").ToList();
                ViewBag.Initiativeid = collection.Initiativeid;
                return View(collection);
            }
            catch
            {
                return View(collection);
            }

        }
        //ادارة مهام الفريق
        public ActionResult ExecutePlan(int iId)
        {
            //var userId = User.Identity.GetUserId();

            if ((string)Session["userId"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }

            string userId = (string)Session["userId"];

            var user = db.Users.Find(userId);
            //Team_Tasks
            var iNm = unitfw.Initiatives.SingleOrDefault(c => (c.Id == iId));
            ViewBag.Initiativeid = iId;
            ViewBag.InitiativeName = iNm.InitiativeName;
            ViewBag.Team = unitfw.Teams.Find(c => (c.Initiativeid == iId)).ToList();
            return View(unitfw.Team_Tasks.Find(c => (c.Initiativeid == iId)).ToList());
        }
        // اضافة فرق العمل 
        public ActionResult CreateTask(int iId,int TmId)
        {
            ViewBag.ActionName = "اضافة مهام";
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);

            var iNm = unitfw.Initiatives.SingleOrDefault(c => (c.Id == iId));
            ViewBag.TmId = unitfw.Team_Tasks.Find(c => (c.empid == TmId)).ToList();

            ViewBag.empid = TmId;
            ViewBag.Initiativeid = iId;
            ViewBag.InitiativeName = iNm.InitiativeName;

            return View();
        }
        [HttpPost]
        public ActionResult CreateTask(Team_Tasks collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    unitfw.Team_Tasks.InsertObj(collection);
                    unitfw.Commit();

                    TempData["title"] = "Success Message ";
                    TempData["SuccessMsg"] = "تم الحفظ بنجاح";
                    //TempData["SuccessMsg"] = "تم التحديث بنجاح";
                    TempData["type"] = "success";
                    return RedirectToAction("ExecutePlan" ,new { iId=collection.Initiativeid });
                }

                ViewBag.empid = collection.empid;
                ViewBag.Initiativeid = collection.Initiativeid;
                return View(collection);
            }
            catch
            {
                return View(collection);
            }

        }  
        
        // الاطلاع على اعضاء فرق العمل 
        public ActionResult MemberDetails(int iId,int TmId)
        {
            ViewBag.ActionName = "اضافة مهام";
            //var userId = User.Identity.GetUserId();

            if ((string)Session["userId"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }

            string userId = (string)Session["userId"];

            var user = db.Users.Find(userId);

            var iNm = unitfw.Initiatives.SingleOrDefault(c => (c.Id == iId));


            ViewBag.empid = TmId;
            ViewBag.Initiativeid = iId;
            ViewBag.InitiativeName = iNm.InitiativeName;


            return View(unitfw.Team_Tasks.Find(c => (c.empid == TmId)).ToList());
        }
        //الشركاء
        public ActionResult Partners(int iId)
        {
            //var userId = User.Identity.GetUserId();

            if ((string)Session["userId"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }

            string userId = (string)Session["userId"];

            var user = db.Users.Find(userId);
            //Team_Tasks
            var iNm = unitfw.Initiatives.SingleOrDefault(c => (c.Id == iId));
            ViewBag.Initiativeid = iId;
            ViewBag.InitiativeName = iNm.InitiativeName;
            return View(unitfw.Partners.Find(c => (c.Initiativeid == iId)).ToList());
        }
        // اضافة شريك 
        public ActionResult CreatePartner(int iId)
        {
            ViewBag.ActionName = "اضافة شريك للمبادرة";
            //var userId = User.Identity.GetUserId();

            if ((string)Session["userId"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }

            string userId = (string)Session["userId"];

            var user = db.Users.Find(userId);

            var iNm = unitfw.Initiatives.SingleOrDefault(c => (c.Id == iId));

            ViewBag.Initiativeid = iId;
            ViewBag.InitiativeName = iNm.InitiativeName;

            return View();
        }
        [HttpPost]
        public ActionResult CreatePartner(Partners collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    unitfw.Partners.InsertObj(collection);
                    unitfw.Commit();

                    TempData["title"] = "Success Message ";
                    TempData["SuccessMsg"] = "تم الحفظ بنجاح";
                    //TempData["SuccessMsg"] = "تم التحديث بنجاح";
                    TempData["type"] = "success";
                    return RedirectToAction("Partners", new { iId = collection.Initiativeid });
                }

                ViewBag.Initiativeid = collection.Initiativeid;
                return View(collection);
            }
            catch
            {
                return View(collection);
            }

        }
        //المخاطر
        public ActionResult RiskList(int iId)
        {
            //var userId = User.Identity.GetUserId();

            if ((string)Session["userId"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }

            string userId = (string)Session["userId"];

            var user = db.Users.Find(userId);
            //Team_Tasks
            var iNm = unitfw.Initiatives.SingleOrDefault(c => (c.Id == iId));
            ViewBag.Initiativeid = iId;
            ViewBag.InitiativeName = iNm.InitiativeName;
            return View(unitfw.RiskList.Find(c => (c.Initiativeid == iId)).ToList());
        }
        // اضافة مخاطر 
        public ActionResult CreateRisk(int iId)
        {
            ViewBag.ActionName = "اضافة مخاطر للمبادرة";
            //var userId = User.Identity.GetUserId();

            if ((string)Session["userId"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }

            string userId = (string)Session["userId"];


            var user = db.Users.Find(userId);

            var iNm = unitfw.Initiatives.SingleOrDefault(c => (c.Id == iId));

            ViewBag.Initiativeid = iId;
            ViewBag.InitiativeName = iNm.InitiativeName;

            return View();
        }
        [HttpPost]
        public ActionResult CreateRisk(RiskList collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    unitfw.RiskList.InsertObj(collection);
                    unitfw.Commit();

                    TempData["title"] = "Success Message ";
                    TempData["SuccessMsg"] = "تم الحفظ بنجاح";
                    //TempData["SuccessMsg"] = "تم التحديث بنجاح";
                    TempData["type"] = "success";
                    return RedirectToAction("RiskList", new { iId = collection.Initiativeid });
                }

                ViewBag.Initiativeid = collection.Initiativeid;
                return View(collection);
            }
            catch
            {
                return View(collection);
            }

        }
        //المؤشرات
        public ActionResult OperationalIndicators(int iId)
        {
            //var userId = User.Identity.GetUserId();

            if ((string)Session["userId"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }

            string userId = (string)Session["userId"];

            var user = db.Users.Find(userId);
            //Team_Tasks
            var iNm = unitfw.Initiatives.SingleOrDefault(c => (c.Id == iId));
            ViewBag.Initiativeid = iId;
            ViewBag.InitiativeName = iNm.InitiativeName;
            return View(unitfw.OperationalIndicators.Find(c => (c.Initiativeid == iId)).ToList());
        }
        // اضافة المؤشرات 
        public ActionResult CreateIndicator(int iId)
        {
            ViewBag.ActionName = "اضافة المؤشرات";
            //var userId = User.Identity.GetUserId();

            if ((string)Session["userId"] == null)
            {
                return RedirectToAction("LogOff", "Account");
            }

            string userId = (string)Session["userId"];

            var user = db.Users.Find(userId);

            var iNm = unitfw.Initiatives.SingleOrDefault(c => (c.Id == iId));

            ViewBag.Initiativeid = iId;
            ViewBag.InitiativeName = iNm.InitiativeName;

            return View();
        }
        [HttpPost]
        public ActionResult CreateIndicator(OperationalIndicators collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    unitfw.OperationalIndicators.InsertObj(collection);
                    unitfw.Commit();

                    TempData["title"] = "Success Message ";
                    TempData["SuccessMsg"] = "تم الحفظ بنجاح";
                    //TempData["SuccessMsg"] = "تم التحديث بنجاح";
                    TempData["type"] = "success";
                    return RedirectToAction("OperationalIndicators", new { iId = collection.Initiativeid });
                }

                ViewBag.Initiativeid = collection.Initiativeid;
                return View(collection);
            }
            catch
            {
                return View(collection);
            }

        }
        
    }
    
}