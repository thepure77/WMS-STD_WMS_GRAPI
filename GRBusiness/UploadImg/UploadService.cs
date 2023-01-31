using Comone.Utils;
using DataAccess;
using GRBusiness.GoodsReceive;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using static GRBusiness.LPN.LPNViewModel;
using GRDataAccess.Models;
using GRBusiness.ConfigModel;
using Business.Library;
using System.Reflection;
using static GRBusiness.GoodsReceive.GoodsReceiveDocViewModel;
using GRBusiness.GoodsReceiveImage;
using System.IO;

namespace GRBusiness.Upload
{
    public class UploadService
    {
        private GRDbContext db;

        public UploadService()
        {
            db = new GRDbContext();
        }

        public UploadService(GRDbContext db)
        {
            this.db = db;
        }

        public string UploadImg(GoodsReceiveImageViewModel data)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            try
            {
                byte[] img = Convert.FromBase64String(data.base64);
                var path = Directory.GetCurrentDirectory();
                path += "\\" + "ImageFolder" + "\\";
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                System.IO.File.WriteAllBytes(path + data.name, img);

                im_GoodsReceive_Image result = new im_GoodsReceive_Image();

                result.GoodsReceiveImage_Index = Guid.NewGuid();
                result.GoodsReceive_Index = data.goodsReceive_Index;
                result.GoodsReceiveImage_path = "http://kascoit.ddns.me:99/ImageFolder/" + data.name.ToString();
                result.GoodsReceiveImage_type = data.type;
                result.Create_By = data.create_By;
                result.Create_Date = DateTime.Now;
                db.im_GoodsReceive_Image.Add(result);


                var transactionx = db.Database.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    db.SaveChanges();
                    transactionx.Commit();
                }

                catch (Exception exy)
                {
                    msglog = State + " ex Rollback " + exy.Message.ToString();
                    olog.logging("SaveTruckLoadImages", msglog);
                    transactionx.Rollback();

                    throw exy;

                }

                return "Upload Success";

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<GoodsReceiveImageViewModel> findImg(GoodsReceiveImageViewModel data)
        {
            try
            {
                var result = new List<GoodsReceiveImageViewModel>();

                var query = db.im_GoodsReceive_Image.Where(c => c.GoodsReceive_Index == data.goodsReceive_Index).ToList();

                foreach (var item in query)
                {
                    var resultItem = new GoodsReceiveImageViewModel();

                    resultItem.src = item.GoodsReceiveImage_path;
                    resultItem.type = "image";
                    result.Add(resultItem);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
