using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using Microsoft.Extensions.Configuration;

namespace CIM.DAL.Implements
{
    public class RecordProductionPlanLossRepository : Repository<RecordProductionPlanLoss>, IRecordProductionPlanLossRepository
    {
        public RecordProductionPlanLossRepository(cim_dbContext context, IConfiguration configuration ) : base(context, configuration)
        {

        }
    }
}