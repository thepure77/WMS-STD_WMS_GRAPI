using System;
using System.Collections.Generic;
using System.Text;

namespace GRBusiness.PlanGoodsReceive
{
    public partial class RemainQtyViewModel
    {

        public Guid PlanGoodsReceive_Index { get; set; }
        public Guid PlanGoodsReceiveItem_Index { get; set; }
        public string ProductId { get; set; }

        public string ProductName { get; set; }

        public string ProductSecondName { get; set; }

        public decimal Total { get; set; }

        public string ProductConversionId { get; set; }

        public string ProductConversionName { get; set; }

        public decimal Qty { get; set; }

        public decimal Ratio { get; set; }

        public decimal GRTotalQty { get; set; }

        public decimal DocumentStatus { get; set; }

        public string GoodsReceiveDate { get; set; }

    }
}
