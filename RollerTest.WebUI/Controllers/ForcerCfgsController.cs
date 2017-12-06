using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RollerTest.Domain.Concrete;
using RollerTest.Domain.Entities;
using RollerTest.WebUI.Models.PROCEDURE;

namespace RollerTest.WebUI.Controllers
{
    public class ForcerCfgsController : Controller
    {
        private EFDbContext db = new EFDbContext();

        // GET: ForcerCfgs
        public ActionResult Index()
        {
            return View(db.ForcerCfgs.ToList());
        }

        // GET: ForcerCfgs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ForcerCfg forcerCfg = db.ForcerCfgs.Find(id);
            if (forcerCfg == null)
            {
                return HttpNotFound();
            }
            return View(forcerCfg);
        }

        // GET: ForcerCfgs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ForcerCfgs/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ForcerName,ForcerMark,ForcerSwitch,ForcerUp,ForcerDn,ForcerSet")] ForcerCfg forcerCfg)
        {
            if (ModelState.IsValid)
            {
                db.ForcerCfgs.Add(forcerCfg);
                db.SaveChanges();
                Entities context = new Entities();
                context.PROCEDURE_FORCERCFG(0);
                context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(forcerCfg);
        }

        // GET: ForcerCfgs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ForcerCfg forcerCfg = db.ForcerCfgs.Find(id);
            if (forcerCfg == null)
            {
                return HttpNotFound();
            }
            return View(forcerCfg);
        }

        // POST: ForcerCfgs/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ForcerName,ForcerMark,ForcerSwitch,ForcerUp,ForcerDn,ForcerSet")] ForcerCfg forcerCfg)
        {
            if (ModelState.IsValid)
            {
                db.Entry(forcerCfg).State = EntityState.Modified;
                db.SaveChanges();
                Entities context = new Entities();
                context.PROCEDURE_FORCERCFG(0);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(forcerCfg);
        }

        // GET: ForcerCfgs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ForcerCfg forcerCfg = db.ForcerCfgs.Find(id);
            if (forcerCfg == null)
            {
                return HttpNotFound();
            }
            return View(forcerCfg);
        }

        // POST: ForcerCfgs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Entities context = new Entities();
            context.PROCEDURE_FORCERCFG(id);
            context.SaveChanges();
            ForcerCfg forcerCfg = db.ForcerCfgs.Find(id);
            db.ForcerCfgs.Remove(forcerCfg);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
