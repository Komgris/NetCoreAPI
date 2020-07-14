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
    public class EmployeesService : BaseService, IEmployeesService
    {
        private readonly IEmployeesRepository _employeesRepository;
        private IUnitOfWorkCIM _unitOfWork;
        private INameRepository _nameRepository;

        public EmployeesService(
            IUnitOfWorkCIM unitOfWork,
            IEmployeesRepository employeesRepository,
            IProductMaterialRepository productMaterialRepository,
            INameRepository nameRepository
            )
        {
            _employeesRepository = employeesRepository;
            _unitOfWork = unitOfWork;
            _nameRepository = nameRepository;
        }

        public async Task<EmployeesModel> Create(EmployeesModel model)
        {
            var dbModel = MapperHelper.AsModel(model, new Employees());
            dbModel.IsActive = true;
            dbModel.IsDelete = false;
            dbModel.CreatedBy = CurrentUser.UserId;
            dbModel.CreatedAt = DateTime.Now;
            _employeesRepository.Add(dbModel);
            await _unitOfWork.CommitAsync();

            await CreateName(model);

            return MapperHelper.AsModel(dbModel, new EmployeesModel());
        }

        public async Task CreateName(EmployeesModel model)
        {
            var data = await _employeesRepository.FirstOrDefaultAsync(x => x.EmNo == model.EmNo);

            var dbName = new Name();
            dbName.EmployeesId = data.Id;
            dbName.TitleName = model.TitleNameTH;
            dbName.FirstName = model.FirstNameTH;
            dbName.LastName = model.LastNameTH;
            dbName.LanguageId = "TH";
            dbName.IsActive = true;
            dbName.IsDelete = false;
            dbName.CreatedBy = CurrentUser.UserId;
            dbName.CreatedAt = DateTime.Now;
            _nameRepository.Add(dbName);

            var dbNameEN = new Name();
            dbNameEN.EmployeesId = data.Id;
            dbNameEN.TitleName = model.TitleNameEN;
            dbNameEN.FirstName = model.FirstNameEN;
            dbNameEN.LastName = model.LastNameEN;
            dbNameEN.LanguageId = "EN";
            dbNameEN.IsActive = true;
            dbNameEN.IsDelete = false;
            dbNameEN.CreatedBy = CurrentUser.UserId;
            dbNameEN.CreatedAt = DateTime.Now;
            _nameRepository.Add(dbNameEN);

            await _unitOfWork.CommitAsync();
        }

        public async Task<EmployeesModel> Update(EmployeesModel model)
        {
            var dbModel = await _employeesRepository.FirstOrDefaultAsync(x => x.Id == model.Id);
            dbModel = MapperHelper.AsModel(model, dbModel);
            dbModel.UpdatedBy = CurrentUser.UserId;
            dbModel.UpdatedAt = DateTime.Now;
            _employeesRepository.Edit(dbModel);

            var dbName = await _nameRepository.FirstOrDefaultAsync(x => x.EmployeesId == model.Id && x.LanguageId == "TH");
            dbName.TitleName = model.TitleNameTH;
            dbName.FirstName = model.FirstNameTH;
            dbName.LastName = model.LastNameTH;
            dbName.UpdatedBy = CurrentUser.UserId;
            dbName.UpdatedAt = DateTime.Now;
            _nameRepository.Edit(dbName);

            var dbNameEN = await _nameRepository.FirstOrDefaultAsync(x => x.EmployeesId == model.Id && x.LanguageId == "EN");
            dbNameEN.TitleName = model.TitleNameEN;
            dbNameEN.FirstName = model.FirstNameEN;
            dbNameEN.LastName = model.LastNameEN;
            dbNameEN.UpdatedBy = CurrentUser.UserId;
            dbNameEN.UpdatedAt = DateTime.Now;
            _nameRepository.Edit(dbNameEN);

            await _unitOfWork.CommitAsync();
            return MapperHelper.AsModel(dbModel, new EmployeesModel());
        }

        public async Task<PagingModel<EmployeesModel>> List(string keyword, int page, int howMany, bool isActive)
        {
            var output = await _employeesRepository.ListAsPaging("sp_ListEmployees", new Dictionary<string, object>()
                {
                    {"@keyword", keyword},
                    {"@howmany", howMany},
                    {"@page", page},
                    {"@is_active", isActive}
                }, page, howMany);
            return output;
        }

        public async Task<EmployeesModel> Get(int id)
        {
            var output = await _employeesRepository.List("sp_ListEmployeesById", new Dictionary<string, object>()
                {
                    {"@id", id}
                });
            return output.FirstOrDefault();
        }

        public async Task<EmployeesModel> GetFromEmployeeNo(string no)
        {
            return await _employeesRepository.Where(x => x.EmNo == no)
                .Select(x => new EmployeesModel
                {
                    Id = x.Id,
                    EmNo = x.EmNo,
                    IsActive = x.IsActive,
                    IsDelete = x.IsDelete
                }).FirstOrDefaultAsync();
        }

    }
}
