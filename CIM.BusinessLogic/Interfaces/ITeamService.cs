using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Interfaces
{
    public interface ITeamService : IBaseService
    {
        Task Create(TeamModel data);
        Task<PagingModel<TeamModel>> List(string keyword, int page, int howMany, bool isActive);
        Task<TeamModel> Get(int id);
        Task Update(TeamModel data);
        Task InsertEmployeesMappingByTeam(List<TeamEmployeesModel> data);
        Task<List<TeamEmployeesModel>> GetEmployeesByTeam(int teamId);
    }
}
