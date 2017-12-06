using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace RollerTest.Domain.ATISEntities
{
    public class ORollerRecordInfo
    {
        [Key]
        public int RollerRecordInfoID { get; set; }

        [ForeignKey("RollerSampleInfo")]
        public int RollerSampleInfoID { get; set; }
        public virtual ORollerSampleInfo RollerSampleInfo { get; set; }
        [Required(ErrorMessage = "请输入样品状态")]
        public bool SampleStatus { get; set; }
        public DateTime CurrentTime { get; set; }
        [Column(TypeName = "varchar2"), MaxLength(500)]
        public string TotalTime { get; set; }
        [Required(ErrorMessage = "请输入样品信息"), Column(TypeName = "varchar2"), MaxLength(2000)]
        public string RecordInfo { get; set; }
    }
}
