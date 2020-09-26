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
    public class TeamRepository : Repository<Team, TeamModel>, ITeamRepository
    {
        public TeamRepository(cim_dbContext context, IConfiguration configuration) : base(context, configuration)
        {

        }

    }
}
