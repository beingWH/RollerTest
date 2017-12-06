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
    public class TimerCfgsController : Controller
    {
        private EFDbContext db = new EFDbContext();

        // GET: TimerCfgs
        public ActionResult Index()
        {
            return View(db.TimerCfgs.ToList());
        }

        // GET: TimerCfgs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TimerCfg timerCfg = db.TimerCfgs.Find(id);
            if (timerCfg == null)
            {
                return HttpNotFound();
            }
            return View(timerCfg);
        }

        // GET: TimerCfgs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TimerCfgs/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TimerName,TimerMark,TimerSwitch")] TimerCfg timerCfg)
        {
            if (ModelState.IsValid)
            {
                db.TimerCfgs.Add(timerCfg);
                db.SaveChanges();
                Entities context = new Entities();
                context.PROCEDURE_TIMERCFG(0);
                context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(timerCfg);
        }

        // GET: TimerCfgs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TimerCfg timerCfg = db.TimerCfgs.Find(id);
            if (timerCfg == null)
            {
                return HttpNotFound();
            }
            return View(timerCfg);
        }

        // POST: TimerCfgs/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TimerName,TimerMark,TimerSwitch")] TimerCfg timerCfg)
        {
            if (ModelState.IsValid)
            {
                db.Entry(timerCfg).State = EntityState.Modified;
                db.SaveChanges();
                Entities context = new Entities();
                context.PROCEDURE_TIMERCFG(0);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(timerCfg);
        }

        // GET: TimerCfgs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TimerCfg timerCfg = db.TimerCfgs.Find(id);
            if (timerCfg == null)
            {
                return HttpNotFound();
            }
            return View(timerCfg);
        }

        // POST: TimerCfgs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Entities context = new Entities();
            context.PROCEDURE_TIMERCFG(id);
            context.SaveChanges();
            TimerCfg timerCfg = db.TimerCfgs.Find(id);
            db.TimerCfgs.Remove(timerCfg);
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
