using CIM.Domain.Models;
using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.DAL.Interfaces
{
    public interface IRecordManufacturingLossRepository : IRepository<RecordManufacturingLoss>
    {
        Task<RecordManufacturingLoss> GetByGuid(Guid guid);
    }
}
