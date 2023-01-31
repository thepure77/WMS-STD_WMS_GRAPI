using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GRDataAccess.Models
{
    public partial class IM_GoodsReceive
    {
        [Key]
        public Guid? GoodsReceive_Index { get; set; }

        public Guid Owner_Index { get; set; }


        public string Owner_Id { get; set; }


        public string Owner_Name { get; set; }

        public Guid? DocumentType_Index { get; set; }


        public string DocumentType_Id { get; set; }


        public string DocumentType_Name { get; set; }


        public string GoodsReceive_No { get; set; }

        public DateTime? GoodsReceive_Date { get; set; }

        public DateTime? GoodsReceive_Due_Date { get; set; }


        public string DocumentRef_No1 { get; set; }


        public string DocumentRef_No2 { get; set; }


        public string DocumentRef_No3 { get; set; }


        public string DocumentRef_No4 { get; set; }


        public string DocumentRef_No5 { get; set; }

        public int? Document_Status { get; set; }


        public string Document_Remark { get; set; }


        public string UDF_1 { get; set; }


        public string UDF_2 { get; set; }


        public string UDF_3 { get; set; }


        public string UDF_4 { get; set; }


        public string UDF_5 { get; set; }

        public int? DocumentPriority_Status { get; set; }


        public string Create_By { get; set; }

        public DateTime? Create_Date { get; set; }


        public string Update_By { get; set; }

        public DateTime? Update_Date { get; set; }


        public string Cancel_By { get; set; }

        public DateTime? Cancel_Date { get; set; }

        public int? Putaway_Status { get; set; }

        public Guid? Warehouse_Index { get; set; }


        public string Warehouse_Id { get; set; }


        public string Warehouse_Name { get; set; }

        public Guid? Warehouse_Index_To { get; set; }


        public string Warehouse_Id_To { get; set; }


        public string Warehouse_Name_To { get; set; }

        public Guid? DockDoor_Index { get; set; }


        public string DockDoor_Id { get; set; }


        public string DockDoor_Name { get; set; }

        public Guid? VehicleType_Index { get; set; }


        public string VehicleType_Id { get; set; }


        public string VehicleType_Name { get; set; }

        public Guid? ContainerType_Index { get; set; }


        public string ContainerType_Id { get; set; }


        public string ContainerType_Name { get; set; }


        public string UserAssign { get; set; }


        public string Invoice_No { get; set; }

        public Guid? Vendor_Index { get; set; }


        public string Vendor_Id { get; set; }


        public string Vendor_Name { get; set; }

        public Guid? WHOwner_Index { get; set; }


        public string WHOwner_Id { get; set; }


        public string WHOwner_Name { get; set; }


        public string License_Name { get; set; }

        public Guid? Forwarder_Index { get; set; }


        public string Forwarder_Id { get; set; }


        public string Forwarder_Name { get; set; }

        public Guid? ShipmentType_Index { get; set; }


        public string ShipmentType_Id { get; set; }


        public string ShipmentType_Name { get; set; }

        public Guid? CargoType_Index { get; set; }


        public string CargoType_Id { get; set; }


        public string CargoType_Name { get; set; }

        public Guid? UnloadingType_Index { get; set; }


        public string UnloadingType_Id { get; set; }


        public string UnloadingType_Name { get; set; }




        public string Container_No1 { get; set; }


        public string Container_No2 { get; set; }


        public string Labur { get; set; }
        public string Checker_Name { get; set; }
        public string Driver_Name { get; set; }

        public Guid? CostCenter_Index { get; set; }

        public string CostCenter_Id { get; set; }

        public string CostCenter_Name { get; set; }

        public int? Is_Amz { get; set; }
    }
}
