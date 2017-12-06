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
using RollerTest.WebUI.Models;
using RollerTest.Domain.Abstract;
using RollerTest.WebUI.Models.WTTESTMODEL;
using RollerTest.WebUI.Models.PROCEDURE;

namespace RollerTest.WebUI.Controllers
{
    public class RollerBaseStationsController : Controller
    {
        private EFDbContext db = new EFDbContext();
        private IBaseRepository baserepository;
        private IReadRepository<WTTESTEQUIPMENT> wtequipmentrepo;
        private IReadRepository<WTSAMPLEINFO> wtsampleinfo;

        public RollerBaseStationsController(IBaseRepository baserepository, IReadRepository<WTTESTEQUIPMENT> wtequipmentrepo, IReadRepository<WTSAMPLEINFO> wtsampleinfo)
        {
            this.baserepository = baserepository;
            this.wtequipmentrepo = wtequipmentrepo;
            this.wtsampleinfo = wtsampleinfo;
        }


        // GET: RollerBaseStations
        public ActionResult Index()
        {
            var rollerBaseStations = db.RollerBaseStations.Include(r => r.ForcerCfg).Include(r => r.TimerCfg);
            return View(rollerBaseStations.ToList());
        }

        // GET: RollerBaseStations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RollerBaseStation rollerBaseStation = db.RollerBaseStations.Find(id);
            if (rollerBaseStation == null)
            {
                return HttpNotFound();
            }
            return View(rollerBaseStation);
        }

        // GET: RollerBaseStations/Create
        public ActionResult Create()
        {
            SettingViewModel settingviewModel = new SettingViewModel(baserepository, wtequipmentrepo, wtsampleinfo);
            ViewData["DeviceList"] = settingviewModel.GetLISDeviceList();
            ViewBag.ForcerCfgId = new SelectList(db.ForcerCfgs, "Id", "ForcerName");
            ViewBag.TimerCfgId = new SelectList(db.TimerCfgs, "Id", "TimerName");
            return View();
        }

        // POST: RollerBaseStations/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RollerBaseStationID,Device,Station,Mark,State,TimerCfgId,ForcerCfgId")] RollerBaseStation rollerBaseStation)
        {
            if (ModelState.IsValid)
            {
                db.RollerBaseStations.Add(rollerBaseStation);
                db.SaveChanges();
                Entities context = new Entities();
                context.PROCEDURE_ROLLERBASESTATION(0);
                context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ForcerCfgId = new SelectList(db.ForcerCfgs, "Id", "ForcerName", rollerBaseStation.ForcerCfgId);
            ViewBag.TimerCfgId = new SelectList(db.TimerCfgs, "Id", "TimerName", rollerBaseStation.TimerCfgId);
            return View(rollerBaseStation);
        }

        // GET: RollerBaseStations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RollerBaseStation rollerBaseStation = db.RollerBaseStations.Find(id);
            if (rollerBaseStation == null)
            {
                return HttpNotFound();
            }
            SettingViewModel settingviewModel = new SettingViewModel(baserepository, wtequipmentrepo, wtsampleinfo);
            ViewData["DeviceList"] = settingviewModel.GetLISDeviceList();
            ViewBag.ForcerCfgId = new SelectList(db.ForcerCfgs, "Id", "ForcerName", rollerBaseStation.ForcerCfgId);
            ViewBag.TimerCfgId = new SelectList(db.TimerCfgs, "Id", "TimerName", rollerBaseStation.TimerCfgId);
            return View(rollerBaseStation);
        }

        // POST: RollerBaseStations/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RollerBaseStationID,Device,Station,Mark,State,TimerCfgId,ForcerCfgId")] RollerBaseStation rollerBaseStation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rollerBaseStation).State = EntityState.Modified;
                db.SaveChanges();
                Entities context = new Entities();
                context.PROCEDURE_ROLLERBASESTATION(0);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ForcerCfgId = new SelectList(db.ForcerCfgs, "Id", "ForcerName", rollerBaseStation.ForcerCfgId);
            ViewBag.TimerCfgId = new SelectList(db.TimerCfgs, "Id", "TimerName", rollerBaseStation.TimerCfgId);
            return View(rollerBaseStation);
        }

        // GET: RollerBaseStations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RollerBaseStation rollerBaseStation = db.RollerBaseStations.Find(id);
            if (rollerBaseStation == null)
            {
                return HttpNotFound();
            }
            return View(rollerBaseStation);
        }

        // POST: RollerBaseStations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Entities context = new Entities();
            context.PROCEDURE_ROLLERBASESTATION(id);
            context.SaveChanges();
            RollerBaseStation rollerBaseStation = db.RollerBaseStations.Find(id);
            db.RollerBaseStations.Remove(rollerBaseStation);
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
