using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GRBusiness.GoodsReceive
{
  

    public partial class GoodsReceiveTagItemPutawayLpnViewModel
    {
        [Key]
        public Guid TagItemIndex { get; set; }

        public Guid? TagIndex { get; set; }
         

        public string TagNo { get; set; }

        public Guid? GoodsReceiveIndex { get; set; }

        public Guid? GoodsReceiveItemIndex { get; set; }

        public Guid ProductIndex { get; set; }



        public string ProductId { get; set; }



        public string ProductName { get; set; }


        public string ProductSecondName { get; set; }


        public string ProductThirdName { get; set; }


        public string ProductLot { get; set; }

        public Guid ItemStatusIndex { get; set; }


        public string ItemStatusId { get; set; }


        public string ItemStatusName { get; set; }


        public decimal? Qty { get; set; }


        public decimal Ratio { get; set; }


        public decimal TotalQty { get; set; }

        public Guid ProductConversionIndex { get; set; }



        public string ProductConversionId { get; set; }



        public string ProductConversionName { get; set; }


        public decimal? Weight { get; set; }


        public decimal? Volume { get; set; }


        public DateTime? MFGDate { get; set; }


        public DateTime? EXPDate { get; set; }

        public string TagRefNo1 { get; set; }


        public string TagRefNo2 { get; set; }


        public string TagRefNo3 { get; set; }


        public string TagRefNo4 { get; set; }


        public string TagRefNo5 { get; set; }

        public int? TagStatus { get; set; }


        public string UDF1 { get; set; }


        public string UDF2 { get; set; }


        public string UDF3 { get; set; }


        public string UDF4 { get; set; }


        public string UDF5 { get; set; }


        public string CreateBy { get; set; }


        public DateTime? CreateDate { get; set; }


        public string UpdateBy { get; set; }


        public DateTime? UpdateDate { get; set; }


        public string CancelBy { get; set; }


        public DateTime? CancelDate { get; set; }


        public string SuggestLocation { get; set; }


        public string PutAwayLocation { get; set; }

        public Guid LocationIndex { get; set; }

        public string LocationName { get; set; }

    }
}
