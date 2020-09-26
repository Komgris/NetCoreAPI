using CIM.DAL.Interfaces;
using CIM.DAL.Utility;
using CIM.Domain.Models;
using CIM.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.DAL.Implements
{
    public class TeamTypeRepository : Repository<TeamType, TeamTypeModel>, ITeamTypeRepository
    {
        public TeamTypeRepository(cim_3m_1Context context, IConfiguration configuration) : base(context, configuration)
        {

        }

    }
}
