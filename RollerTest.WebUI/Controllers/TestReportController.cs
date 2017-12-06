using System.Linq;
using System.Web.Mvc;
using RollerTest.Domain.Abstract;
using RollerTest.Domain.Entities;
using System;
using RollerTest.WebUI.Models.PROCEDURE;

namespace RollerTest.WebUI.Controllers
{
    [Authorize(Roles = "Tester,Admin,Manager")]
    public class TestReportController : Controller
    {

        private IBaseRepository baserepo;
        private ISampleinfoRepository samplerepo;
        private ITestreportinfoRepository testreportrepo;
        public TestReportController(ISampleinfoRepository samplerepo, IBaseRepository baserepo, ITestreportinfoRepository testreportrepo)
        {
            this.samplerepo = samplerepo;
            this.baserepo = baserepo;
            this.testreportrepo = testreportrepo;
        }
        // GET: TestReport
        [HttpGet]
        public ActionResult Index(int RollerSampleInfoId)
        {
            RollerTestreportInfo rti = testreportrepo.RollerTestreportInfos.FirstOrDefault(x => x.RollerSampleInfoID == RollerSampleInfoId);
            return View(rti);
        }
        [HttpPost]
        public ActionResult EditTestReport(RollerTestreportInfo rollertestreportinfo)
        {
            testreportrepo.SaveRollerTestreportInfo(rollertestreportinfo);
            Entities context = new Entities();
            context.PROCEDURE_ROLLERTESTREPORTINFO(0);
            context.SaveChanges();
            return RedirectToAction("Index", "TestBlock");
        }
        [HttpGet]
        public ActionResult AfterTest(int RollerSampleInfoId)
        {
            RollerTestreportInfo rollertestreportinfo = testreportrepo.RollerTestreportInfos.FirstOrDefault(x => x.RollerSampleInfoID == RollerSampleInfoId);
            return View(rollertestreportinfo);
        }
        [HttpPost]
        public ActionResult EditAfterReport(RollerTestreportInfo rollertestreportinfo)
        {
            testreportrepo.SaveRollerTestreportInfo(rollertestreportinfo);
            Entities context = new Entities();
            context.PROCEDURE_ROLLERTESTREPORTINFO(0);
            context.SaveChanges();
            return RedirectToAction("Index", "TestBlock");
        }
    }
}