using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GRBusiness.GoodsReceive
{
    public partial class GoodsReceiveModel
    {
        [Key]
        public Guid? GoodsReceiveIndex { get; set; }

        public Guid OwnerIndex { get; set; }

        public Guid GoodsReceiveItemIndex { get; set; }



        public string OwnerId { get; set; }


        public string OwnerName { get; set; }

        public Guid? DocumentTypeIndex { get; set; }


        public string DocumentTypeId { get; set; }


        public string DocumentTypeName { get; set; }


        public string GoodsReceiveNo { get; set; }

        public string GoodsReceiveDate { get; set; }


        public string DocumentRefNo1 { get; set; }


        public string DocumentRefNo2 { get; set; }


        public string DocumentRefNo3 { get; set; }


        public string DocumentRefNo4 { get; set; }


        public string DocumentRefNo5 { get; set; }

        public int? DocumentStatus { get; set; }


        public string DocumentRemark { get; set; }


        public string UDF1 { get; set; }


        public string UDF2 { get; set; }


        public string UDF3 { get; set; }


        public string UDF4 { get; set; }


        public string UDF5 { get; set; }

        public int? DocumentPriorityStatus { get; set; }


        public string Create_By { get; set; }


        public string Create_Date { get; set; }


        public string Update_By { get; set; }


        public string Update_Date { get; set; }


        public string Cancel_By { get; set; }


        public string Cancel_Date { get; set; }

        public Guid? WarehouseIndex { get; set; }


        public string WarehouseId { get; set; }


        public string WarehouseName { get; set; }


        public Guid? WarehouseIndexTo { get; set; }


        public string WarehouseIdTo { get; set; }


        public string WarehouseNameTo { get; set; }

        public int? PutawayStatus { get; set; }

        public Guid? DockDoorIndex { get; set; }


        public string DockDoorId { get; set; }


        public string DockDoorName { get; set; }

        public Guid? VehicleTypeIndex { get; set; }


        public string VehicleTypeId { get; set; }


        public string VehicleTypeName { get; set; }

        public Guid? ContainerTypeIndex { get; set; }


        public string ContainerTypeId { get; set; }


        public string ContainerTypeName { get; set; }

        public Guid? RefDocumentIndex { get; set; }

        public string RefDocumentNo { get; set; }

        public Guid ProductIndex { get; set; }

        public string ProductId { get; set; }

        public string ProductName { get; set; }

        public string ProductSecondName { get; set; }

        public string ProductThirdName { get; set; }

        public string ProcessStatusName { get; set; }
        public int? TagStatus { get; set; }
        public string UserAssign { get; set; }

       
    }
}
