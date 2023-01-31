using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GRDataAccess.Models
{
    public class View_CheckPlanGRScanGR
    {
        [Key]
        public Guid GoodsReceive_Index { get; set; }

     
        public string GoodsReceive_No { get; set; }


        public string Ref_Document_No { get; set; }

        public Guid? Ref_Document_Index { get; set; }

    }
}
