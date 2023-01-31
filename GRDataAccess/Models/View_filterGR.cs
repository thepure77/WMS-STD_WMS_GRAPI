using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GRDataAccess.Models
{
  

    public partial class View_filterGR
    {
        [Key]
        public Guid? GoodsReceive_Index { get; set; }

        public string GoodsReceive_No { get; set; }

        public DateTime GoodsReceive_Date { get; set; }

        public Guid? DocumentType_Index { get; set; }

        public string DocumentType_Id { get; set; }

        public string DocumentType_Name { get; set; }
        public Guid? Owner_Index { get; set; }

        public string Owner_Id { get; set; }

        public string Owner_Name { get; set; }

        public Guid? Vendor_Index { get; set; }

        public string Vendor_Id { get; set; }

        public string Vendor_Name { get; set; }

        //public int? Putaway_Status { get; set; }

        public string DocumentRef_No2 { get; set; }

        public string Create_By { get; set; }

        public DateTime? Create_Date { get; set; }

        public string Ref_Plan_No { get; set; }

        public int? Document_Status { get; set; }

    }
}
