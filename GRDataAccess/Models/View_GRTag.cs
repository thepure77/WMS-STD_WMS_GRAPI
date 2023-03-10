using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GRDataAccess.Models
{
  

    public partial class View_GRTag
    {
        [Key]

        public long? RowIndex { get; set; }
        public Guid GoodsReceive_Index { get; set; }


        public Guid GoodsReceiveItem_Index { get; set; }

        public Guid? Tag_Index { get; set; }

        public Guid? TagItem_Index { get; set; }


        public string Tag_No { get; set; }

        public Guid? Product_Index { get; set; }


        public string Product_Id { get; set; }


        public string Product_Name { get; set; }


        public string Product_SecondName { get; set; }


        public string Product_ThirdName { get; set; }


        public string Product_Lot { get; set; }

        public Guid? ItemStatus_Index { get; set; }


        public string ItemStatus_Id { get; set; }


        public string ItemStatus_Name { get; set; }

        public Guid? Suggest_Location_Index { get; set; }


        public string Suggest_Location_Id { get; set; }


        public string Suggest_Location_Name { get; set; }


        public decimal? Qty { get; set; }


        public decimal? Ratio { get; set; }


        public decimal? TotalQty { get; set; }

        public Guid? ProductConversion_Index { get; set; }


        public string ProductConversion_Id { get; set; }


        public string ProductConversion_Name { get; set; }


        public DateTime? MFG_Date { get; set; }


        public DateTime? EXP_Date { get; set; }


        public decimal? UnitWeight { get; set; }


        public decimal? Weight { get; set; }

        public decimal? UnitWidth { get; set; }

        public decimal? UnitLength { get; set; }


        public decimal? UnitHeight { get; set; }

        public decimal? UnitVolume { get; set; }

        public decimal? Volume { get; set; }

        public decimal? UnitPrice { get; set; }

        public decimal? Price { get; set; }


        public string DocumentRef_No1 { get; set; }


        public string DocumentRef_No2 { get; set; }


        public string DocumentRef_No3 { get; set; }


        public string DocumentRef_No4 { get; set; }


        public string DocumentRef_No5 { get; set; }


        public string UDF_1 { get; set; }


        public string UDF_2 { get; set; }


        public string UDF_3 { get; set; }


        public string UDF_4 { get; set; }


        public string UDF_5 { get; set; }


        public string LineNum { get; set; }
        public string ERP_Location { get; set; }

    }
}
