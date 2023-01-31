using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GRDataAccess.Models
{


    public partial class View_GoodsReceive
    {
        [Key]
        public Guid GoodsReceive_Index { get; set; }

        public Guid GoodsReceiveItem_Index { get; set; }

        public Guid GoodsReceiveItemLocation_Index { get; set; }
        public string GoodsReceive_No { get; set; }

        public DateTime? GoodsReceive_Date { get; set; }
        public Guid Owner_Index { get; set; }

        public string Owner_Id { get; set; }

        public string Owner_Name { get; set; }

        public Guid? DocumentType_Index { get; set; }
        
        public string DocumentType_Id { get; set; }

        
        public string DocumentType_Name { get; set; }

        public Guid? Product_Index { get; set; }

        
        public string Product_Id { get; set; }

        
        public string Product_Name { get; set; }

        
        public string Product_Lot { get; set; }

        
        public string Product_SecondName { get; set; }

        
        public string Product_ThirdName { get; set; }
        public Guid? ProductConversion_Index { get; set; }

        public string ProductConversion_Id { get; set; }

        public string ProductConversion_Name { get; set; }

        public decimal Qty { get; set; }

        public decimal Ratio { get; set; }

        public decimal TotalQty { get; set; }

        public decimal Volume { get; set; }

        public decimal Weight { get; set; }

        public Guid ItemStatus_Index { get; set; }

        public string ItemStatus_Id { get; set; }
     
        public string ItemStatus_Name { get; set; }

        public DateTime? MFG_Date { get; set; }

        public DateTime? EXP_Date { get; set; }

        public Guid? Location_Index { get; set; }

        
        public string Location_Id { get; set; }

        
        public string Location_Name { get; set; }
        public Guid? TagItem_Index { get; set; }

        public Guid? Tag_Index { get; set; }

        public string Tag_No { get; set; }

    }
}
