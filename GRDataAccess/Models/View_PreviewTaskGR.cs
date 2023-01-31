using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GRDataAccess.Models
{
  

    public partial class View_PreviewTaskGR
    {
        [Key]
        public long? RowIndex { get; set; }

        public Guid? TaskGR_Index { get; set; }
        public Guid? TaskGRItem_Index { get; set; }

        public string TaskGR_No { get; set; }

        public Guid? Ref_Document_Index { get; set; }
        public string Ref_Document_No { get; set; }

        public string Tag_No { get; set; }
        public string UserAssign { get; set; }
        public string Create_By { get; set; }
        public DateTime Create_Date { get; set; }
        public string Update_By { get; set; }
        public Guid? Product_Index { get; set; }
        public string Product_Id { get; set; }
        public string Product_Name { get; set; }
        public string Product_SecondName { get; set; }
        public Guid? ProductConversion_Index { get; set; }

        public string ProductConversion_Id { get; set; }

        public string ProductConversion_Name { get; set; }

        public Guid? Location_Index { get; set; }
        public string Location_Id { get; set; }
        public string Location_Name { get; set; }
        public decimal? Qty { get; set; }
        public string Assign_By { get; set; }

        
    }
}
