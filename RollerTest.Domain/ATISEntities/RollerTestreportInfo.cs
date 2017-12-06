using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RollerTest.Domain.ATISEntities
{
    public class ORollerTestreportInfo
    {
        [Key]
        public int RollerTestReportInfoID { get; set; }

        [ForeignKey("RollerSampleInfo")]
        public int RollerSampleInfoID { get; set; }
        public virtual ORollerSampleInfo RollerSampleInfo { get; set; }
        public bool StartStatus { get; set; }
        [Column(TypeName = "varchar2"), MaxLength(2000)]
        public string StartText { get; set; }
        public bool EndStatus { get; set; }
        [Column(TypeName = "varchar2"), MaxLength(2000)]
        public string EndText { get; set; }
        public bool FinalStatus { get; set; }
    }
}
