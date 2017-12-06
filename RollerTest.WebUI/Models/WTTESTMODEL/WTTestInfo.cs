using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RollerTest.WebUI.Models.WTTESTMODEL
{
    public class WTTESTINFO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]//不自动增长
        public int ID { get; set; }
        [MaxLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string PLATFORM { get; set; }
        [MaxLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string TYPE { get; set; }
        [MaxLength(100)]
        [Column(TypeName = "VARCHAR2")]
        public string NAME { get; set; }
        [MaxLength(100)]
        [Column(TypeName = "VARCHAR2")]
        public string PRD_REQ_NAME { get; set; }
        [MaxLength(1000)]
        [Column(TypeName = "VARCHAR2")]
        public string STANDARD { get; set; }
        [MaxLength(100)]
        [Column(TypeName = "VARCHAR2")]
        public string STANDARD_OPTION { get; set; }
        [MaxLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string STANDARD_REMARK { get; set; }
        [MaxLength(15)]
        [Column(TypeName = "VARCHAR2")]
        public string TESTER_ID { get; set; }
        [MaxLength(15)]
        [Column(TypeName = "VARCHAR2")]
        public string CHECKER_ID { get; set; }
        public int COMMISSION_ID { get; set; }
        [MaxLength(10)]
        [Column(TypeName = "VARCHAR2")]
        public string SERIAL_NO { get; set; }
        [MaxLength(15)]
        [Column(TypeName = "VARCHAR2")]
        public string CREATOR_NAME { get; set; }
        [MaxLength(40)]
        [Column(TypeName = "VARCHAR2")]
        public string DEPT_NAME { get; set; }
        [MaxLength(200)]
        [Column(TypeName = "VARCHAR2")]
        public string PRODUCT_NAME { get; set; }
        [MaxLength(100)]
        [Column(TypeName = "VARCHAR2")]
        public string PRODUCT_NO { get; set; }
        [MaxLength(200)]
        [Column(TypeName = "VARCHAR2")]
        public string PRODUCT_GL_CODE { get; set; }
        [MaxLength(200)]
        [Column(TypeName = "VARCHAR2")]
        public string PRODUCT_TYPE_NO { get; set; }
        [MaxLength(100)]
        [Column(TypeName = "VARCHAR2")]
        public string PRODUCT_CATEGORY { get; set; }
        [MaxLength(200)]
        [Column(TypeName = "VARCHAR2")]
        public string PRODUCT_COMPANY { get; set; }
        [MaxLength(400)]
        [Column(TypeName = "VARCHAR2")]
        public string COMPANY_ADDRESS { get; set; }
        [MaxLength(400)]
        [Column(TypeName = "VARCHAR2")]
        public string TEST_PURPOSE { get; set; }
        [MaxLength(2000)]
        [Column(TypeName = "VARCHAR2")]
        public string TEST_REQUIREMENTS { get; set; }
        [MaxLength(500)]
        [Column(TypeName = "VARCHAR2")]
        public string EMC_PERFORMANCE_A { get; set; }
        [MaxLength(500)]
        [Column(TypeName = "VARCHAR2")]
        public string EMC_PERFORMANCE_B { get; set; }
        [MaxLength(500)]
        [Column(TypeName = "VARCHAR2")]
        public string EMC_PERFORMANCE_C { get; set; }
        [MaxLength(500)]
        [Column(TypeName = "VARCHAR2")]
        public string EMC_PERFORMANCE_D { get; set; }
        [MaxLength(500)]
        [Column(TypeName = "VARCHAR2")]
        public string EMC_CONTENT { get; set; }
        public DateTime APPROVE_TIME { get; set; }
        public DateTime RE_FINISH_DATE { get; set; }

        public virtual ICollection<WTSAMPLEINFO> SampleInfos { get; set; }
    }
}
