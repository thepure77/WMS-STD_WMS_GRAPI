using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GRDataAccess.Models
{
    public partial class View_CheckDocumentStatus
    {
        [Key]
        public Guid? GoodsReceive_Index { get; set; }
        public string PlanGoodsReceive_No { get; set; }

        public int PlanGRDocument_Status { get; set; }

        public int GRDocument_Status { get; set; }
    }
}
