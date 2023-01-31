using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GRDataAccess.Models
{
    public class View_GetTagItem
    {
        [Key]
        public Guid TagItem_Index { get; set; }

        public Guid? Tag_Index { get; set; }


        public string Tag_No { get; set; }

        public Guid GoodsReceive_Index { get; set; }

        public Guid GoodsReceiveItem_Index { get; set; }

        public Guid Product_Index { get; set; }



        public string Product_Id { get; set; }



        public string Product_Name { get; set; }


        public string Product_SecondName { get; set; }


        public string Product_ThirdName { get; set; }


        public string Product_Lot { get; set; }

        public Guid ItemStatus_Index { get; set; }


        public string ItemStatus_Id { get; set; }


        public string ItemStatus_Name { get; set; }


        public decimal? Qty { get; set; }


        public decimal Ratio { get; set; }


        public decimal TotalQty { get; set; }

        public Guid ProductConversion_Index { get; set; }



        public string ProductConversion_Id { get; set; }



        public string ProductConversion_Name { get; set; }


        public decimal? Weight { get; set; }


        public decimal? Volume { get; set; }


        public DateTime? MFG_Date { get; set; }


        public DateTime? EXP_Date { get; set; }

        public string TagRef_No1 { get; set; }


        public string TagRef_No2 { get; set; }


        public string TagRef_No3 { get; set; }


        public string TagRef_No4 { get; set; }


        public string TagRef_No5 { get; set; }

        public int? Tag_Status { get; set; }


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

        public string GoodsReceive_No { get; set; }

        public decimal GRITotalQty { get; set; }

        public string Product_ProductConvertion { get; set; }

    }
}
