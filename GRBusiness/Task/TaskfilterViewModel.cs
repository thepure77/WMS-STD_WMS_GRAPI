using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GRBusiness.GoodsReceive
{


    public class TaskfilterViewModel 
    {
        public TaskfilterViewModel()
        {
            listTaskViewModel = new List<listTaskViewModel>();

        }

        public Guid? taskGR_Index { get; set; }

        public Guid? taskGRItem_Index { get; set; }

        public string taskGR_No { get; set; }

        public string goodsReceive_No { get; set; }

        public Guid? goodsReceive_Index { get; set; }
        public string userAssign { get; set; }
        public string create_By { get; set; }
        public string tag_No { get; set; }

        public class actionResultTask
        {
            public string msg { get; set; }
        }

        public List<listTaskViewModel> listTaskViewModel { get; set; }

    }

}
