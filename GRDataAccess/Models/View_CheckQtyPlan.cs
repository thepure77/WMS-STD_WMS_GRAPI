using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GRDataAccess.Models
{
    public partial class View_CheckQtyPlan
    {
        [Key]
        public Guid PlanGoodsReceiveItem_Index { get; set; }

        [StringLength(200)]
        public string Product_Id { get; set; }

        [StringLength(50)]
        public string LineNum { get; set; }

        [StringLength(50)]
        public string PurchaseOrder_No { get; set; }

        public Guid? PurchaseOrder_Index { get; set; }

        public Guid? PurchaseOrderItem_Index { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? TotalQty { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? GR_TotalQty { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Remain_qty { get; set; }
    }
}
