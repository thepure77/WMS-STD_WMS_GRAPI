using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GRDataAccess.Models
{
    public partial class View_CheckLocation
    {
        [Key]
        public Guid Location_Index { get; set; }

        public Guid Warehouse_Index { get; set; }

        public Guid Room_Index { get; set; }

        public Guid LocationType_Index { get; set; }

        public string Location_Id { get; set; }

        public string Location_Name { get; set; }

        public Guid? LocationAisle_Index { get; set; }

        public int? Location_Bay { get; set; }

        public int? Location_Depth { get; set; }

        public int? Location_Level { get; set; }

        public decimal? Max_Qty { get; set; }

        public decimal? Max_Weight { get; set; }

        public decimal? Max_Volume { get; set; }

        public decimal? Max_Pallet { get; set; }

        public int? PutAway_Seq { get; set; }

        public int? Picking_Seq { get; set; }

        public int? IsActive { get; set; }

        public int? IsDelete { get; set; }

        public int? IsSystem { get; set; }

        public int? Status_Id { get; set; }

        public string Create_By { get; set; }

        public DateTime? Create_Date { get; set; }

        public string Update_By { get; set; }

        public DateTime? Update_Date { get; set; }

        public string Cancel_By { get; set; }

        public DateTime? Cancel_Date { get; set; }

        public decimal? LocationVol_Height { get; set; }

        public decimal? LocationVol_Width { get; set; }

        public decimal? LocationVol_Depth { get; set; }

        public string Location_Aspect { get; set; }

        public string Location_Aisle { get; set; }

        public int? BlockPut { get; set; }

        public int? BlockPick { get; set; }

        public string Location_Bay_Desc { get; set; }

        public string Location_Level_Desc { get; set; }

        public int? Location_Position { get; set; }

        public string Location_Position_Desc { get; set; }

        public string Location_Prefix_Desc { get; set; }

        public string Ref_No1 { get; set; }

        public string Ref_No2 { get; set; }

        public string Ref_No3 { get; set; }

        public string Ref_No4 { get; set; }

        public string Ref_No5 { get; set; }

        public string Document_Remark { get; set; }
    }
}
