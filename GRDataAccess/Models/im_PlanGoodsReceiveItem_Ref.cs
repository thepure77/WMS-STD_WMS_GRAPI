using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GRDataAccess.Models
{
    public class im_PlanGoodsReceiveItem_Ref
    {
        [Key]
        public Guid PlanGoodsReceiveItem_Ref_Index { get; set; }

        public Guid PlanGoodsReceive_Ref_Index { get; set; }
        public string SAP_Id { get; set; }
        public string ITEM_CAT { get; set; }
        public string HIGH_LV_ITEM { get; set; }
        public string Plant_Id { get; set; }
        public string Sloc_Id { get; set; }
        public decimal Plan_QTY { get; set; }
        public string WMS_GET_FLAG { get; set; }
        public string SALE_UNIT { get; set; }
        public string MSG_TYPE { get; set; }
        public string MSG_TEXT { get; set; }
        public string Document_Remark { get; set; }
        public string CRE_DATE { get; set; }
        public string CRE_TIME { get; set; }
        public string CRE_BY { get; set; }
        public string PROC_DATE { get; set; }
        public string PROC_TIME { get; set; }
        public string CHG_DATE { get; set; }
        public string CHG_TIME { get; set; }
        public string CHG_BY { get; set; }
        public string Document_Status { get; set; }
        public string GR_RCPT { get; set; }
        public string MovementType_Id { get; set; }
        public string soldTo_Id { get; set; }
        public string WareHouse_Id { get; set; }
        public string RECEI_SLOC { get; set; }
        public string MAT_GRP { get; set; }
        public string MAT_GPNM { get; set; }
        public string IO { get; set; }
        public string ORDER_NAME { get; set; }
        public string CostCenter_Id { get; set; }
        public string CostCenter_Name { get; set; }
        public string TARGET_QTY { get; set; }
        public string PROF_NO { get; set; }
        public string PROF_NAME { get; set; }
        public string COMP_NO { get; set; }
        public string COMP_MAT { get; set; }
        public string COMP_NAME { get; set; }
        public string COMP_PLANT{ get; set; }
        public string COMP_SLOC { get; set; }
        public string COMP_QTY_BASE{ get; set; }
        public string COMP_UOM_BASE { get; set; }
        public string COMP_QTY { get; set; }
        public string COMP_UOM { get; set; }
        public string COLLECT_NO { get; set; }
        public string CREATE_ASSET { get; set; }
        public string ASSET_NO { get; set; }
        public string ORDERID { get; set; }
        public string UNLOAD_PT{ get; set; }
        public string STORE_PBL { get; set; }
        public string STORE_COSTCENTER { get; set; }
        public string ITEM_TEXT { get; set; }
        public string ZLAST_FLAG { get; set; }
    }
}
