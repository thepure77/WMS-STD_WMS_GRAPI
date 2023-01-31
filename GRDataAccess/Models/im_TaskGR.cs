using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GRDataAccess.Models
{
  

    public partial class im_TaskGR
    {
        [Key]
        public Guid TaskGR_Index { get; set; }

        public string TaskGR_No { get; set; }

        public Guid Process_Index { get; set; }

        public Guid? TaskGroup_Index { get; set; }

        public string TaskGroup_Id { get; set; }

        public string TaskGroup_Name { get; set; }

        public int? DocumentPriority_Status { get; set; }

        public string DocumentRef_No1 { get; set; }

        public string DocumentRef_No2 { get; set; }

        public string DocumentRef_No3 { get; set; }

        public string DocumentRef_No4 { get; set; }

        public string DocumentRef_No5 { get; set; }

        public int? Document_Status { get; set; }

        public string UDF_1 { get; set; }

        public string UDF_2 { get; set; }

        public string UDF_3 { get; set; }

        public string UDF_4 { get; set; }

        public string UDF_5 { get; set; }

        public string Create_By { get; set; }

        public DateTime? Create_Date { get; set; }

        public string Update_By { get; set; }

        public DateTime? Update_Date { get; set; }

        public string Cancel_By { get; set; }

        public DateTime? Cancel_Date { get; set; }

        public string DoTask_By { get; set; }

        public DateTime? DoTask_Date { get; set; }

        public string UserAssign { get; set; }

        public string Assign_By { get; set; }



    }
}
