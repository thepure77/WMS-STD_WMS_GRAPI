using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GRDataAccess.Models
{
    public partial class View_Location_Alert_MSG
    {
        [Key]
        public Guid Location_Index { get; set; }
        public string Location_Id { get; set; }
        public string Location_Name { get; set; }
        public int? Location_Bay { get; set; }
        public int? Location_Depth { get; set; }
        public int? Location_Level { get; set; }
        public decimal? Max_Qty { get; set; }
        public decimal? Max_Weight { get; set; }
        public decimal? Max_Volume { get; set; }
        public decimal? Max_Pallet { get; set; }
        public int? PutAway_Seq { get; set; }
        public int? Picking_Seq { get; set; }
        public decimal? LocationVol_Height { get; set; }
        public decimal? LocationVol_Width { get; set; }
        public decimal? LocationVol_Depth { get; set; }
        public string Location_Aspect { get; set; }

        public Guid? Warehouse_Index { get; set; }
        public string Warehouse_Id { get; set; }
        public string Warehouse_Name { get; set; }

        public Guid? Room_Index { get; set; }
        public string Room_Id { get; set; }
        public string Room_Name { get; set; }

        public Guid? LocationType_Index { get; set; }
        public string LocationType_Id { get; set; }
        public string LocationType_Name { get; set; }

        public Guid? LocationAisle_Index { get; set; }
        public string LocationLock_Id { get; set; }
        public string LocationLock_Name { get; set; }


        public int? IsActive { get; set; }
        public int? IsDelete { get; set; }

        public int? BlockPut { get; set; }
        public int? BlockPick { get; set; }

        public string Document_Remark { get; set; }

        public string MSG { get; set; }
    }
}
