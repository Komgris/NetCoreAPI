using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.DAL.Implements
{
    public class WasteLevel2Repository : Repository<WasteLevel2>, IWasteLevel2Repository
    {
        public WasteLevel2Repository(cim_dbContext context, IConfiguration configuration) : base(context, configuration)
        {

        }
    }
}
