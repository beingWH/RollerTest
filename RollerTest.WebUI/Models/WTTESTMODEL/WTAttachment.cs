using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RollerTest.WebUI.Models.WTTESTMODEL
{
    public class WTATTACHMENT
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]//不自动增长
        public int ID { get; set; }
        public int MAIN_ID { get; set; }
        public DateTime CREATE_TIME { get; set; }
        [Column(TypeName = "varchar2"), MaxLength(15)]
        public string CREATOR_ID { get; set; }
        [Column(TypeName = "varchar2"), MaxLength(15)]
        public string CREATOR_NAME { get; set; }
        [Column(TypeName = "varchar2"), MaxLength(40)]
        public string DEPT_NAME { get; set; }
        [Column(TypeName = "varchar2"), MaxLength(40)]
        public string NAME { get; set; }
        public int FILE_SIZE { get; set; }
        [Column(TypeName = "varchar2"), MaxLength(200)]
        public string DIRECTORY { get; set; }
        [Column(TypeName = "varchar2"), MaxLength(500)]
        public string DESCRIPTION { get; set; }
        [Column(TypeName = "varchar2"), MaxLength(4)]
        public string FILE_TYPE { get; set; }
        [Column(TypeName = "varchar2"), MaxLength(100)]
        public string LOCAL_FILE_NAME { get; set; }
        [Column(TypeName = "varchar2"), MaxLength(10)]
        public string ATTACHMENT_TYPE { get; set; }
    }
}
