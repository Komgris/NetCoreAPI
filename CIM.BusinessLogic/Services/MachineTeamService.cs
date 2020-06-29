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
    public class MachineTeamService : BaseService, IMachineTeamService
    {
        private IResponseCacheService _responseCacheService;
        private IUnitOfWorkCIM _unitOfWork;
        private IMachineTeamRepository _machineTeamRepository;

        public MachineTeamService(
            IResponseCacheService responseCacheService,
            IUnitOfWorkCIM unitOfWork,
            IMachineTeamRepository machineTeamRepository
            )
        {
            _responseCacheService = responseCacheService;
            _unitOfWork = unitOfWork;
            _machineTeamRepository = machineTeamRepository;
        }

        public async Task Create(MachineTeamModel data)
        {
            var db_model = MapperHelper.AsModel(data, new MachineTeam());
            db_model.CreatedAt = DateTime.Now;
            db_model.CreatedBy = CurrentUser.UserId;
            db_model.IsActive = true;
            db_model.IsDelete = false;
            _machineTeamRepository.Add(db_model);
            await _unitOfWork.CommitAsync();
        }

        public async Task<MachineTeamModel> Get(int id)
        {
            return await _machineTeamRepository.Where(x => x.Id == id).Select(
                x => new MachineTeamModel
                {
                    Id = x.Id,
                    MachineId = x.MachineId,
                    MachineName = x.Machine.Name,
                    MachineType = x.Machine.MachineType.Name,
                    TeamId = x.TeamId,
                    TeamName = x.Team.Name,
                    IsActive = x.IsActive,
                    IsDelete = x.IsDelete,
                    CreatedAt = x.CreatedAt,
                    CreatedBy = x.CreatedBy,
                    UpdatedAt = x.UpdatedAt,
                    UpdatedBy = x.UpdatedBy
                }).FirstOrDefaultAsync();
        }

        public async Task<PagingModel<MachineTeamModel>> List(string keyword, int page, int howMany, bool isActive)
        {
            var output = await _machineTeamRepository.ListAsPaging("sp_ListMachineTeam", new Dictionary<string, object>()
                {
                    {"@keyword", keyword},
                    {"@howmany", howMany},
                    {"@page", page},
                    {"@is_active", isActive}
                }, page, howMany);
            return output;
        }

        public async Task Update(MachineTeamModel data)
        {
            var db_model = MapperHelper.AsModel(data, new MachineTeam());
            db_model.UpdatedAt = DateTime.Now;
            db_model.UpdatedBy = CurrentUser.UserId;
            _machineTeamRepository.Edit(db_model);
            await _unitOfWork.CommitAsync();
        }

    }
}
