using RollerTest.Domain.Entities;
using System.Collections.Generic;

namespace RollerTest.WebUI.Models
{
    public class SampleTestingRecordViewModel
    {
        public IEnumerable<RollerRecordInfo> rollerrecordinfos { get; set; }
        public int SampleId { get; set; }

    }
}