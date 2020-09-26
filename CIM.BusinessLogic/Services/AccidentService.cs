using CIM.BusinessLogic.Interfaces;
using CIM.BusinessLogic.Utility;
using CIM.DAL.Interfaces;
using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CIM.Domain;
using CIM.Domain.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CIM.BusinessLogic.Services
{
    public class AccidentService : BaseService, IAccidentService
    {
        private IAccidentRepository _accidentRepository;
        private IAccidentParticipantRepository _accidentParticipantRepository;
        private IUnitOfWorkCIM _unitOfWorkCIM;

        public AccidentService(
            IAccidentRepository accidentRepository,
            IAccidentParticipantRepository accidentParticipantRepository,
            IUnitOfWorkCIM unitOfWorkCIM
            )
        {
            _accidentRepository = accidentRepository;
            _accidentParticipantRepository = accidentParticipantRepository;
            _unitOfWorkCIM = unitOfWorkCIM;
        }

        public async Task Create(AccidentModel model)
        {
            var dbModel = MapperHelper.AsModel(model, new Accidents());
            foreach (var item in model.Participants)
            {
                dbModel.AccidentParticipants.Add(MapperHelper.AsModel(item, new AccidentParticipants()));
            }
            dbModel.CreatedAt = DateTime.Now;
            dbModel.CreatedBy = CurrentUser.UserId;
            _accidentRepository.Add(dbModel);
            await _unitOfWorkCIM.CommitAsync();
        }

        public async Task Delete(int id)
        {
            var model = await _accidentRepository.Get(id);           
            foreach (var item in model.AccidentParticipants)
            {
                _accidentParticipantRepository.Delete(item);
            }
            _accidentRepository.Delete(model);
            await _unitOfWorkCIM.CommitAsync();
        }

        public async Task<AccidentModel> Get(int id)
        {
            var dbModel = await _accidentRepository.Get(id);
            var output = MapperHelper.AsModel(dbModel, new AccidentModel());
            foreach(var item in dbModel.AccidentParticipants)
            {
                output.Participants.Add(MapperHelper.AsModel(item, new AccidentParticipantsModel()));
            }
            return output;
        }

        public async Task Update(AccidentModel model)
        {
            var dbModel = await _accidentRepository.Get(model.Id);
            dbModel.Title = model.Title;
            dbModel.CategoryId = model.CategoryId;
            dbModel.MachineId = model.MachineId;
            dbModel.HappenAt = model.HappenAt;
            dbModel.Note = model.Note;
            dbModel.UpdatedAt = DateTime.Now;
            dbModel.UpdatedBy = CurrentUser.UserId;
            foreach (var item in dbModel.AccidentParticipants)
                _accidentParticipantRepository. Delete(item);

            foreach (var item in model.Participants)
                dbModel.AccidentParticipants.Add(MapperHelper.AsModel(item, new AccidentParticipants()));

            _accidentRepository.Edit(dbModel);
            await _unitOfWorkCIM.CommitAsync();
        }

        public async Task<PagingModel<AccidentModel>> List(string keyword, int page, int howMany)
        {
            var output = await _accidentRepository.ListAsPaging("sp_ListAccident", new Dictionary<string, object>()
                {
                    {"@keyword", keyword},
                    {"@howmany", howMany},
                    { "@page", page}
                }, page, howMany);
            return output;
        }

        public async Task End(int id)
        {
            var dbModel = await _accidentRepository.Get(id);
            var now = DateTime.Now;

            dbModel.EndAt = now;
            dbModel.UpdatedAt = now;
            dbModel.UpdatedBy = CurrentUser.UserId;

            _accidentRepository.Edit(dbModel);
            await _unitOfWorkCIM.CommitAsync();
        }
    }
}
