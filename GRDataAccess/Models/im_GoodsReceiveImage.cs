using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GRDataAccess.Models
{
    public partial class im_GoodsReceive_Image
    {
        [Key]
        public Guid GoodsReceiveImage_Index { get; set; }

        public Guid GoodsReceive_Index { get; set; }

        public string GoodsReceive_No { get; set; }

        public int Document_Status { get; set; }

        public string Create_By { get; set; }

        public DateTime? Create_Date { get; set; }
        public string Update_By { get; set; }

        public DateTime? Update_Date { get; set; }
        public string Cancel_By { get; set; }

        public DateTime? Cancel_Date { get; set; }
        public string GoodsReceiveImage_path { get; set; }
        public string GoodsReceiveImage_type { get; set; }
    }
}
