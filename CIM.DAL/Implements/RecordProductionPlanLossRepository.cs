using CIM.DAL.Interfaces;
using CIM.Domain.Models;

namespace CIM.DAL.Implements
{
    public class RecordProductionPlanLossRepository : Repository<RecordProductionPlanLoss>, IRecordProductionPlanLossRepository
    {
        public RecordProductionPlanLossRepository(cim_dbContext context) : base(context)
        {

        }
    }
}