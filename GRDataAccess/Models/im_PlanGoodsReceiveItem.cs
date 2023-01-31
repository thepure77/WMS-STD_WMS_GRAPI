using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GRDataAccess.Models
{
    public partial class IM_PlanGoodsReceiveItem
    {
        [Key]
        public Guid PlanGoodsReceiveItem_Index { get; set; }

        public Guid? PlanGoodsReceive_Index { get; set; }


        public string LineNum { get; set; }

        public Guid? Product_Index { get; set; }


        public string Product_Id { get; set; }


        public string Product_Name { get; set; }


        public string Product_SecondName { get; set; }


        public string Product_ThirdName { get; set; }


        public string Product_Lot { get; set; }

        public Guid? ItemStatus_Index { get; set; }


        public string ItemStatus_Id { get; set; }


        public string ItemStatus_Name { get; set; }


        public decimal? Qty { get; set; }


        public decimal? Ratio { get; set; }


        public decimal? TotalQty { get; set; }

        public Guid? ProductConversion_Index { get; set; }


        public string ProductConversion_Id { get; set; }


        public string ProductConversion_Name { get; set; }

        public DateTime? MFG_Date { get; set; }

        public DateTime? EXP_Date { get; set; }


        public string DocumentRef_No1 { get; set; }


        public string DocumentRef_No2 { get; set; }


        public string DocumentRef_No3 { get; set; }


        public string DocumentRef_No4 { get; set; }


        public string DocumentRef_No5 { get; set; }

        public int? Document_Status { get; set; }


        public string DocumentItem_Remark { get; set; }


        public string UDF_1 { get; set; }


        public string UDF_2 { get; set; }


        public string UDF_3 { get; set; }


        public string UDF_4 { get; set; }


        public string UDF_5 { get; set; }


        public decimal? UnitWeight { get; set; }


        public decimal? Weight { get; set; }


        public decimal? NetWeight { get; set; }

        public Guid? Weight_Index { get; set; }


        public string Weight_Id { get; set; }


        public string Weight_Name { get; set; }

        public decimal? WeightRatio { get; set; }

        public decimal? UnitGrsWeight { get; set; }


        public decimal? GrsWeight { get; set; }

        public Guid? GrsWeight_Index { get; set; }


        public string GrsWeight_Id { get; set; }


        public string GrsWeight_Name { get; set; }
        public decimal? GrsWeightRatio { get; set; }


        public decimal? UnitWidth { get; set; }


        public decimal? Width { get; set; }

        public Guid? Width_Index { get; set; }


        public string Width_Id { get; set; }


        public string Width_Name { get; set; }

        public decimal? WidthRatio { get; set; }

        public decimal? UnitLength { get; set; }


        public decimal? Length { get; set; }

        public Guid? Length_Index { get; set; }


        public string Length_Id { get; set; }


        public string Length_Name { get; set; }

        public decimal? LengthRatio { get; set; }

        public decimal? UnitHeight { get; set; }


        public decimal? Height { get; set; }

        public Guid? Height_Index { get; set; }


        public string Height_Id { get; set; }


        public string Height_Name { get; set; }
        public decimal? HeightRatio { get; set; }



        public decimal? UnitVolume { get; set; }


        public decimal? Volume { get; set; }

        public decimal? UnitPrice { get; set; }


        public decimal? Price { get; set; }

        public decimal? TotalPrice { get; set; }

        public Guid? Currency_Index { get; set; }


        public string Currency_Id { get; set; }


        public string Currency_Name { get; set; }


        public string Ref_Code1 { get; set; }


        public string Ref_Code2 { get; set; }


        public string Ref_Code3 { get; set; }


        public string Ref_Code4 { get; set; }


        public string Ref_Code5 { get; set; }


        public string Create_By { get; set; }

        public DateTime? Create_Date { get; set; }


        public string Update_By { get; set; }

        public DateTime? Update_Date { get; set; }


        public string Cancel_By { get; set; }

        public DateTime? Cancel_Date { get; set; }
    }
}
