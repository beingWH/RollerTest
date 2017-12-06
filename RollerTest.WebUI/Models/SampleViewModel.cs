using RollerTest.Domain.Entities;
using System.Collections.Generic;

namespace RollerTest.WebUI.Models
{
    public class SampleViewModel
    {
        public RollerProjectInfo rollerprojectinfo { get; set; }
        public IEnumerable<RollerSampleInfo> rollersampleinfos {get;set;}
        public ProjectListViewModel projectlistviewmodel { get; set; }

    }
}