using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GRBusiness.ConfigModel
{
    public class View_SugesstionPutaway 
    {
        public Guid RuleputawaySuggest_Index { get; set; }
        public int RuleputawaySuggest_Seq { get; set; }
        public int? Suggest_IsActive { get; set; }
        public int? Suggest_IsDelete { get; set; }

        public Guid Ruleputaway_Index { get; set; }
        public string Ruleputaway_Id { get; set; }
        public string Ruleputaway_Name { get; set; }
        public int Ruleputaway_Seq { get; set; }
        public int? Ruleputaway_IsActive { get; set; }
        public int? Ruleputaway_IsDelete { get; set; }

        public Guid RuleputawayCondition_Index { get; set; }
        public string RuleputawayCondition_Id { get; set; }
        public string RuleputawayCondition_Name { get; set; }
        public string RuleputawayConditionOperator { get; set; }
        public string RuleputawayCondition_Param { get; set; }
        public Guid Zoneputaway_Index { get; set; }
        public int? Condition_IsActive { get; set; }
        public int? Condition_IsDelete { get; set; }

        public Guid RuleputawayConditionField_Index { get; set; }
        public string RuleputawayConditionField_Id { get; set; }
        public string RuleputawayConditionField_Name { get; set; }
        public string RuleputawayConditionField_Description { get; set; }
        public int? ConditionField_IsActive { get; set; }
        public int? ConditionField_IsDelete { get; set; }

    }
}
