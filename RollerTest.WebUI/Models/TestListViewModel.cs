using RollerTest.Domain.Entities;
using System.Collections.Generic;

namespace RollerTest.WebUI.Models
{
    public class TestListViewModel
    {
        public IEnumerable<RollerSampleInfo> rollersampleinfos { get; set; }
    }
}