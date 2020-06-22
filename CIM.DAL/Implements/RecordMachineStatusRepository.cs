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
    public class RecordMachineStatusRepository : Repository<RecordMachineStatus, object>, IRecordMachineStatusRepository
    {
        public RecordMachineStatusRepository(cim_dbContext context, IConfiguration configuration) : base(context, configuration)
        {
        }
    }
} 
