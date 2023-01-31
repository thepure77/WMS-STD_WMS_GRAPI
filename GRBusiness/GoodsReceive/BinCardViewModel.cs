using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GRBusiness.GoodsReceive
{

    public partial class BinCardViewModel
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


        public decimal? BinCard_WeightIn { get; set; }


        public decimal? BinCard_WeightOut { get; set; }


        public decimal? BinCard_WeightSign { get; set; }


        public decimal? BinCard_VolumeIn { get; set; }


        public decimal? BinCard_VolumeOut { get; set; }


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

    }
}
