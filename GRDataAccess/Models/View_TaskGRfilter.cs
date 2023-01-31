using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GRDataAccess.Models
{
  

    public partial class View_TaskGRfilter
    {
        [Key]
        public Guid? TaskGR_Index { get; set; }
        public string TaskGR_No { get; set; }
        public string Ref_Document_No { get; set; }

        public Guid? Ref_Document_Index { get; set; }
        public string UserAssign { get; set; }
        public string Create_By { get; set; }


    }
}
