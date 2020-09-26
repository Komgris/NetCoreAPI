using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace CIM.DAL.Implements
{
    public class UnitsRepository : Repository<Units, object>, IUnitsRepository
    {
        public UnitsRepository(cim_3m_1Context context, IConfiguration configuration ) : base(context, configuration)
        {
        }
    }
}