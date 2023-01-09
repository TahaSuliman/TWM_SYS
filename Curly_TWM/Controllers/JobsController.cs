using Curly_TWM.Domain.Entities;
using Curly_TWM.Infrastructure.DbaseContext;
using Curly_TWM.Infrastructure.Repositories;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Curly_TWM.Controllers
{
    public class JobsController : Controller
    {
        private UnitOfWork unitfw = null;
        public JobsController()
        {
            unitfw = new UnitOfWork();
        }
        public JobsController(UnitOfWork _unitfw)
        {
            this.unitfw = _unitfw;
        }
        private TWMDB db = new TWMDB();
        // GET: HR
        public ActionResult Index()
        {
            return View(unitfw.Jobs.GetAll());
        }



        // GET: HR/Details/5
        public ActionResult Details(int id)
        {
            Jobs maindata = unitfw.Jobs.GetObjByID(id);
            if (maindata == null)
            {
                return HttpNotFound();
            }
            //// TODO: Add delete logic here
            return View(maindata);
        }

        // GET: HR/Create
        public ActionResult Create()
        {
            ViewBag.ActionName = "اضافة  مسمى وظيفي";
            return View();
        }

        // POST: HR/Create
        [HttpPost]
        public ActionResult Create(Jobs collection)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    unitfw.Jobs.InsertObj(collection);
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

        // GET: HR/Edit/5
        public ActionResult Edit(int id)
        {
            Jobs maindata = unitfw.Jobs.GetObjByID(id);
            if (maindata == null)
            {
                return HttpNotFound();
            }
            return View(maindata);
        }

        // POST: HR/Edit/5
        [HttpPost]
        public ActionResult Edit(Jobs maindata)
        {
            try
            {
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {

                    unitfw.Jobs.UpdateObj(maindata);
                    unitfw.Commit();

                    TempData["title"] = "Success Message ";
                    //TempData["SuccessMsg"] = "تم الحفظ بنجاح";
                    TempData["SuccessMsg"] = "تم التحديث بنجاح";
                    TempData["type"] = "success";

                    return RedirectToAction("Index");
                }
                return View(maindata);
            }
            catch
            {
                return View();
            }
        }

        // GET: HR/Delete/5
        [HttpPost]

        public ActionResult Delete(int id)
        {
            Jobs maindata = unitfw.Jobs.GetObjByID(id);
            if (maindata == null)
            {
                return HttpNotFound();
            }
            //// TODO: Add delete logic here
            unitfw.Jobs.DeleteObj(maindata);
            unitfw.Commit();
            return Json("ok");

        }


    }
}