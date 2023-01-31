using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GRDataAccess.Models
{
  

    public partial class View_GoodsReceivePop
    {
        [Key]
        public long? RowIndex { get; set; }

        public Guid GoodsReceive_Index { get; set; }


        public string GoodsReceive_No { get; set; }

        public DateTime? GoodsReceive_Date { get; set; }

        public Guid? Owner_Index { get; set; }

        public string Owner_Id { get; set; }

        public string Owner_Name { get; set; }

        public int? Document_Status { get; set; }

        public Guid? DocumentType_Index { get; set; }

        public string DocumentType_Id { get; set; }

        public string DocumentType_Name { get; set; }


        public decimal? Qty { get; set; }

        public string Create_By { get; set; }

        public DateTime? Create_Date { get; set; }

    }
}
