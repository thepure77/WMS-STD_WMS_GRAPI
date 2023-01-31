
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GRBusiness.GoodsReceive
{

    public partial class TagItemViewModel : Pagination
    {

        public Guid? TagIndex { get; set; }
        public Guid TagItemIndex { get; set; }

        public string GoodsReceiveNo { get; set; }
        public Guid? GoodsReceiveIndex { get; set; }
        public string ProductId { get; set; }
        public string ItemStatusName { get; set; }

        public string ProductName { get; set; }

        public string LPN { get; set; }

        public decimal? Qty { get; set; }

        public decimal? TotalQty { get; set; }

        public string Uom { get; set; }

        public string Remark { get; set; }

        public string CreateBy { get; set; }


        public DateTime? CreateDate { get; set; }


        public string UpdateBy { get; set; }


        public DateTime? UpdateDate { get; set; }


        public string CancelBy { get; set; }


        public DateTime? CancelDate { get; set; }

        public int? TagStatus { get; set; }

        public Guid? ProductIndex { get; set; }

        public string GoodsReceiveProductConversion_Name { get; set; }

        public Guid? GoodsReceiveProductConversion_Index { get; set; }

        public decimal ratio { get; set; }

        public bool isCheckQty { get; set; }

        public class actionResultTagItemViewModel
        {
            public IList<TagItemViewModel> items { get; set; }
            public Pagination pagination { get; set; }
        }


    }
}
