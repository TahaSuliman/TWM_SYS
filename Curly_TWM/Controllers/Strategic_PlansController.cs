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
    public class Strategic_PlansController : Controller
    {
        private UnitOfWork unitfw = null;
        //private IGenericRepository<qualifications> unitfw33 = null;
        //-----------------------------------
        public Strategic_PlansController()
        {
            unitfw = new UnitOfWork();
        }
        public Strategic_PlansController(UnitOfWork _unitfw)
        {
            this.unitfw = _unitfw;
        }
        //-------------------%%%%%%%%%%%%%%%%%%%%%%----------------
        private TWMDB db = new TWMDB();
        // قائمة الخطط الاستراتيجية
        public ActionResult Index()
        {
            return View(unitfw.Strategic_Plans.GetAll());
        }
        // اضافة الخطط الاستراتيجية
        public ActionResult Create()
        {
            ViewBag.ActionName = "اضافة  الخطط الاستراتيجية";
            return View();
        }
        [HttpPost]
        public ActionResult Create(Strategic_Plans collection)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    var y1 = collection.Plan_Sdt.Year.ToString();
                    var y2 = collection.Plan_Edt.Year.ToString();

                    collection.Plan_Title = y1 + "-" + y2;

                    unitfw.Strategic_Plans.InsertObj(collection);
                    unitfw.Commit();

                    TempData["title"] = "Success Message ";
                    TempData["SuccessMsg"] = "تم الحفظ بنجاح";
                    //TempData["SuccessMsg"] = "تم التحديث بنجاح";
                    TempData["type"] = "success";
                    return RedirectToAction("Index");
                }
                return View(collection);
            }
            catch
            {
                return View(collection);
            }

        }
        // قائمة الاهداف الاستراتيجية
        public ActionResult Strategic_Targets()
        {
            return View(unitfw.Strategic_Targets.GetAll().ToList());
        }
        // اضافة الاهداف الاستراتيجية
        public ActionResult CreateStrategic_Targets()
        {
            ViewBag.ActionName = "اضافة  الاهداف الاستراتيجية";

            ViewBag.planid = new SelectList(unitfw.Strategic_Plans.GetAll(), "Id", "Plan_Title").ToList();

            return View();
        }
        [HttpPost]
        public ActionResult CreateStrategic_Targets(Strategic_Targets collection)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {

                    unitfw.Strategic_Targets.InsertObj(collection);
                    unitfw.Commit();

                    TempData["title"] = "Success Message ";
                    TempData["SuccessMsg"] = "تم الحفظ بنجاح";
                    //TempData["SuccessMsg"] = "تم التحديث بنجاح";
                    TempData["type"] = "success";
                    return RedirectToAction("Strategic_Targets");
                }
                return View(collection);
            }
            catch
            {
                return View(collection);
            }

        }

    }
}