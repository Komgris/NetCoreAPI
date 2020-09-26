using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIM.DAL.Implements
{
    public class MachineOperatorRepository : Repository<MachineOperators, MachineOperatorModel>, IMachineOperatorRepository
    {
        public MachineOperatorRepository(cim_3m_1Context context, IConfiguration configuration) : base(context, configuration)
        {
        }

        public async Task<T> ExecuteProcedure<T>(string procedureName, Dictionary<string, object> parameters)
        {
            return (await ExecStoreProcedure<T>(procedureName, parameters)).FirstOrDefault();
        }


    }
}
