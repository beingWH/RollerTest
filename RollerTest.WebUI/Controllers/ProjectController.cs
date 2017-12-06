using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using RollerTest.Domain.Abstract;
using RollerTest.Domain.Concrete;
using RollerTest.Domain.Entities;
using RollerTest.WebUI.Models;
using RollerTest.WebUI.Models.PROCEDURE;
using RollerTest.WebUI.Models.WTTESTMODEL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace RollerTest.WebUI.Controllers
{
    public class ProjectController : Controller
    {
        private IProjectRepository projectrepository;
        private IBaseRepository baserepository;
        private IReadRepository<WTTESTEQUIPMENT> wtequipmentrepo;
        private IReadRepository<WTTESTINFO> wttestinforepo;
        public ProjectController(IProjectRepository projectrepo,IBaseRepository baserepo,IReadRepository<WTTESTEQUIPMENT> wtequipmentrepo,IReadRepository<WTTESTINFO> wttestinforepo)
        {
            projectrepository = projectrepo;
            baserepository = baserepo;
            this.wtequipmentrepo = wtequipmentrepo;
            this.wttestinforepo = wttestinforepo;
        }

        
        //[Authorize(Roles = "Tester,Admin,Manager")]
        //[HttpPost]
        //public ActionResult EditProject(RollerProjectInfo rollerprojectinfo)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        projectrepository.SaveRollerProjectInfo(rollerprojectinfo);
        //        return RedirectToAction("Index", "Sample");
        //    }
        //    else
        //    {
        //        SettingViewModel settingviewModel = new SettingViewModel(baserepository, wtequipmentrepo);
        //        ViewData["DeviceList"] = settingviewModel.GetLISDeviceList();
        //        return View(rollerprojectinfo);
        //    }

        //}
        //[Authorize(Roles = "Tester,Admin,Manager")]
        //public ViewResult EditProject(int RollerProjectInfoID)
        //{
        //    SettingViewModel settingviewModel = new SettingViewModel(baserepository, wtequipmentrepo);
        //    ViewData["DeviceList"] = settingviewModel.GetLISDeviceList();

        //    RollerProjectInfo rollerprojectinfo = projectrepository.RollerProjectInfos.FirstOrDefault(p => p.RollerProjectInfoID == RollerProjectInfoID);
        //    return View(rollerprojectinfo);
        //}


        [Authorize(Roles = "Tester,Admin,Manager")]
        [HttpPost]
        public ActionResult DeleteProject(int RollerProjectInfoID)
        {
            Entities context = new Entities();
            context.PROCEDURE_ROLLERPROJECTINFO(RollerProjectInfoID);
            context.SaveChanges();
            projectrepository.DeleteRollerProjectInfo(RollerProjectInfoID);
            return RedirectToAction("Index", "Sample");
        }
        [Authorize(Roles = "Tester,Admin,Manager")]
        public ViewResult CreateProject()
        {
            IEnumerable<WTTESTINFO> wttestinfos = wttestinforepo.QueryEntities;
            return View(wttestinfos);
        }
        public ViewResult DetailInfo(int RollerProjectInfoID)
        {
            WTTESTINFO wttestinfo = wttestinforepo.QueryEntities.FirstOrDefault(x => x.ID == RollerProjectInfoID);
            return View(wttestinfo);
        }
        public ActionResult Confirm(int RollerProjectInfoID)
        {
            WTTESTINFO wttestinfo = wttestinforepo.QueryEntities.FirstOrDefault(x => x.ID == RollerProjectInfoID);
            RollerProjectInfo project = new RollerProjectInfo()
            {
                Approve_Time=wttestinfo.APPROVE_TIME,
                CheckPersonID=wttestinfo.CHECKER_ID,
                Commission=wttestinfo.SERIAL_NO,
                CommissionID=wttestinfo.COMMISSION_ID,
                Company_Address=wttestinfo.COMPANY_ADDRESS,
                Platform=wttestinfo.PLATFORM,
                Product_CateGory=wttestinfo.PRODUCT_CATEGORY,
                Product_NO=wttestinfo.PRODUCT_NO,
                Product_Company=wttestinfo.PRODUCT_COMPANY,
                Product_GL_Code=wttestinfo.PRODUCT_GL_CODE,
                Product_Name=wttestinfo.PRODUCT_NAME,
                WTPersonDept=wttestinfo.DEPT_NAME,
                Product_Type_NO=wttestinfo.PRODUCT_TYPE_NO,
                Standard_Remark=wttestinfo.STANDARD_REMARK,
                Standard_Option=wttestinfo.STANDARD_OPTION,
                ReqStandard=wttestinfo.PRD_REQ_NAME,
                Re_Finish_Date=wttestinfo.RE_FINISH_DATE,
                TestID=wttestinfo.ID,
                TestName=wttestinfo.NAME,
                TestPersonID=wttestinfo.TESTER_ID,
                TestStandard=wttestinfo.STANDARD,
                Test_Purpose=wttestinfo.TEST_PURPOSE,
                WTPersonName=wttestinfo.CREATOR_NAME,
                Test_Requirement=wttestinfo.TEST_REQUIREMENTS,
                Type=wttestinfo.TYPE,
            };
            if (ModelState.IsValid)
            {
                projectrepository.SaveRollerProjectInfo(project);
                Entities context = new Entities();
                context.PROCEDURE_ROLLERPROJECTINFO(0);
                context.SaveChanges();
                return RedirectToAction("Index", "Sample");
            }
            else {
                return RedirectToAction("CreateProject");
            }

        }
        public ActionResult Index()
        {
            return View();
        }
    }
}