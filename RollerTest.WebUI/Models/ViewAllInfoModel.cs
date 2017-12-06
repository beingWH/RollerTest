using RollerTest.Domain.Entities;
using System.Collections.Generic;

namespace RollerTest.WebUI.Models
{
    public class ViewAllInfoModel
    {
        public IEnumerable<RollerTestreportInfo> rollertestreportinfos { get; set; }
        public SampleListViewModel samplelistviewmodel { get; set; }
        public RollerSampleInfo rollersampleinfo { get; set; }

        public IEnumerable<RollerRecordInfo> rollerrecordinfos { get; set; }

    }
}