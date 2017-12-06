using RollerTest.Domain.Entities;
using System.Linq;

namespace RollerTest.Domain.Abstract
{
    public interface IProjectRepository
    {
        IQueryable<RollerProjectInfo> RollerProjectInfos { get; }
        void SaveRollerProjectInfo(RollerProjectInfo rollerprojectinfo);
        RollerProjectInfo DeleteRollerProjectInfo(int rollerprojectinfoID);
    }
}
