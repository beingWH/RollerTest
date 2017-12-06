using RollerTest.Domain.Abstract;
using System.Linq;
using RollerTest.Domain.Entities;

namespace RollerTest.Domain.Concrete
{
    public class EFProjectinfoRepository:IProjectRepository
    {
        private EFDbContext context = new EFDbContext();

        public IQueryable<RollerProjectInfo> RollerProjectInfos
        {
            get
            {
                return context.RollerProjectInfos;
            }
        }

        public RollerProjectInfo DeleteRollerProjectInfo(int rollerprojectinfoID)
        {
            RollerProjectInfo dbEntry = context.RollerProjectInfos.Find(rollerprojectinfoID);
            if (dbEntry != null)
            {
                context.RollerProjectInfos.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }

        public void SaveRollerProjectInfo(RollerProjectInfo rollerprojectinfo)
        {
            if (rollerprojectinfo.RollerProjectInfoID == 0)
            {
                context.RollerProjectInfos.Add(rollerprojectinfo);
            }
            else
            {
                RollerProjectInfo dbEntry = context.RollerProjectInfos.Find(rollerprojectinfo.RollerProjectInfoID);
                if (dbEntry != null)
                {
                    dbEntry.Approve_Time = rollerprojectinfo.Approve_Time;
                    dbEntry.CheckPersonID = rollerprojectinfo.CheckPersonID;
                    dbEntry.Commission = rollerprojectinfo.Commission;
                    dbEntry.CommissionID = rollerprojectinfo.CommissionID;
                    dbEntry.Company_Address = rollerprojectinfo.Company_Address;
                    dbEntry.Platform = rollerprojectinfo.Platform;
                    dbEntry.Product_CateGory = rollerprojectinfo.Product_CateGory;
                    dbEntry.Product_Company = rollerprojectinfo.Product_Company;
                    dbEntry.Product_GL_Code = rollerprojectinfo.Product_GL_Code;
                    dbEntry.Product_Name = rollerprojectinfo.Product_Name;
                    dbEntry.Product_NO = rollerprojectinfo.Product_NO;
                    dbEntry.Product_Type_NO = rollerprojectinfo.Product_Type_NO;
                    dbEntry.ReqStandard = rollerprojectinfo.ReqStandard;
                    dbEntry.Re_Finish_Date = rollerprojectinfo.Re_Finish_Date;
                    dbEntry.RollerProjectInfoID = rollerprojectinfo.RollerProjectInfoID;
                    dbEntry.RollerSampleInfo = rollerprojectinfo.RollerSampleInfo;
                    dbEntry.Standard_Option = rollerprojectinfo.Standard_Option;
                    dbEntry.Standard_Remark = rollerprojectinfo.Standard_Remark;
                    dbEntry.TestID = rollerprojectinfo.TestID;
                    dbEntry.TestName = rollerprojectinfo.TestName;
                    dbEntry.TestPersonID = rollerprojectinfo.TestPersonID;
                    dbEntry.TestStandard = rollerprojectinfo.TestStandard;
                    dbEntry.Test_Purpose = rollerprojectinfo.Test_Purpose;
                    dbEntry.Test_Requirement = rollerprojectinfo.Test_Requirement;
                    dbEntry.Type = rollerprojectinfo.Type;
                    dbEntry.WTPersonDept = rollerprojectinfo.WTPersonDept;
                    dbEntry.WTPersonName = rollerprojectinfo.WTPersonName;

                }
            }
            context.SaveChanges();
        }
    }
}
