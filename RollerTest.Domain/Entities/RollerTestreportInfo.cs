using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace RollerTest.Domain.Entities
{
    public class RollerTestreportInfo
    {
        [Key]
        public int RollerTestReportInfoID { get; set; }

        [ForeignKey("RollerSampleInfo")]
        public int RollerSampleInfoID { get; set; }
        public virtual RollerSampleInfo RollerSampleInfo { get; set; }
        public bool StartStatus { get; set; }
        [Column(TypeName = "varchar2"), MaxLength(2000)]
        public string StartText { get; set; }
        public bool EndStatus { get; set; }
        [Column(TypeName = "varchar2"), MaxLength(2000)]
        public string EndText { get; set; }
        public bool FinalStatus { get; set; }
    }
}
