using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CIM.DAL.Implements
{
    public class RecordMachineComponentLossRepository : Repository<RecordMachineComponentLoss>, IMachineComponentLossRepository
    {
        public RecordMachineComponentLossRepository(cim_dbContext context) : base(context)
        {
        }
    }
}
