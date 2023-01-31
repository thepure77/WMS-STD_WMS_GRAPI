using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MasterDataBusiness.Product
{
    public class BarcodeViewModel
    {
        [Key]
        public Guid? product_Index { get; set; }

        public string product_Id { get; set; }

        public string product_Name { get; set; }

        public string product_SecondName { get; set; }

        public string product_ThirdName { get; set; }

        public string Ref_No2 { get; set; }
        public string Ref_No3 { get; set; }

        public decimal? productConversion_Width { get; set; }

        public decimal? productConversion_Length { get; set; }

        public decimal? productConversion_Height { get; set; }

        public string productConversionBarcode { get; set; }

        public Guid volume_Index { get; set; }

        public string volume_Id { get; set; }

        public string volume_Name { get; set; }

        public decimal? volume_Ratio { get; set; }
        public int? isLot { get; set; }
        public int? isExpDate { get; set; }
        public int? isMfgDate { get; set; }
        public int? isSerial { get; set; }
        public int? ProductItemLife_D { get; set; }
        public int? ProductItemLife_Y { get; set; }
        public int? ProductItemLife_M { get; set; }
        public decimal? productConversion_Ratio { get; set; }
        public Guid? productConversion_Index { get; set; }
        public string productConversion_Id { get; set; }
        public string productConversion_Name { get; set; }
        public string tihi { get; set; }
        public decimal? qty_Per_Tag { get; set; }
    }
}
