using CIM.Domain.Interfaces;
using CIM.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.DAL.Implements
{
    public class SiteRepository : Repository<Sites>, ISiteRepository
    {
        public SiteRepository(cim_dbContext context) : base(context)
        {

        }
    }
}
