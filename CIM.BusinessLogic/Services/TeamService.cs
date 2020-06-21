using CIM.BusinessLogic.Interfaces;
using CIM.BusinessLogic.Utility;
using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Services
{
    public class TeamService : BaseService, ITeamService
    {
        private IResponseCacheService _responseCacheService;
        private IUnitOfWorkCIM _unitOfWork;
        private ITeamRepository _teamRepository;
        private ITeamEmployeesRepository _teamEmployeesRepository;

        public TeamService(
            IResponseCacheService responseCacheService,
            IUnitOfWorkCIM unitOfWork,
            ITeamRepository teamRepository,
            ITeamEmployeesRepository teamEmployeesRepository
            )
        {
            _responseCacheService = responseCacheService;
            _unitOfWork = unitOfWork;
            _teamRepository = teamRepository;
            _teamEmployeesRepository = teamEmployeesRepository;
        }

        public async Task Create(TeamModel data)
        {
            var db_model = MapperHelper.AsModel(data, new Team());
            db_model.CreatedAt = DateTime.Now;
            db_model.CreatedBy = CurrentUser.UserId;
            db_model.IsActive = true;
            db_model.IsDelete = false;
            _teamRepository.Add(db_model);
            await _unitOfWork.CommitAsync();
        }

        public async Task<TeamModel> Get(int id)
        {
            return await _teamRepository.All().Select(
                x => new TeamModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    TeamTypeId = x.TeamTypeId,
                    IsActive = x.IsActive,
                    IsDelete = x.IsDelete,
                    CreatedAt = x.CreatedAt,
                    CreatedBy = x.CreatedBy,
                    UpdatedAt = x.UpdatedAt,
                    UpdatedBy = x.UpdatedBy
                }).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<PagingModel<TeamModel>> List(string keyword, int page, int howmany, bool isActive)
        {
            var output = await _teamRepository.ListAsPaging("sp_ListTeam", new Dictionary<string, object>()
                {
                    {"@keyword", keyword},
                    {"@howmany", howmany},
                    {"@page", page},
                    {"@is_active", isActive}
                }, page, howmany);
            return output;
        }

        public async Task Update(TeamModel data)
        {
            var db_model = MapperHelper.AsModel(data, new Team());
            db_model.UpdatedAt = DateTime.Now;
            db_model.UpdatedBy = CurrentUser.UserId;
            _teamRepository.Edit(db_model);
            await _unitOfWork.CommitAsync();
        }

        public async Task InsertEmployeesMappingByTeam(List<TeamEmployeesModel> data)
        {
            await DeleteMapping(data[0].TeamId);
            foreach (var model in data)
            {
                if (model.EmployeesId != 0)
                {
                    var db_model = MapperHelper.AsModel(model, new TeamEmployees());
                    db_model.IsActive = true;
                    db_model.IsDelete = false;
                    db_model.CreatedAt = DateTime.Now;
                    db_model.CreatedBy = CurrentUser.UserId;
                    _teamEmployeesRepository.Add(db_model);
                }
            }
            await _unitOfWork.CommitAsync();
        }

        public async Task<List<TeamEmployeesModel>> GetEmployeesByTeam(int teamId)
        {
            var output = await _teamEmployeesRepository.List("sp_ListEmployeeByTeam", new Dictionary<string, object>()
                {
                    {"@team_id", teamId}
                });
            output.ForEach(x => x.ImagePath = ImagePath);
            return output;
        }
        public async Task DeleteMapping(int teamId)
        {
            var list = await _teamEmployeesRepository.WhereAsync(x => x.TeamId == teamId);
            foreach (var model in list)
            {
                _teamEmployeesRepository.Delete(model);
            }
        }

    }
}
