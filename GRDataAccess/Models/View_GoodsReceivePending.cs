using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GRDataAccess.Models
{
    public partial class View_GoodsReceivePending
    {
        
        public Guid PlanGoodsReceive_Index { get; set; }
        [Key]
        public Guid PlanGoodsReceiveItem_Index { get; set; }

        public string Product_Id { get; set; }

        public string Product_Name { get; set; }

        public string Product_SecondName { get; set; }
        [Column(TypeName = "decimal")]
        public decimal Total { get; set; }

        public string ProductConversion_Id { get; set; }

        public string ProductConversion_Name { get; set; }
        [Column(TypeName = "decimal")]
        public decimal Qty { get; set; }
        [Column(TypeName = "decimal")]
        public decimal Ratio { get; set; }
        [Column(TypeName = "decimal")]
        public decimal GRTotalQty { get; set; }

        public int Document_Status { get; set; }

        //public DateTime? GoodsReceive_Date { get; set; }
    }
}
