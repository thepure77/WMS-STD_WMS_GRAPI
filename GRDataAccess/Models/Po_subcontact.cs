using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GRDataAccess.Models
{
    public partial class Po_subcontact
    {
        [Key]
        public Guid Po_Index { get; set; }
        public Guid? GoodsReceive_Index { get; set; }

        public Guid? GoodsReceiveItem_Index { get; set; }

        public Guid? GoodsIssueItem_Index { get; set; }

        public Guid? GoodsIssue_Index { get; set; }

        [StringLength(200)]
        public string GoodsIssue_No { get; set; }

        public Guid? DocumentType_Index { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Qty { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Ratio { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? TotalQty { get; set; }

        [StringLength(50)]
        public string DocumentType_Id { get; set; }

        [StringLength(200)]
        public string DocumentType_Name { get; set; }

        [StringLength(50)]
        public string GoodsReceive_No { get; set; }

        
        public Guid Product_Index { get; set; }

        [StringLength(50)]
        public string Product_Id { get; set; }

        [StringLength(200)]
        public string Product_Name { get; set; }

        public Guid? ProductConversion_Index { get; set; }

        [StringLength(50)]
        public string ProductConversion_Id { get; set; }

        [StringLength(200)]
        public string ProductConversion_Name { get; set; }

        [StringLength(50)]
        public string ERP_Location { get; set; }

        public int? IsActive { get; set; }

        public int? IsDelete { get; set; }

        public int? IsDelete_Plant { get; set; }

        public int? IsSystem { get; set; }

        public int? Status_Id { get; set; }

        [StringLength(200)]
        public string Create_By { get; set; }

        public DateTime? Create_Date { get; set; }

        [StringLength(200)]
        public string Update_By { get; set; }

        public DateTime? Update_Date { get; set; }

        [StringLength(200)]
        public string Cancel_By { get; set; }

        public DateTime? Cancel_Date { get; set; }
    }
}
