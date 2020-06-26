using CIM.Domain.Models;
using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.DAL.Interfaces
{
    public interface IMachineOperatorRepository : IRepository<MachineOperators, MachineOperatorModel>
    {
        Task<T> ExecuteProcedure<T>(string procedureName, Dictionary<string, object> parameters);
    }
}
