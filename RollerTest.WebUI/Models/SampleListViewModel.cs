using RollerTest.Domain.Entities;
using System.Collections.Generic;

namespace RollerTest.WebUI.Models
{
    public class SampleListViewModel
    {
        public IEnumerable<RollerSampleInfo> rollersampleinfos { get; set; }

    }
}