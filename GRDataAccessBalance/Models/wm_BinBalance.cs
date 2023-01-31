using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GRDataAccessBalance.Models
{

    public partial class WM_BinBalance
    {   
        [Key]
        public Guid BinBalance_Index { get; set; }

        public Guid Owner_Index { get; set; }

        public string Owner_Id { get; set; }


        public string Owner_Name { get; set; }

        public Guid Location_Index { get; set; }

        public string Location_Id { get; set; }


        public string Location_Name { get; set; }

        public Guid GoodsReceive_Index { get; set; }

        public string GoodsReceive_No { get; set; }

        public DateTime GoodsReceive_Date { get; set; }

        public Guid GoodsReceiveItem_Index { get; set; }

        public Guid GoodsReceiveItemLocation_Index { get; set; }

        public Guid TagItem_Index { get; set; }

        public Guid Tag_Index { get; set; }

        public string Tag_No { get; set; }

        public Guid Product_Index { get; set; }

        public string Product_Id { get; set; }


        public string Product_Name { get; set; }


        public string Product_SecondName { get; set; }


        public string Product_ThirdName { get; set; }

        public string Product_Lot { get; set; }

        public Guid ItemStatus_Index { get; set; }

        public string ItemStatus_Id { get; set; }

        public string ItemStatus_Name { get; set; }

        public DateTime? GoodsReceive_MFG_Date { get; set; }

        public DateTime? GoodsReceive_EXP_Date { get; set; }

        public Guid GoodsReceive_ProductConversion_Index { get; set; }

        public string GoodsReceive_ProductConversion_Id { get; set; }

        public string GoodsReceive_ProductConversion_Name { get; set; }


        public decimal? BinBalance_Ratio { get; set; }


        public decimal? BinBalance_QtyBegin { get; set; }


        public decimal? BinBalance_WeightBegin { get; set; }

        public Guid? BinBalance_WeightBegin_Index { get; set; }


        public string BinBalance_WeightBegin_Id { get; set; }


        public string BinBalance_WeightBegin_Name { get; set; }


        public decimal? BinBalance_WeightBeginRatio { get; set; }


        public decimal? BinBalance_NetWeightBegin { get; set; }

        public Guid? BinBalance_NetWeightBegin_Index { get; set; }


        public string BinBalance_NetWeightBegin_Id { get; set; }


        public string BinBalance_NetWeightBegin_Name { get; set; }


        public decimal? BinBalance_NetWeightBeginRatio { get; set; }


        public decimal? BinBalance_GrsWeightBegin { get; set; }

        public Guid? BinBalance_GrsWeightBegin_Index { get; set; }


        public string BinBalance_GrsWeightBegin_Id { get; set; }


        public string BinBalance_GrsWeightBegin_Name { get; set; }


        public decimal? BinBalance_GrsWeightBeginRatio { get; set; }


        public decimal? BinBalance_WidthBegin { get; set; }

        public Guid? BinBalance_WidthBegin_Index { get; set; }


        public string BinBalance_WidthBegin_Id { get; set; }


        public string BinBalance_WidthBegin_Name { get; set; }


        public decimal? BinBalance_WidthBeginRatio { get; set; }


        public decimal? BinBalance_LengthBegin { get; set; }

        public Guid? BinBalance_LengthBegin_Index { get; set; }


        public string BinBalance_LengthBegin_Id { get; set; }


        public string BinBalance_LengthBegin_Name { get; set; }


        public decimal? BinBalance_LengthBeginRatio { get; set; }


        public decimal? BinBalance_HeightBegin { get; set; }

        public Guid? BinBalance_HeightBegin_Index { get; set; }


        public string BinBalance_HeightBegin_Id { get; set; }


        public string BinBalance_HeightBegin_Name { get; set; }


        public decimal? BinBalance_HeightBeginRatio { get; set; }


        public decimal? BinBalance_UnitVolumeBegin { get; set; }


        public decimal? BinBalance_VolumeBegin { get; set; }


        public decimal? BinBalance_QtyBal { get; set; }


        public decimal? BinBalance_UnitWeightBal { get; set; }

        public Guid? BinBalance_UnitWeightBal_Index { get; set; }


        public string BinBalance_UnitWeightBal_Id { get; set; }


        public string BinBalance_UnitWeightBal_Name { get; set; }


        public decimal? BinBalance_UnitWeightBalRatio { get; set; }


        public decimal? BinBalance_WeightBal { get; set; }

        public Guid? BinBalance_WeightBal_Index { get; set; }


        public string BinBalance_WeightBal_Id { get; set; }


        public string BinBalance_WeightBal_Name { get; set; }


        public decimal? BinBalance_WeightBalRatio { get; set; }


        public decimal? BinBalance_UnitNetWeightBal { get; set; }

        public Guid? BinBalance_UnitNetWeightBal_Index { get; set; }


        public string BinBalance_UnitNetWeightBal_Id { get; set; }


        public string BinBalance_UnitNetWeightBal_Name { get; set; }


        public decimal? BinBalance_UnitNetWeightBalRatio { get; set; }


        public decimal? BinBalance_NetWeightBal { get; set; }

        public Guid? BinBalance_NetWeightBal_Index { get; set; }


        public string BinBalance_NetWeightBal_Id { get; set; }


        public string BinBalance_NetWeightBal_Name { get; set; }


        public decimal? BinBalance_NetWeightBalRatio { get; set; }


        public decimal? BinBalance_UnitGrsWeightBal { get; set; }

        public Guid? BinBalance_UnitGrsWeightBal_Index { get; set; }


        public string BinBalance_UnitGrsWeightBal_Id { get; set; }


        public string BinBalance_UnitGrsWeightBal_Name { get; set; }


        public decimal? BinBalance_UnitGrsWeightBalRatio { get; set; }


        public decimal? BinBalance_GrsWeightBal { get; set; }

        public Guid? BinBalance_GrsWeightBal_Index { get; set; }


        public string BinBalance_GrsWeightBal_Id { get; set; }


        public string BinBalance_GrsWeightBal_Name { get; set; }


        public decimal? BinBalance_GrsWeightBalRatio { get; set; }


        public decimal? BinBalance_UnitWidthBal { get; set; }

        public Guid? BinBalance_UnitWidthBal_Index { get; set; }


        public string BinBalance_UnitWidthBal_Id { get; set; }


        public string BinBalance_UnitWidthBal_Name { get; set; }


        public decimal? BinBalance_UnitWidthBalRatio { get; set; }


        public decimal? BinBalance_WidthBal { get; set; }

        public Guid? BinBalance_WidthBal_Index { get; set; }


        public string BinBalance_WidthBal_Id { get; set; }


        public string BinBalance_WidthBal_Name { get; set; }


        public decimal? BinBalance_WidthBalRatio { get; set; }


        public decimal? BinBalance_UnitLengthBal { get; set; }

        public Guid? BinBalance_UnitLengthBal_Index { get; set; }


        public string BinBalance_UnitLengthBal_Id { get; set; }


        public string BinBalance_UnitLengthBal_Name { get; set; }


        public decimal? BinBalance_UnitLengthBalRatio { get; set; }


        public decimal? BinBalance_LengthBal { get; set; }

        public Guid? BinBalance_LengthBal_Index { get; set; }


        public string BinBalance_LengthBal_Id { get; set; }


        public string BinBalance_LengthBal_Name { get; set; }


        public decimal? BinBalance_LengthBalRatio { get; set; }


        public decimal? BinBalance_UnitHeightBal { get; set; }

        public Guid? BinBalance_UnitHeightBal_Index { get; set; }


        public string BinBalance_UnitHeightBal_Id { get; set; }


        public string BinBalance_UnitHeightBal_Name { get; set; }


        public decimal? BinBalance_UnitHeightBalRatio { get; set; }


        public decimal? BinBalance_HeightBal { get; set; }

        public Guid? BinBalance_HeightBal_Index { get; set; }


        public string BinBalance_HeightBal_Id { get; set; }


        public string BinBalance_HeightBal_Name { get; set; }


        public decimal? BinBalance_HeightBalRatio { get; set; }


        public decimal? BinBalance_UnitVolumeBal { get; set; }


        public decimal? BinBalance_VolumeBal { get; set; }


        public decimal? BinBalance_QtyReserve { get; set; }


        public decimal? BinBalance_WeightReserve { get; set; }

        public Guid? BinBalance_WeightReserve_Index { get; set; }


        public string BinBalance_WeightReserve_Id { get; set; }


        public string BinBalance_WeightReserve_Name { get; set; }


        public decimal? BinBalance_WeightReserveRatio { get; set; }


        public decimal? BinBalance_NetWeightReserve { get; set; }

        public Guid? BinBalance_NetWeightReserve_Index { get; set; }


        public string BinBalance_NetWeightReserve_Id { get; set; }


        public string BinBalance_NetWeightReserve_Name { get; set; }


        public decimal? BinBalance_NetWeightReserveRatio { get; set; }


        public decimal? BinBalance_GrsWeightReserve { get; set; }

        public Guid? BinBalance_GrsWeightReserve_Index { get; set; }


        public string BinBalance_GrsWeightReserve_Id { get; set; }


        public string BinBalance_GrsWeightReserve_Name { get; set; }


        public decimal? BinBalance_GrsWeightReserveRatio { get; set; }


        public decimal? BinBalance_WidthReserve { get; set; }

        public Guid? BinBalance_WidthReserve_Index { get; set; }


        public string BinBalance_WidthReserve_Id { get; set; }


        public string BinBalance_WidthReserve_Name { get; set; }


        public decimal? BinBalance_WidthReserveRatio { get; set; }


        public decimal? BinBalance_LengthReserve { get; set; }

        public Guid? BinBalance_LengthReserve_Index { get; set; }


        public string BinBalance_LengthReserve_Id { get; set; }


        public string BinBalance_LengthReserve_Name { get; set; }


        public decimal? BinBalance_LengthReserveRatio { get; set; }


        public decimal? BinBalance_HeightReserve { get; set; }

        public Guid? BinBalance_HeightReserve_Index { get; set; }


        public string BinBalance_HeightReserve_Id { get; set; }


        public string BinBalance_HeightReserve_Name { get; set; }


        public decimal? BinBalance_HeightReserveRatio { get; set; }


        public decimal? BinBalance_UnitVolumeReserve { get; set; }


        public decimal? BinBalance_VolumeReserve { get; set; }

        public Guid? ProductConversion_Index { get; set; }

        public string ProductConversion_Id { get; set; }

        public string ProductConversion_Name { get; set; }


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


        public string IsUse { get; set; }

        public int? BinBalance_Status { get; set; }


        public string Invoice_No { get; set; }


        public string Declaration_No { get; set; }


        public string HS_Code { get; set; }


        public string Conutry_of_Origin { get; set; }


        public decimal? Tax1 { get; set; }

        public Guid? Tax1_Currency_Index { get; set; }


        public string Tax1_Currency_Id { get; set; }


        public string Tax1_Currency_Name { get; set; }


        public decimal? Tax2 { get; set; }

        public Guid? Tax2_Currency_Index { get; set; }


        public string Tax2_Currency_Id { get; set; }


        public string Tax2_Currency_Name { get; set; }


        public decimal? Tax3 { get; set; }

        public Guid? Tax3_Currency_Index { get; set; }


        public string Tax3_Currency_Id { get; set; }


        public string Tax3_Currency_Name { get; set; }


        public decimal? Tax4 { get; set; }

        public Guid? Tax4_Currency_Index { get; set; }


        public string Tax4_Currency_Id { get; set; }


        public string Tax4_Currency_Name { get; set; }

        public decimal? Tax5 { get; set; }

        public Guid? Tax5_Currency_Index { get; set; }


        public string Tax5_Currency_Id { get; set; }


        public string Tax5_Currency_Name { get; set; }

        public string ERP_Location { get; set; }
    }
}
