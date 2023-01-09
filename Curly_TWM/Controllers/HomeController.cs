using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Curly_TWM.Domain.Entities;
using Curly_TWM.Infrastructure.DbaseContext;
using Curly_TWM.Infrastructure.Repositories;

namespace Curly_TWM.Controllers
{
    public class HomeController : Controller
    {
        private UnitOfWork unitfw = null;
        //private IGenericRepository<qualifications> unitfw33 = null;
        //-----------------------------------
        public HomeController()
        {
            unitfw = new UnitOfWork();
        }
        public HomeController(UnitOfWork _unitfw)
        {
            this.unitfw = _unitfw;
        }
        //-------------------%%%%%%%%%%%%%%%%%%%%%%----------------
        private TWMDB db = new TWMDB();
        public ActionResult Index()
        {
            Session["Style"] = "light";
            //just For you ! Again
            return View(unitfw.emp_main.Find(c =>(c.emp_arnm !="super")).ToList());
        }
        // GET: HR/Create
        public ActionResult Create()
     
        {    
            ViewBag.ActionName="ادخال موظف جديد";
            ViewBag.nat = new SelectList(unitfw.GenericRepos<countries>().GetAll(), "Id", "ar_name");
            ViewBag.Job_id = new SelectList(unitfw.Jobs.GetAll(), "Id", "JobName").ToList();
            ViewBag.Branch_id = new SelectList(unitfw.Branchs.GetAll(), "Id", "BranchsName").ToList();

            return View();
        }
        // POST: HR/Create
        [HttpPost]
        public ActionResult Create(emp_main collection)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    string button = Request.Form["submit"];
                    int cid = unitfw.emp_main.InsertEmp(collection);
                    //unitfw.Commit();


                    TempData["title"] = "Success Message ";
                    TempData["SuccessMsg"] = "تم الحفظ بنجاح";
                    //TempData["SuccessMsg"] = "تم التحديث بنجاح";
                    TempData["type"] = "success";
                    return RedirectToAction("Index");
             

                }
                ViewBag.nat = new SelectList(unitfw.GenericRepos<countries>().GetAll(), "Id", "ar_name");
                ViewBag.Job_id = new SelectList(unitfw.Jobs.GetAll(), "Id", "JobName").ToList();
                ViewBag.Branch_id = new SelectList(unitfw.Branchs.GetAll(), "Id", "BranchsName").ToList();

                return View(collection);
            }
            catch
            {
                return View(collection);
            }

        }
        // GET: HR/Create Doc
        public ActionResult CreateDoc(int id)
        {
            var ini = unitfw.emp_main.GetObjByID(id);
            ViewBag.empid = id;
                ViewBag.empName = ini.emp_arnm;

            ViewBag.Uploads = unitfw.GenericRepos<uploaddocs>().Find(n => n.empid == id).ToList();

            return View();
        }
        // POST: HR/Create Doc
        [HttpPost]
        public ActionResult CreateDoc(uploaddocs collection)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {

                    collection.doc_url = unitfw.GenericRepos<uploaddocs>().UploadFile(collection.doc_file, "Documents");
                    unitfw.GenericRepos<uploaddocs>().InsertObj(collection);
                    unitfw.Commit();

                    TempData["title"] = "Success Message ";
                    TempData["SuccessMsg"] = "تم الحفظ بنجاح";
                    //TempData["SuccessMsg"] = "تم التحديث بنجاح";
                    TempData["type"] = "success";
                    return RedirectToAction("CreateDoc");
                }
                ViewBag.empid = new SelectList(unitfw.emp_main.GetAll(), "Id", "emp_arnm").ToList();

                return View(collection);
            }
            catch
            {
                return View(collection);
            }

        }
    }
}