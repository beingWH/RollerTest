using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RollerTest.Domain.ATISEntities
{
    public class ORollerProjectInfo
    {
        [Key]
        public int RollerProjectInfoID { get; set; }

        public int CommissionID { get; set; }

        [Column(TypeName = "varchar2"), MaxLength(100)]
        public string Commission { get; set; }

        [Column(TypeName = "varchar2"), MaxLength(200)]
        public string Platform { get; set; }
        [Column(TypeName = "varchar2"), MaxLength(100)]
        public string Type { get; set; }
        [Required]
        public int TestID { get; set; }

        [Column(TypeName = "varchar2"), MaxLength(100)]
        public string TestName { get; set; }
        [Column(TypeName = "varchar2"), MaxLength(500)]
        public string ReqStandard { get; set; }

        [Column(TypeName = "varchar2"), MaxLength(500)]
        public string TestStandard { get; set; }
        [Column(TypeName = "varchar2"), MaxLength(500)]
        public string Standard_Option { get; set; }
        [Column(TypeName = "varchar2"), MaxLength(1000)]
        public string Standard_Remark { get; set; }

        [Column(TypeName = "varchar2"), MaxLength(50)]
        public string TestPersonID { get; set; }
        [Column(TypeName = "varchar2"), MaxLength(50)]

        public string CheckPersonID { get; set; }
        [Column(TypeName = "varchar2"), MaxLength(50)]

        public string WTPersonName { get; set; }
        [Column(TypeName = "varchar2"), MaxLength(50)]

        public string WTPersonDept { get; set; }
        [Column(TypeName = "varchar2"), MaxLength(50)]

        public string Product_NO { get; set; }
        [Column(TypeName = "varchar2"), MaxLength(50)]

        public string Product_GL_Code { get; set; }
        [Column(TypeName = "varchar2"), MaxLength(50)]

        public string Product_Type_NO { get; set; }
        [Column(TypeName = "varchar2"), MaxLength(50)]

        public string Product_CateGory { get; set; }
        [Column(TypeName = "varchar2"), MaxLength(200)]

        public string Product_Name { get; set; }
        [Column(TypeName = "varchar2"), MaxLength(200)]

        public string Product_Company { get; set; }
        [Column(TypeName = "varchar2"), MaxLength(200)]

        public string Company_Address { get; set; }
        [Column(TypeName = "varchar2"), MaxLength(1000)]

        public string Test_Purpose { get; set; }
        [Column(TypeName = "varchar2"), MaxLength(1000)]

        public string Test_Requirement { get; set; }

        public DateTime Approve_Time { get; set; }

        public DateTime Re_Finish_Date { get; set; }

       

        public virtual ICollection<ORollerSampleInfo> RollerSampleInfo { get; set; }
    }
}
