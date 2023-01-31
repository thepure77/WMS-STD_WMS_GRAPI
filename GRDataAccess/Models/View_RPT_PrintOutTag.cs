using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GRDataAccess.Models
{


    public partial class View_RPT_PrintOutTag
    {
        [Key]
        public long? Row_Index { get; set; }
        public Guid? GoodsReceive_Index { get; set; }
        public Guid? GoodsReceiveItem_Index { get; set; }
        public string GoodsReceive_No { get; set; }
        public Guid? Product_Index { get; set; }
        public string Product_Id { get; set; }
        public string Product_Name { get; set; }
        public Guid? Owner_Index { get; set; }
        public string Owner_Id { get; set; }
        public string Owner_Name { get; set; }
        public Guid? Warehouse_Index { get; set; }
        public string Warehouse_Id { get; set; }
        public string Warehouse_Name { get; set; }
        public Guid? Location_Index { get; set; }
        public string Location_Id { get; set; }
        public string Location_Name { get; set; }
        public Guid? ProductConversion_Index { get; set; }
        public string ProductConversion_Id { get; set; }
        public string ProductConversion_Name { get; set; }
        public string Tag_No { get; set; }
        public decimal? Qty { get; set; }
        public DateTime? GoodsReceive_Date { get; set; }
        public string Ref_Document_No { get; set; }
        //public Guid? Vendor_Index { get; set; }
        //public string Vendor_Id { get; set; }
        //public string Vendor_Name { get; set; }

        // Add new
        public string ProductionLineNo { get; set; }
        public string PalletID { get; set; }
        public string SKU { get; set; }
        public string SKUBarcode { get; set; }
        public string IsLastCarton { get; set; }
        public string Description { get; set; }
        public string MainType { get; set; }
        public string QuantityInCRT { get; set; }
        public string QuantityInPC { get; set; }
        public string MFGDate { get; set; }
        public string EXPDate { get; set; }
        public string LotNo { get; set; }
        public string RefDoc { get; set; }
        public int CartonPerPallet { get; set; }
        public int Ti { get; set; }
        public int Hi { get; set; }
        public string ReceiverDate { get; set; }
        public string Receiver { get; set; }
        public string Status { get; set; }
        public DateTime? SapCreateDT { get; set; }
        public DateTime? ProductionEndDT { get; set; }
        public int SaleQty { get; set; }
        public string SaleUnit { get; set; }
        public DateTime? GiBeforeDate { get; set; }
        public string BatchNo { get; set; }
        public string Type { get; set; }
        public int UnitOnPallet { get; set; }
        public decimal PalletWT { get; set; }
        public string BU { get; set; }
        public string Supplier { get; set; }
        public string Remark { get; set; }
        public int QtyInUnit { get; set; }
        public string UnitOfInUnit { get; set; }
        public int QtyPOUnit { get; set; }
        public string UnitOfPOUnit { get; set; }

    }
}
