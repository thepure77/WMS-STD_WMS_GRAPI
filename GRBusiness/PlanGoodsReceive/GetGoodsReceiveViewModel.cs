using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GRBusiness.PlanGoodsReceive
{
    public partial class GetGoodsReceiveViewModel
    {
        [Key]
        public Guid? GoodsReceiveItemIndex { get; set; }

        public Guid? GoodsReceiveIndex { get; set; }

        
        public string LineNum { get; set; }

        public Guid ProductIndex { get; set; }

        
        public string ProductId { get; set; }

        
        public string ProductName { get; set; }

        
        public string ProductSecondName { get; set; }

        
        public string ProductThirdName { get; set; }

        
        public string ProductLot { get; set; }

        public Guid? ItemStatusIndex { get; set; }

        
        public string ItemStatusId { get; set; }

        
        public string ItemStatusName { get; set; }

        
        public decimal QtyPlan { get; set; }

        public string GoodsReceiveNo { get; set; }

        public string GoodsReceiveDate { get; set; }

        public decimal? qty { get; set; }

        
        public decimal? ratio { get; set; }

        
        public decimal? TotalQty { get; set; }

        public Guid ProductConversionIndex { get; set; }


       
        public string ProductConversionId { get; set; }
        
        public string ProductConversionName { get; set; }

        public Guid PalletIndex { get; set; }

        
        public DateTime? MFGDate { get; set; }

        
        public DateTime? EXPDate { get; set; }

        
        public decimal? UnitWeight { get; set; }

        
        public decimal? Weight { get; set; }

        
        public decimal? UnitWidth { get; set; }

        
        public decimal? UnitLength { get; set; }

        
        public decimal? UnitHeight { get; set; }

        
        public decimal? UnitVolume { get; set; }

        
        public decimal? Volume { get; set; }

        
        public decimal? UnitPrice { get; set; }

        
        public decimal? Price { get; set; }
        
        public string DocumentRefNo1 { get; set; }

        
        public string DocumentRefNo2 { get; set; }

        
        public string DocumentRefNo3 { get; set; }

        
        public string DocumentRefNo4 { get; set; }

        
        public string DocumentRefNo5 { get; set; }

        public int? DocumentStatus { get; set; }

        
        public string UDF1 { get; set; }

        
        public string UDF2 { get; set; }

        
        public string UDF3 { get; set; }

        
        public string UDF4 { get; set; }

        
        public string UDF5 { get; set; }

        public Guid RefProcessIndex { get; set; }

        public string RefDocumentNo { get; set; }

        public string RefDocumentLineNum { get; set; }

        public Guid? RefDocumentIndex { get; set; }

        public Guid? RefDocumentItemIndex { get; set; }


        public string GoodsReceiveRemark { get; set; }


        public string GoodsReceiveDockDoor { get; set; }


        public string Create_By { get; set; }
        
        public DateTime? Create_Date { get; set; }

        public string Update_By { get; set; }
        
        public DateTime? Update_Date { get; set; }

       
        public string Cancel_By { get; set; }

        public DateTime? Cancel_Date { get; set; }
    }
}
