using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GRDataAccess.Models
{
  

    public partial class View_TagPutaway
    {
        [Key]
        public long? RowIndex { get; set; }

        
        public Guid? GoodsReceive_Index { get; set; }

        public Guid? Tag_Index { get; set; }

        public string Tag_No { get; set; }

        public Guid Product_Index { get; set; }

        public string Product_Id { get; set; }

        public string Product_Name { get; set; }

        public string Location_Name { get; set; }

        public string LocationType_Name { get; set; }

        public string ItemStatus_Name { get; set; }

        public int Putaway_Status { get; set; }

        public string putaway_By { get; set; }

    }
}
