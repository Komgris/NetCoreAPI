using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIM.DAL.Implements
{
    public class AccidentRepository : Repository<Accidents, AccidentModel>, IAccidentRepository
    {
        public AccidentRepository(cim_dbContext context, IConfiguration configuration) : base(context, configuration)
        {
        }

        public async Task<Accidents> Get(int id)
        {
            var dbModel = await _dbset.Where(x => x.Id == id)
                .Select(x => new
                {
                    Accident = x,
                    Paticipients = x.AccidentParticipants,
                }).FirstOrDefaultAsync();
            dbModel.Accident.AccidentParticipants = dbModel.Paticipients;
            return dbModel.Accident;

        }

    }
}
