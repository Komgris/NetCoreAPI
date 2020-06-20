using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.DAL.Implements
{
    public class MachineTypeComponentTypeRepository : Repository<MachineTypeComponentType, object>, IMachineTypeComponentTypeRepository
    {
        public MachineTypeComponentTypeRepository(cim_dbContext context, IConfiguration configuration) : base(context, configuration)
        {

        }

    }
}
