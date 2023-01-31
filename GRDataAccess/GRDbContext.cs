using GRDataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DataAccess
{
    public class GRDbContext : DbContext
    {
        public DbSet<IM_PlanGoodsReceive> IM_PlanGoodsReceive { get; set; }
        public DbSet<IM_PlanGoodsReceiveItem> IM_PlanGoodsReceiveItem { get; set; }
        public DbSet<IM_GoodsReceive> IM_GoodsReceive { get; set; }
        public DbSet<IM_GoodsReceiveItem> IM_GoodsReceiveItem { get; set; }
        public DbSet<WM_Tag> WM_Tag { get; set; }
        public DbSet<View_ProductDetail> View_ProductDetail { get; set; }
        public virtual DbSet<IM_GoodsReceiveItemLocation> IM_GoodsReceiveItemLocation { get; set; }
        //public virtual DbSet<WM_BinBalance> wm_BinBalance { get; set; }
        //public virtual DbSet<WM_BinCard> wm_BinCard { get; set; }
        public virtual DbSet<WM_TagItem> wm_TagItem { get; set; }
        public virtual DbSet<GetValueByColumn> GetValueByColumn { get; set; }
        public virtual DbSet<View_GoodsReceivePending> View_GoodsReceivePending { get; set; }
        public virtual DbSet<View_CheckCloseDocument> View_CheckCloseDocuments { get; set; }
        public DbSet<View_PlanGoodsReceiveItem> View_PlanGoodsReceiveItem { get; set; }
        public DbSet<View_PlanGrProcessStatus> View_PlanGrProcessStatus { get; set; }
        public DbSet<View_GoodsReceiveItemAlloCate> View_GoodsReceiveItemAlloCate { get; set; }

        public DbSet<View_CheckDocumentStatus> View_CheckDocumentStatus { get; set; }

        public DbSet<View_GrProcessStatus> View_GrProcessStatus { get; set; }

        public DbSet<View_GetPlanGoodsReceiveItem> View_GetPlanGoodsReceiveItem { get; set; }

        public DbSet<View_GetTagItem> View_GetTagItem { get; set; }
        public DbSet<TagItemPutawaySKU> TagItemPutawaySKU { get; set; }

        public DbSet<View_GetScanProductDetail> View_GetScanProductDetail { get; set; }
        public DbSet<Get_PlanGoodsIssueItemPopup> Get_PlanGoodsIssueItemPopup { get; set; }

        public DbSet<View_ReturnReceive> View_ReturnReceive { get; set; }
        public DbSet<im_GoodsIssueItemLocation> IM_GoodsIssueItemLocation { get; set; }

        public DbSet<View_CheckPlanGR> View_CheckPlanGR { get; set; }

        public DbSet<View_GoodsReceiveWithTag> View_GoodsReceiveWithTag { get; set; }
        public virtual DbSet<View_GoodsReceive> View_GoodsReceive { get; set; }
        public virtual DbSet<im_TaskGR> im_TaskGR { get; set; }
        public virtual DbSet<im_TaskGRItem> im_TaskGRItem { get; set; }
        public virtual DbSet<View_GRTag> View_GRTag { get; set; }
        public virtual DbSet<View_PreviewTaskGR> View_PreviewTaskGR { get; set; }
        public virtual DbSet<View_GoodsReceivePop> View_GoodsReceivePop { get; set; }
        public virtual DbSet<View_TaskGR> View_TaskGR { get; set; }
       
        public virtual DbSet<View_TagitemSugesstion> View_TagitemSugesstion { get; set; }

        public virtual DbSet<im_GoodsReceive_Image> im_GoodsReceive_Image { get; set; }
        public virtual DbSet<im_Signatory_log> im_Signatory_log { get; set; }
        public virtual DbSet<View_RPT_PrintOutGR> View_RPT_PrintOutGR { get; set; }
        public virtual DbSet<View_RPT_PrintOutTag> View_RPT_PrintOutTag { get; set; }

        public virtual DbSet<View_filterGR> View_filterGR { get; set; }
        public virtual DbSet<View_TagPutaway> View_TagPutaway { get; set; }
        public virtual DbSet<View_TaskGRfilter> View_TaskGRfilter { get; set; }
        public virtual DbSet<Po_subcontact> Po_subcontact { get; set; }
        public virtual DbSet<View_RPT_PrintOutGR_V2> View_RPT_PrintOutGR_V2 { get; set; }
        public virtual DbSet<View_Location_Alert_MSG> View_Location_Alert_MSG { get; set; }
        public virtual DbSet<View_GoodsReceiveWithTag_V2> View_GoodsReceiveWithTag_V2 { get; set; }
        public virtual DbSet<View_CheckPlanGRScanGR> View_CheckPlanGRScanGR { get; set; }
        public virtual DbSet<View_CheckQtyPlan> View_CheckQtyPlan { get; set; }
        public virtual DbSet<View_CheckLocation> View_CheckLocation { get; set; }
        public virtual DbSet<im_PlanGoodsReceiveItem_Ref> im_PlanGoodsReceiveItem_Ref { get; set; }
        public virtual DbSet<View_Matdoc_Update_lineproduction> View_Matdoc_Update_lineproduction { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder();
                builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"), optional: false);

                var configuration = builder.Build();

                var connectionString = configuration.GetConnectionString("DefaultConnection").ToString();

                optionsBuilder.UseSqlServer(connectionString);
            }
            //optionsBuilder.UseSqlServer(@"Server=192.168.1.11\MSSQL2017;Database=WMSDB;Trusted_Connection=True;Integrated Security=False;user id=sa;password=K@sc0db12345;");
            //optionsBuilder.UseSqlServer(@"Server=10.0.177.33\SQLEXPRESS;Database=WMSDB;Trusted_Connection=True;Integrated Security=False;user id=cfrffmusr;password=ffmusr@cfr;");
            //optionsBuilder.UseSqlServer(@"Server=kascoit.ddns.net,22017;Database=WMSDB_QA;Trusted_Connection=True;Integrated Security=False;user id=sa;password=K@sc0db12345;");
        }
    }

    

   
}
