using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GRDataAccess.Models
{
    public class im_GoodsIssueItemLocation
    {
        [Key]
        public Guid GoodsIssueItemLocation_Index { get; set; }

        public Guid GoodsIssueItem_Index { get; set; }

        public Guid GoodsIssue_Index { get; set; }

        public Guid? TagItem_Index { get; set; }

        public Guid? Tag_Index { get; set; }


        public string Tag_No { get; set; }

        public Guid Product_Index { get; set; }



        public string Product_Id { get; set; }



        public string Product_Name { get; set; }


        public string Product_SecondName { get; set; }


        public string Product_ThirdName { get; set; }


        public string Product_Lot { get; set; }

        public Guid? ItemStatus_Index { get; set; }



        public string ItemStatus_Id { get; set; }



        public string ItemStatus_Name { get; set; }

        public Guid? Location_Index { get; set; }


        public string Location_Id { get; set; }


        public string Location_Name { get; set; }


        public decimal Qty { get; set; }


        public decimal Ratio { get; set; }


        public decimal TotalQty { get; set; }

        public Guid ProductConversion_Index { get; set; }



        public string ProductConversion_Id { get; set; }



        public string ProductConversion_Name { get; set; }


        public DateTime? MFG_Date { get; set; }


        public DateTime? EXP_Date { get; set; }


        public decimal? UnitWeight { get; set; }


        public decimal Weight { get; set; }


        public decimal UnitWidth { get; set; }


        public decimal UnitLength { get; set; }


        public decimal UnitHeight { get; set; }


        public decimal UnitVolume { get; set; }


        public decimal Volume { get; set; }


        public decimal UnitPrice { get; set; }


        public decimal Price { get; set; }


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

        public Guid? Ref_Process_Index { get; set; }

        public string Ref_Document_No { get; set; }

        public string Ref_Document_LineNum { get; set; }

        public Guid? Ref_Document_Index { get; set; }

        public Guid? Ref_DocumentItem_Index { get; set; }

        public Guid GoodsReceiveItem_Index { get; set; }


        public string Create_By { get; set; }


        public DateTime? Create_Date { get; set; }


        public string Update_By { get; set; }


        public DateTime? Update_Date { get; set; }


        public string Cancel_By { get; set; }


        public DateTime? Cancel_Date { get; set; }

        public int? Picking_Status { get; set; }


        public string Picking_By { get; set; }


        public DateTime? Picking_Date { get; set; }


        public string Picking_Ref1 { get; set; }


        public string Picking_Ref2 { get; set; }


        public decimal? Picking_Qty { get; set; }


        public decimal? Picking_Ratio { get; set; }


        public decimal? Picking_TotalQty { get; set; }

        public Guid? Picking_ProductConversion_Index { get; set; }

        public int? Mashall_Status { get; set; }


        public decimal? Mashall_Qty { get; set; }
    }
}
