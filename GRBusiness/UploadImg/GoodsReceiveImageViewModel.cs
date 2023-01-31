using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GRBusiness.GoodsReceiveImage
{
    public class GoodsReceiveImageViewModel
    {
        public Guid goodsReceiveImage_Index { get; set; }

        public Guid goodsReceive_Index { get; set; }

        public string goodsReceive_No { get; set; }

        public int document_Status { get; set; }

        public string create_By { get; set; }

        public DateTime? create_Date { get; set; }
        public string update_By { get; set; }

        public DateTime? update_Date { get; set; }
        public string cancel_By { get; set; }

        public DateTime? cancel_Date { get; set; }
        public string goodsReceiveImage_path { get; set; }
        public string goodsReceiveImage_type { get; set; }

        public string name { get; set; }
        public string base64 { get; set; }
        public string type { get; set; }
        public string src { get; set; }
    }
}
