using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GRBusiness.ConfigModel
{
    public class RuleputawayViewModel
    {
        public Guid Ruleputaway_Index { get; set; }

        public string Ruleputaway_Id { get; set; }

        public string Ruleputaway_Name { get; set; }

        public int? Ruleputaway_Seq { get; set; }

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

    }
}
