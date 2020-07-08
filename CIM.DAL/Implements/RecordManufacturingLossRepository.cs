using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using CIM.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CIM.DAL.Implements
{
    public class RecordManufacturingLossRepository : Repository<RecordManufacturingLoss, RecordManufacturingLossModel>, IRecordManufacturingLossRepository
    {
        public RecordManufacturingLossRepository(cim_dbContext context, IConfiguration configuration ) : base(context, configuration)
        {

        }

        public async Task<int[]> GetNotExistingStoppedMachineRecord(Dictionary<int, ActiveMachineModel> activeMachines)
        {
            var stoppedMachineIds = activeMachines.Where(x => x.Value.StatusId == Constans.MACHINE_STATUS.Stop).Select(x => x.Key).ToArray();
            var stoppedDbIds = await _dbset.Where(x => x.MachineId.HasValue && stoppedMachineIds.Contains(x.MachineId.Value) && x.EndAt == null)
                .Select(x=>x.MachineId.Value)
                .ToListAsync();
            return stoppedMachineIds.Except(stoppedDbIds).ToArray();
        }

        public async Task<RecordManufacturingLoss> GetByGuid(Guid guid)
        {
            var models = await _entities.RecordManufacturingLoss.Where(x => x.Guid == guid.ToString()).ToListAsync();
            return models.First(x => x.Guid == guid.ToString());
        }

        public async Task<int[]> ListMachineReady(Dictionary<string, object> dictionaries)
        {
            var output = await ExecStoreProcedure<MachineReadyModel>("[dbo].[sp_ListMachineReady]", dictionaries); 
            return output.Select(x => x.MachineId).ToArray();
        }
        public async Task<int[]> ListMachineLossRecording(Dictionary<string, object> dictionaries)
        {
            var output = await ExecStoreProcedure<MachineReadyModel>("[dbo].[sp_ListMachineLossRecording]", dictionaries);
            return output.Select(x => x.MachineId).ToArray();
        }
    }
}