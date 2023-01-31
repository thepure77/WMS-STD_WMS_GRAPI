
using DataAccess;
using GRDataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace GRBusiness.GoodsReceive
{
    public class Matdoc_lineproductionService
    {
        private GRDbContext db;


        public Matdoc_lineproductionService()
        {
            db = new GRDbContext();


        }

        public Matdoc_lineproductionService(GRDbContext db)
        {
            this.db = db;

        }
        
        public ViewModel_updateMatdoc update_lineproduction_select()
        {
            logtxt LoggingService = new logtxt();
            ViewModel_updateMatdoc result = new ViewModel_updateMatdoc();
            try
            {
                LoggingService.DataLogLines("update_lineproduction", "update_lineproduction", "Start Checkout" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                List<View_Matdoc_Update_lineproduction> get_updateMatdoc = db.View_Matdoc_Update_lineproduction.ToList();
                LoggingService.DataLogLines("update_lineproduction", "update_lineproduction", " select : " + JsonConvert.SerializeObject(get_updateMatdoc) +" "+ DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                get_updateMatdoc = get_updateMatdoc.OrderBy(c => c.Product_Id).ThenBy(c => c.Product_Lot).ToList();

                foreach (var item in get_updateMatdoc)
                {
                    ViewModel_updateMatdoc model = new ViewModel_updateMatdoc();

                    model.Product_Id=item.Product_Id;
                    model.Product_Lot=item.Product_Lot;
                    model.Remark=item.Remark;
                    model.CountMD=item.CountMD;
                    model.CountRobotReceive=item.CountRobotReceive;
                    model.CountTAG_Putaway=item.CountTAG_Putaway;
                    model.CountCalSAP=item.CountCalSAP;
                    model.IN_QTY_Putaway=item.IN_QTY_Putaway;
                    model.IN_QTY_SAP=item.IN_QTY_SAP;
                    model.IN_QTY_PartialPallet=item.IN_QTY_PartialPallet;
                    model.IN_Ratio=item.IN_Ratio;
                    model.Create_Date=item.Create_Date;
                    model.Ref_Document_No=item.Ref_Document_No;
                    model.Ref_Document_Index=item.Ref_Document_Index;
                    model.Ref_DocumentItem_Index=item.Ref_DocumentItem_Index;
                    model.Is_Amz=item.Is_Amz;
                    result.list.Add(model);               
                }
                result.resultIsUse = true;

                return result;
            }
            catch (Exception ex)
            {
                LoggingService.DataLogLines("update_lineproduction", "update_lineproduction", " ex : " + JsonConvert.SerializeObject(ex) + " " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                result.resultIsUse = false;
                result.resultMsg = ex.Message;
                return result;
            }
        }

        public ViewModel_updateMatdoc update_lineproduction_update()
        {
            db.Database.SetCommandTimeout(360);
            logtxt LoggingService = new logtxt();
            ViewModel_updateMatdoc result = new ViewModel_updateMatdoc();
            try
            {
                LoggingService.DataLogLines("update_lineproduction", "update_lineproduction", "Start Checkout" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                List<View_Matdoc_Update_lineproduction> get_updateMatdoc = db.View_Matdoc_Update_lineproduction.ToList();
                LoggingService.DataLogLines("update_lineproduction", "update_lineproduction", " select : " + JsonConvert.SerializeObject(get_updateMatdoc) + " " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                get_updateMatdoc = get_updateMatdoc.OrderBy(c => c.Product_Id).ThenBy(c => c.Product_Lot).ToList();

                foreach (var item in get_updateMatdoc)
                {
                    LoggingService.DataLogLines("update_lineproduction", "update_lineproduction", " update : " + JsonConvert.SerializeObject(item) + " " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));

                    var Product_Id = new SqlParameter("@Product_Id", item.Product_Id);
                    var Product_Lot = new SqlParameter("@Product_Lot", item.Product_Lot);
                    var update_lineproduction = db.Database.ExecuteSqlCommand("EXEC sp_UpdateMatdoc_lineproduction @Product_Id,@Product_Lot", Product_Id, Product_Lot);
                }
                result.resultIsUse = true;

                return result;
            }
            catch (Exception ex)
            {
                LoggingService.DataLogLines("update_lineproduction", "update_lineproduction", " ex : " + JsonConvert.SerializeObject(ex) + " " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                result.resultIsUse = false;
                result.resultMsg = ex.Message;
                return result;
            }
        }
    }
}
