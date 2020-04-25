using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.DAL.Implements
{
    public class WasteLevel1Repository : Repository<WasteLevel1>, IWasteLevel1Repository
    {
        public WasteLevel1Repository(cim_dbContext context, IConfiguration configuration) : base(context, configuration)
        {

        }
    }
}
