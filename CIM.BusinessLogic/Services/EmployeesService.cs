using CIM.BusinessLogic.Interfaces;
using CIM.BusinessLogic.Utility;
using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
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

        public EmployeesService(
            IUnitOfWorkCIM unitOfWork,
            IEmployeesRepository employeesRepository,
            IProductMaterialRepository productMaterialRepository
            )
        {
            _employeesRepository = employeesRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<EmployeesModel> Create(EmployeesModel model)
        {
            var dbModel = MapperHelper.AsModel(model, new Employees());
            _employeesRepository.Add(dbModel);
            dbModel.IsActive = true;
            dbModel.IsDelete = false;
            dbModel.CreatedBy = CurrentUser.UserId;
            dbModel.CreatedAt = DateTime.Now;
            await _unitOfWork.CommitAsync();
            return MapperHelper.AsModel(dbModel, new EmployeesModel());
        }

        public async Task<EmployeesModel> Update(EmployeesModel model)
        {
            var dbModel = await _employeesRepository.FirstOrDefaultAsync(x => x.Id == model.Id);
            dbModel = MapperHelper.AsModel(model, dbModel);
            dbModel.UpdatedBy = CurrentUser.UserId;
            dbModel.UpdatedAt = DateTime.Now;
            _employeesRepository.Edit(dbModel);
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
            output.Data.ForEach(x => x.ImagePath = ImagePath);
            return output;
        }

        public async Task<EmployeesModel> Get(int id)
        {
            var dbModel = await _employeesRepository.FirstOrDefaultAsync(x => x.Id == id);
            return MapperHelper.AsModel(dbModel, new EmployeesModel());
        }

    }
}
