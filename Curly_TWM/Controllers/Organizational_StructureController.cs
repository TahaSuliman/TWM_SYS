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
    public class Organizational_StructureController : Controller
    {
        private UnitOfWork unitfw = null;
        //private IGenericRepository<qualifications> unitfw33 = null;
        //-----------------------------------
        public Organizational_StructureController()
        {
            unitfw = new UnitOfWork();
        }
        public Organizational_StructureController(UnitOfWork _unitfw)
        {
            this.unitfw = _unitfw;
        }
        //-------------------%%%%%%%%%%%%%%%%%%%%%%----------------
        private TWMDB db = new TWMDB();
        // قائمة الادارات العامة
        public ActionResult MainDepartments()
        {
            ViewBag.Departments = unitfw.Departments.GetAll().ToList().Count();
            ViewBag.Sections = unitfw.Sections.GetAll().ToList().Count();
            ViewBag.Branchs = unitfw.Branchs.GetAll().ToList().Count();
            return View(unitfw.MainDepartments.GetAll());
        }
        // اضافة الادارات العامة
        public ActionResult Create()
        {
            ViewBag.ActionName = "اضافة الادارات العامة";
            ViewBag.empid = new SelectList(unitfw.emp_main.Find(c => (c.emp_arnm != "super")), "Id", "emp_arnm").ToList();

            return View();
        }
        [HttpPost]
        public ActionResult Create(MainDepartments collection)
        {
            try
            {
   
                if (ModelState.IsValid)
                {
                    unitfw.MainDepartments.InsertObj(collection);
                    unitfw.Commit();

                    TempData["title"] = "Success Message ";
                    TempData["SuccessMsg"] = "تم الحفظ بنجاح";
                    //TempData["SuccessMsg"] = "تم التحديث بنجاح";
                    TempData["type"] = "success";
                    return RedirectToAction("MainDepartments");
                }
                ViewBag.empid = new SelectList(unitfw.emp_main.Find(c => (c.emp_arnm != "super")), "Id", "emp_arnm").ToList();


                return View(collection);
            }
            catch
            {
                return View(collection);
            }

        }
        // قائمة الادارات
        public ActionResult Departments()
        {
            ViewBag.MainDepartments = unitfw.MainDepartments.GetAll().ToList().Count();
            ViewBag.Sections = unitfw.Sections.GetAll().ToList().Count();
            ViewBag.Branchs = unitfw.Branchs.GetAll().ToList().Count();
            return View(unitfw.Departments.GetAll());
        }
        // اضافة الادارات 
        public ActionResult CreateDepartments()
        {
            ViewBag.ActionName = "اضافة الادارات";
            ViewBag.empid = new SelectList(unitfw.emp_main.Find(c => (c.emp_arnm != "super")), "Id", "emp_arnm").ToList();
            ViewBag.MainDepartment_id = new SelectList(unitfw.MainDepartments.GetAll(), "Id", "MainDepartmentName").ToList();

            return View();
        }
        [HttpPost]
        public ActionResult CreateDepartments(Departments collection)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    unitfw.Departments.InsertObj(collection);
                    unitfw.Commit();

                    TempData["title"] = "Success Message ";
                    TempData["SuccessMsg"] = "تم الحفظ بنجاح";
                    //TempData["SuccessMsg"] = "تم التحديث بنجاح";
                    TempData["type"] = "success";
                    return RedirectToAction("Departments");
                }
                ViewBag.empid = new SelectList(unitfw.emp_main.Find(c => (c.emp_arnm != "super")), "Id", "emp_arnm").ToList();
                ViewBag.MainDepartment_id = new SelectList(unitfw.MainDepartments.GetAll(), "Id", "MainDepartmentName").ToList();

                return View(collection);
            }
            catch
            {
                return View(collection);
            }

        }

        // قائمة الاقسام
        public ActionResult Sections()
        {
            ViewBag.Departments = unitfw.Departments.GetAll().ToList().Count();
            ViewBag.MainDepartments = unitfw.MainDepartments.GetAll().ToList().Count();
            ViewBag.Branchs = unitfw.Branchs.GetAll().ToList().Count();
            return View(unitfw.Sections.GetAll());
        }
        // اضافة الاقسام 
        public ActionResult CreateSections()
        {
            ViewBag.ActionName = "اضافة الاقسام";
            ViewBag.empid = new SelectList(unitfw.emp_main.Find(c => (c.emp_arnm != "super")), "Id", "emp_arnm").ToList();
            ViewBag.Department_id = new SelectList(unitfw.Departments.GetAll(), "Id", "DepartmentName").ToList();

            return View();
        }
        [HttpPost]
        public ActionResult CreateSections(Sections collection)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    unitfw.Sections.InsertObj(collection);
                    unitfw.Commit();

                    TempData["title"] = "Success Message ";
                    TempData["SuccessMsg"] = "تم الحفظ بنجاح";
                    //TempData["SuccessMsg"] = "تم التحديث بنجاح";
                    TempData["type"] = "success";
                    return RedirectToAction("Sections");
                }
                ViewBag.empid = new SelectList(unitfw.emp_main.Find(c => (c.emp_arnm != "super")), "Id", "emp_arnm").ToList();
                ViewBag.Department_id = new SelectList(unitfw.Departments.GetAll(), "Id", "DepartmentName").ToList();
                
                return View(collection);
            }
            catch
            {
                return View(collection);
            }

        }

        // قائمة الفروع
        public ActionResult Branchs()
        {
            ViewBag.Departments = unitfw.Departments.GetAll().ToList().Count();
            ViewBag.Sections = unitfw.Sections.GetAll().ToList().Count();
            ViewBag.MainDepartments = unitfw.MainDepartments.GetAll().ToList().Count();

            List<xviewmodel> xv = new List<xviewmodel>();
            List<Branchs> n = unitfw.Branchs.GetAll().ToList();

            foreach (Branchs i in n)
            {

                emp_main obj = unitfw.GenericRepos<emp_main>().SingleOrDefault(c => c.Id == i.empid);

                if (obj != null)
                {
                    //t = obj.doc_url;
                    xviewmodel tmp = new xviewmodel { P = i, FileId = obj.emp_arnm };
                    xv.Add(tmp);
                }
                else
                {

                    //break;
                    xviewmodel tmp = new xviewmodel { P = i, FileId = obj.emp_arnm };
                    xv.Add(tmp);
                }
            }



            return View(xv);
        }
        // اضافة الفروع 
        public ActionResult CreateBranchs()
        {
            ViewBag.ActionName = "اضافة الفروع";
            ViewBag.empid = new SelectList(unitfw.emp_main.Find(c => (c.emp_arnm != "super")), "Id", "emp_arnm").ToList();
            ViewBag.Section_id = new SelectList(unitfw.Sections.GetAll(), "Id", "SectiontName").ToList();

            return View();
        }
        [HttpPost]
        public ActionResult CreateBranchs(Branchs collection)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    unitfw.Branchs.InsertObj(collection);
                    unitfw.Commit();

                    TempData["title"] = "Success Message ";
                    TempData["SuccessMsg"] = "تم الحفظ بنجاح";
                    //TempData["SuccessMsg"] = "تم التحديث بنجاح";
                    TempData["type"] = "success";
                    return RedirectToAction("Branchs");
                }
                ViewBag.empid = new SelectList(unitfw.emp_main.Find(c => (c.emp_arnm != "super")), "Id", "emp_arnm").ToList();
                ViewBag.Section_id = new SelectList(unitfw.Sections.GetAll(), "Id", "SectiontName").ToList();

                return View(collection);
            }
            catch
            {
                return View(collection);
            }

        }



    }
}