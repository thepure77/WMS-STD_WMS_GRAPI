using GRBusiness.PlanGoodsReceiveItem;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace GRBusiness.Test.PlanGoodsReceiveItem
{
    public class PlanGoodsReceiveItemServiceTest
    {
        [Fact]
        public void GetByPlanGoodReceiveId()
        {
            #region Arrange
            var planGoodReceiveId = "3CCD17C5-A2EF-46CF-A188-CD963209D32A";
            PlanGoodsReceiveItemService service = new PlanGoodsReceiveItemService();
            #endregion

            #region Act
            var result = service.GetByPlanGoodReceiveId(planGoodReceiveId);
            #endregion

            #region Assert
            Assert.NotNull(result);
            #endregion

        }
    }
}
