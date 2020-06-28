using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Interfaces
{
    public interface IRecordMaintenancePlanService : IBaseService
    {
        Task<List<RecordMaintenancePlanModel>> ListByMonth(int month, int year);
    }
}
