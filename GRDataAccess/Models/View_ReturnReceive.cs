using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GRDataAccess.Models
{
    public class View_ReturnReceive
    {
        [Key]
        public Guid PlanGoodsIssue_Index { get; set; }

        public Guid Owner_Index { get; set; }

        public string Owner_Id { get; set; }

        public string Owner_Name { get; set; }

        public string DocumentRef_No1 { get; set; }
        public Guid? DocumentType_Index { get; set; }

        public string DocumentType_Id { get; set; }

        
        public string DocumentType_Name { get; set; }


        public string PlanGoodsIssue_No { get; set; }


        public DateTime? PlanGoodsIssue_Date { get; set; }


        public DateTime? PlanGoodsIssue_Due_Date { get; set; }


        public string Document_Remark { get; set; }

        public int? Document_Status { get; set; }

        public int? DocumentPriority_Status { get; set; }

        public Guid? Warehouse_Index { get; set; }

        public string Warehouse_Id { get; set; }

        public string Warehouse_Name { get; set; }

        public Guid? Warehouse_Index_To { get; set; }

        public string Warehouse_Id_To { get; set; }

        public string Warehouse_Name_To { get; set; }

        public string Create_By { get; set; }


        public DateTime? Create_Date { get; set; }

        public string Update_By { get; set; }


        public DateTime? Update_Date { get; set; }

        public string Cancel_By { get; set; }


        public DateTime? Cancel_Date { get; set; }

        public string Ref_PlanGoodsIssue_No { get; set; }


    }
}
