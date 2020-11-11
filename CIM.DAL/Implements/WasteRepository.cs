using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StoredProcedureEFCore;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CIM.DAL.Implements
{
    public class WasteRepository : Repository<Waste, WasteModel>, IWasteRepository
    {
        public WasteRepository(cim_3m_1Context context, IConfiguration configuration) : base(context, configuration)
        {
        }
    }
}
