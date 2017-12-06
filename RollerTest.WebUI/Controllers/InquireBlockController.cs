using RollerTest.Domain.Abstract;
using RollerTest.Domain.Entities;
using RollerTest.WebUI.Models;
using System.Linq;
using System.Web.Mvc;

namespace RollerTest.WebUI.Controllers
{
    public class InquireBlockController : Controller
    {
        private IProjectRepository projectrepo;
        private ISampleinfoRepository samplerepo;
        private IRecordinfoRepository recordrepo;
        private ITestreportinfoRepository testreportrepo;
        public InquireBlockController(IProjectRepository projectrepo, ISampleinfoRepository samplerepo, IRecordinfoRepository recordrepo, ITestreportinfoRepository testreportrepo)
        {
            this.projectrepo = projectrepo;
            this.samplerepo = samplerepo;
            this.recordrepo = recordrepo;
            this.testreportrepo = testreportrepo;
        }
        // GET: InquireBlock
        public ActionResult Index(int projectId)
        {
            SampleListViewModel slvm = new SampleListViewModel() {
                rollersampleinfos=samplerepo.RollerSampleInfos.Where(x=>x.RollerProjectInfoID==projectId)
            };
            return View(slvm);
        }
        public ActionResult ViewAllInfo(int sampleId)
        {
            RollerSampleInfo rollersampleinfo = samplerepo.RollerSampleInfos.FirstOrDefault(x => x.RollerSampleInfoID == sampleId);
            SampleListViewModel slvm = new SampleListViewModel() { rollersampleinfos = samplerepo.RollerSampleInfos.Where(x=>x.RollerProjectInfoID== rollersampleinfo.RollerProjectInfoID) };
            ViewAllInfoModel vaim = new ViewAllInfoModel() {
                samplelistviewmodel = slvm,
                rollerrecordinfos =recordrepo.RollerRecordInfos.Where(x=>x.RollerSampleInfoID==sampleId),
                rollertestreportinfos=testreportrepo.RollerTestreportInfos.Where(x=>x.RollerSampleInfoID==sampleId),
                rollersampleinfo= rollersampleinfo
            };
            return View(vaim);
        }
        public PartialViewResult RightSidebar()
        {
            ProjectListViewModel plvm = new ProjectListViewModel()
            {
                rollerprojectinfos = projectrepo.RollerProjectInfos
            };
            return PartialView(plvm);
        }
    }
}