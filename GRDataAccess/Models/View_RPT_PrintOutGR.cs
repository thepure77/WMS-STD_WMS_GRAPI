using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GRDataAccess.Models
{


    public partial class View_RPT_PrintOutGR
    {
        [Key]
        public long? Row_Index { get; set; }
        public Guid? GoodsReceive_Index { get; set; }
        public string GoodsReceive_No { get; set; }
        public Guid? DocumentType_Index { get; set; }
        public string DocumentType_Id { get; set; }
        public string DocumentType_Name { get; set; }
        public Guid? Product_Index { get; set; }
        public string Product_Id { get; set; }
        public string Product_Name { get; set; }
        public Guid? Owner_Index { get; set; }
        public string Owner_Id { get; set; }
        public string Owner_Name { get; set; }
        public Guid? Warehouse_Index { get; set; }
        public string Warehouse_Id { get; set; }
        public string Warehouse_Name { get; set; }
        public string ProductConversion_Name { get; set; }
        public string LineNum { get; set; }
        public decimal? Qty { get; set; }
        public DateTime? GoodsReceive_Date { get; set; }
        public Guid? Ref_Document_Index { get; set; }
        public string Ref_Document_No { get; set; }

    }
}
