using CIM.DAL.Implements;
using CIM.Domain.Models;
using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.DAL.Interfaces
{
    public interface IRecordProductionPlanInformationRepository : IRepository<RecordProductionPlanInformation, RecordProductionPlanInformationModel>
    {
        Task<RecordProductionPlanInformation> Get(int id);
    }
}
