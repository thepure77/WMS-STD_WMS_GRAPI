using System;
using System.Collections.Generic;
using System.Text;

namespace GRBusiness.GoodsReceive
{
    public partial class GoodsReceiveItemPOContrack
    {

        public Guid Po_Index { get; set; }
        public Guid? GoodsReceive_Index { get; set; }

        public Guid? GoodsReceiveItem_Index { get; set; }

        public Guid? GoodsIssueItem_Index { get; set; }

        public Guid? GoodsIssue_Index { get; set; }

        public string GoodsIssue_No { get; set; }

        public Guid? DocumentType_Index { get; set; }

        public decimal? Qty { get; set; }

        public decimal? Ratio { get; set; }

        public decimal? TotalQty { get; set; }

        public string DocumentType_Id { get; set; }

        public string DocumentType_Name { get; set; }

        public string GoodsReceive_No { get; set; }

        public Guid Product_Index { get; set; }


        public string Product_Id { get; set; }


        public string Product_Name { get; set; }

        public Guid? ProductConversion_Index { get; set; }


        public string ProductConversion_Id { get; set; }


        public string ProductConversion_Name { get; set; }


        public string ERP_Location { get; set; }

        public int? IsActive { get; set; }

        public int? IsDelete { get; set; }

        public int? IsDelete_Plant { get; set; }

        public int? IsSystem { get; set; }

        public int? Status_Id { get; set; }


        public string Create_By { get; set; }

        public DateTime? Create_Date { get; set; }


        public string Update_By { get; set; }

        public DateTime? Update_Date { get; set; }


        public string Cancel_By { get; set; }

        public DateTime? Cancel_Date { get; set; }

    }
}
