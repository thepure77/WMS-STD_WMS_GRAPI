using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GRDataAccessBalance.Models
{

    public partial class WM_BinCard
    {
        [Key]
        public Guid BinCard_Index { get; set; }

        public Guid? Process_Index { get; set; }

        public Guid? DocumentType_Index { get; set; }


        public string DocumentType_Id { get; set; }

        public string DocumentType_Name { get; set; }

        public Guid? GoodsReceive_Index { get; set; }

        public Guid? GoodsReceiveItem_Index { get; set; }

        public Guid? GoodsReceiveItemLocation_Index { get; set; }

        public string BinCard_No { get; set; }

        public DateTime? BinCard_Date { get; set; }

        public Guid? TagItem_Index { get; set; }

        public Guid? Tag_Index { get; set; }


        public string Tag_No { get; set; }

        public Guid? Tag_Index_To { get; set; }


        public string Tag_No_To { get; set; }

        public Guid? Product_Index { get; set; }



        public string Product_Id { get; set; }


        public string Product_Name { get; set; }


        public string Product_SecondName { get; set; }


        public string Product_ThirdName { get; set; }

        public Guid? Product_Index_To { get; set; }



        public string Product_Id_To { get; set; }



        public string Product_Name_To { get; set; }


        public string Product_SecondName_To { get; set; }


        public string Product_ThirdName_To { get; set; }


        public string Product_Lot { get; set; }


        public string Product_Lot_To { get; set; }

        public Guid? ItemStatus_Index { get; set; }


        public string ItemStatus_Id { get; set; }


        public string ItemStatus_Name { get; set; }

        public Guid? ItemStatus_Index_To { get; set; }


        public string ItemStatus_Id_To { get; set; }


        public string ItemStatus_Name_To { get; set; }

        public Guid? ProductConversion_Index { get; set; }


        public string ProductConversion_Id { get; set; }


        public string ProductConversion_Name { get; set; }

        public Guid? Owner_Index { get; set; }


        public string Owner_Id { get; set; }

        public string Owner_Name { get; set; }

        public Guid? Owner_Index_To { get; set; }



        public string Owner_Id_To { get; set; }



        public string Owner_Name_To { get; set; }

        public Guid? Location_Index { get; set; }


        public string Location_Id { get; set; }


        public string Location_Name { get; set; }

        public Guid? Location_Index_To { get; set; }


        public string Location_Id_To { get; set; }


        public string Location_Name_To { get; set; }


        public DateTime? GoodsReceive_EXP_Date { get; set; }


        public DateTime? GoodsReceive_EXP_Date_To { get; set; }


        public decimal? BinCard_QtyIn { get; set; }


        public decimal? BinCard_QtyOut { get; set; }


        public decimal? BinCard_QtySign { get; set; }


        public decimal? BinCard_UnitWeightIn { get; set; }

        public Guid? BinCard_UnitWeightIn_Index { get; set; }


        public string BinCard_UnitWeightIn_Id { get; set; }


        public string BinCard_UnitWeightIn_Name { get; set; }


        public decimal? BinCard_UnitWeightInRatio { get; set; }


        public decimal? BinCard_WeightIn { get; set; }

        public Guid? BinCard_WeightIn_Index { get; set; }


        public string BinCard_WeightIn_Id { get; set; }


        public string BinCard_WeightIn_Name { get; set; }


        public decimal? BinCard_WeightInRatio { get; set; }


        public decimal? BinCard_UnitNetWeightIn { get; set; }

        public Guid? BinCard_UnitNetWeightIn_Index { get; set; }


        public string BinCard_UnitNetWeightIn_Id { get; set; }


        public string BinCard_UnitNetWeightIn_Name { get; set; }


        public decimal? BinCard_UnitNetWeightInRatio { get; set; }


        public decimal? BinCard_NetWeightIn { get; set; }

        public Guid? BinCard_NetWeightIn_Index { get; set; }


        public string BinCard_NetWeightIn_Id { get; set; }


        public string BinCard_NetWeightIn_Name { get; set; }


        public decimal? BinCard_NetWeightInRatio { get; set; }


        public decimal? BinCard_UnitGrsWeightIn { get; set; }

        public Guid? BinCard_UnitGrsWeightIn_Index { get; set; }


        public string BinCard_UnitGrsWeightIn_Id { get; set; }


        public string BinCard_UnitGrsWeightIn_Name { get; set; }


        public decimal? BinCard_UnitGrsWeightInRatio { get; set; }


        public decimal? BinCard_GrsWeightIn { get; set; }

        public Guid? BinCard_GrsWeightIn_Index { get; set; }


        public string BinCard_GrsWeightIn_Id { get; set; }


        public string BinCard_GrsWeightIn_Name { get; set; }


        public decimal? BinCard_GrsWeightInRatio { get; set; }


        public decimal? BinCard_UnitWidthIn { get; set; }

        public Guid? BinCard_UnitWidthIn_Index { get; set; }


        public string BinCard_UnitWidthIn_Id { get; set; }


        public string BinCard_UnitWidthIn_Name { get; set; }


        public decimal? BinCard_UnitWidthInRatio { get; set; }


        public decimal? BinCard_WidthIn { get; set; }

        public Guid? BinCard_WidthIn_Index { get; set; }


        public string BinCard_WidthIn_Id { get; set; }


        public string BinCard_WidthIn_Name { get; set; }


        public decimal? BinCard_WidthInRatio { get; set; }


        public decimal? BinCard_UnitLengthIn { get; set; }

        public Guid? BinCard_UnitLengthIn_Index { get; set; }


        public string BinCard_UnitLengthIn_Id { get; set; }


        public string BinCard_UnitLengthIn_Name { get; set; }


        public decimal? BinCard_UnitLengthInRatio { get; set; }


        public decimal? BinCard_LengthIn { get; set; }

        public Guid? BinCard_LengthIn_Index { get; set; }


        public string BinCard_LengthIn_Id { get; set; }


        public string BinCard_LengthIn_Name { get; set; }


        public decimal? BinCard_LengthInRatio { get; set; }


        public decimal? BinCard_UnitHeightIn { get; set; }

        public Guid? BinCard_UnitHeightIn_Index { get; set; }


        public string BinCard_UnitHeightIn_Id { get; set; }


        public string BinCard_UnitHeightIn_Name { get; set; }


        public decimal? BinCard_UnitHeightInRatio { get; set; }


        public decimal? BinCard_HeightIn { get; set; }

        public Guid? BinCard_HeightIn_Index { get; set; }


        public string BinCard_HeightIn_Id { get; set; }


        public string BinCard_HeightIn_Name { get; set; }


        public decimal? BinCard_HeightInRatio { get; set; }


        public decimal? BinCard_UnitVolumeIn { get; set; }


        public decimal? BinCard_VolumeIn { get; set; }

        public decimal? BinCard_UnitWeightOut { get; set; }

        
        public Guid? BinCard_UnitWeightOut_Index { get; set; }


        public string BinCard_UnitWeightOut_Id { get; set; }


        public string BinCard_UnitWeightOut_Name { get; set; }


        public decimal? BinCard_UnitWeightOutRatio { get; set; }


        public decimal? BinCard_WeightOut { get; set; }

        public Guid? BinCard_WeightOut_Index { get; set; }


        public string BinCard_WeightOut_Id { get; set; }


        public string BinCard_WeightOut_Name { get; set; }


        public decimal? BinCard_WeightOutRatio { get; set; }


        public decimal? BinCard_UnitNetWeightOut { get; set; }

        public Guid? BinCard_UnitNetWeightOut_Index { get; set; }


        public string BinCard_UnitNetWeightOut_Id { get; set; }


        public string BinCard_UnitNetWeightOut_Name { get; set; }


        public decimal? BinCard_UnitNetWeightOutRatio { get; set; }


        public decimal? BinCard_NetWeightOut { get; set; }

        public Guid? BinCard_NetWeightOut_Index { get; set; }


        public string BinCard_NetWeightOut_Id { get; set; }


        public string BinCard_NetWeightOut_Name { get; set; }


        public decimal? BinCard_NetWeightOutRatio { get; set; }


        public decimal? BinCard_UnitGrsWeightOut { get; set; }

        public Guid? BinCard_UnitGrsWeightOut_Index { get; set; }


        public string BinCard_UnitGrsWeightOut_Id { get; set; }


        public string BinCard_UnitGrsWeightOut_Name { get; set; }


        public decimal? BinCard_UnitGrsWeightOutRatio { get; set; }


        public decimal? BinCard_GrsWeightOut { get; set; }

        public Guid? BinCard_GrsWeightOut_Index { get; set; }


        public string BinCard_GrsWeightOut_Id { get; set; }


        public string BinCard_GrsWeightOut_Name { get; set; }


        public decimal? BinCard_GrsWeightOutRatio { get; set; }


        public decimal? BinCard_UnitWidthOut { get; set; }

        public Guid? BinCard_UnitWidthOut_Index { get; set; }


        public string BinCard_UnitWidthOut_Id { get; set; }


        public string BinCard_UnitWidthOut_Name { get; set; }


        public decimal? BinCard_UnitWidthOutRatio { get; set; }


        public decimal? BinCard_WidthOut { get; set; }

        public Guid? BinCard_WidthOut_Index { get; set; }


        public string BinCard_WidthOut_Id { get; set; }


        public string BinCard_WidthOut_Name { get; set; }


        public decimal? BinCard_WidthOutRatio { get; set; }


        public decimal? BinCard_UnitLengthOut { get; set; }

        public Guid? BinCard_UnitLengthOut_Index { get; set; }


        public string BinCard_UnitLengthOut_Id { get; set; }


        public string BinCard_UnitLengthOut_Name { get; set; }


        public decimal? BinCard_UnitLengthOutRatio { get; set; }


        public decimal? BinCard_LengthOut { get; set; }

        public Guid? BinCard_LengthOut_Index { get; set; }


        public string BinCard_LengthOut_Id { get; set; }


        public string BinCard_LengthOut_Name { get; set; }


        public decimal? BinCard_LengthOutRatio { get; set; }


        public decimal? BinCard_UnitHeightOut { get; set; }

        public Guid? BinCard_UnitHeightOut_Index { get; set; }


        public string BinCard_UnitHeightOut_Id { get; set; }


        public string BinCard_UnitHeightOut_Name { get; set; }


        public decimal? BinCard_UnitHeightOutRatio { get; set; }


        public decimal? BinCard_HeightOut { get; set; }

        public Guid? BinCard_HeightOut_Index { get; set; }


        public string BinCard_HeightOut_Id { get; set; }


        public string BinCard_HeightOut_Name { get; set; }


        public decimal? BinCard_HeightOutRatio { get; set; }


        public decimal? BinCard_UnitVolumeOut { get; set; }


        public decimal? BinCard_VolumeOut { get; set; }

        public decimal? BinCard_UnitWeightSign { get; set; }

        public Guid? BinCard_UnitWeightSign_Index { get; set; }


        public string BinCard_UnitWeightSign_Id { get; set; }


        public string BinCard_UnitWeightSign_Name { get; set; }


        public decimal? BinCard_UnitWeightSignRatio { get; set; }


        public decimal? BinCard_WeightSign { get; set; }

        public Guid? BinCard_WeightSign_Index { get; set; }


        public string BinCard_WeightSign_Id { get; set; }


        public string BinCard_WeightSign_Name { get; set; }


        public decimal? BinCard_WeightSignRatio { get; set; }


        public decimal? BinCard_UnitNetWeightSign { get; set; }

        public Guid? BinCard_UnitNetWeightSign_Index { get; set; }


        public string BinCard_UnitNetWeightSign_Id { get; set; }


        public string BinCard_UnitNetWeightSign_Name { get; set; }


        public decimal? BinCard_UnitNetWeightSignRatio { get; set; }


        public decimal? BinCard_NetWeightSign { get; set; }

        public Guid? BinCard_NetWeightSign_Index { get; set; }


        public string BinCard_NetWeightSign_Id { get; set; }


        public string BinCard_NetWeightSign_Name { get; set; }


        public decimal? BinCard_NetWeightSignRatio { get; set; }


        public decimal? BinCard_UnitGrsWeightSign { get; set; }

        public Guid? BinCard_UnitGrsWeightSign_Index { get; set; }


        public string BinCard_UnitGrsWeightSign_Id { get; set; }


        public string BinCard_UnitGrsWeightSign_Name { get; set; }


        public decimal? BinCard_UnitGrsWeightSignRatio { get; set; }


        public decimal? BinCard_GrsWeightSign { get; set; }

        public Guid? BinCard_GrsWeightSign_Index { get; set; }


        public string BinCard_GrsWeightSign_Id { get; set; }


        public string BinCard_GrsWeightSign_Name { get; set; }


        public decimal? BinCard_GrsWeightSignRatio { get; set; }


        public decimal? BinCard_UnitWidthSign { get; set; }

        public Guid? BinCard_UnitWidthSign_Index { get; set; }


        public string BinCard_UnitWidthSign_Id { get; set; }


        public string BinCard_UnitWidthSign_Name { get; set; }


        public decimal? BinCard_UnitWidthSignRatio { get; set; }


        public decimal? BinCard_WidthSign { get; set; }

        public Guid? BinCard_WidthSign_Index { get; set; }


        public string BinCard_WidthSign_Id { get; set; }


        public string BinCard_WidthSign_Name { get; set; }


        public decimal? BinCard_WidthSignRatio { get; set; }


        public decimal? BinCard_UnitLengthSign { get; set; }

        public Guid? BinCard_UnitLengthSign_Index { get; set; }


        public string BinCard_UnitLengthSign_Id { get; set; }


        public string BinCard_UnitLengthSign_Name { get; set; }


        public decimal? BinCard_UnitLengthSignRatio { get; set; }


        public decimal? BinCard_LengthSign { get; set; }

        public Guid? BinCard_LengthSign_Index { get; set; }


        public string BinCard_LengthSign_Id { get; set; }


        public string BinCard_LengthSign_Name { get; set; }


        public decimal? BinCard_LengthSignRatio { get; set; }


        public decimal? BinCard_UnitHeightSign { get; set; }

        public Guid? BinCard_UnitHeightSign_Index { get; set; }


        public string BinCard_UnitHeightSign_Id { get; set; }


        public string BinCard_UnitHeightSign_Name { get; set; }


        public decimal? BinCard_UnitHeightSignRatio { get; set; }


        public decimal? BinCard_HeightSign { get; set; }

        public Guid? BinCard_HeightSign_Index { get; set; }


        public string BinCard_HeightSign_Id { get; set; }


        public string BinCard_HeightSign_Name { get; set; }


        public decimal? BinCard_HeightSignRatio { get; set; }


        public decimal? BinCard_UnitVolumeSign { get; set; }


        public decimal? BinCard_VolumeSign { get; set; }


        public string Ref_Document_No { get; set; }

        public Guid? Ref_Document_Index { get; set; }

        public Guid? Ref_DocumentItem_Index { get; set; }


        public string TagOutItem_Index { get; set; }

        public Guid? TagOut_Index { get; set; }


        public string TagOut_No { get; set; }

        public Guid? TagOut_Index_To { get; set; }


        public string TagOut_No_To { get; set; }


        public string UDF_1 { get; set; }


        public string UDF_2 { get; set; }


        public string UDF_3 { get; set; }


        public string UDF_4 { get; set; }


        public string UDF_5 { get; set; }


        public string Create_By { get; set; }

        public DateTime? Create_Date { get; set; }

        public Guid? BinBalance_Index { get; set; }
        public string ERP_Location { get; set; }
        public string ERP_Location_To { get; set; }
    }
}
