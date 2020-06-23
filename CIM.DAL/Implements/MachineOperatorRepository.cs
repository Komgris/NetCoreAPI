using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.DAL.Implements
{
    public class MachineOperatorRepository : Repository<MachineOperators, MachineOperatorModel>, IMachineOperatorRepository
    {
        public MachineOperatorRepository(cim_dbContext context, IConfiguration configuration) : base(context, configuration)
        {
        }
    }
}
