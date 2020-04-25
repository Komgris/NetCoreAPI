using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.BusinessLogic.Interfaces
{
    public interface IRecordProductionPlanWasteService
    {
        Dictionary<int, RecordProductionPlanWasteModel> ToDictiony(IEnumerable<RecordProductionPlanWasteModel> enumerable);
    }
}
