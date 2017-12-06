using RollerTest.Domain.Entities;
using System.Collections.Generic;

namespace RollerTest.WebUI.Models
{
    public class ProjectListViewModel
    {
        public IEnumerable<RollerProjectInfo> rollerprojectinfos  { get; set; }

    }
}