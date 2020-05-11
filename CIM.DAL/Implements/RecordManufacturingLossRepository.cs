using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using CIM.Model;
using Microsoft.EntityFrameworkCore;

namespace CIM.DAL.Implements
{
    public class RecordManufacturingLossRepository : Repository<RecordManufacturingLoss>, IRecordManufacturingLossRepository
    {
        public RecordManufacturingLossRepository(cim_dbContext context, IConfiguration configuration ) : base(context, configuration)
        {

        }

        public async Task<RecordManufacturingLoss> GetByGuid(Guid guid)
        {
            return await _dbset.FirstAsync(x => x.Guid == guid.ToString());
        }
    }
}