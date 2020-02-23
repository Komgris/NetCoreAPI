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
    public class MaterialService : BaseService, IMaterialService
    {
        private readonly IMaterialRepository _materialRepository;
        private IUnitOfWorkCIM _unitOfWork;

        public MaterialService(
            IUnitOfWorkCIM unitOfWork,
            IMaterialRepository materialRepository
            )
        {
            _materialRepository = materialRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<MaterialModel> Create(MaterialModel model)
        {
            var dbModel = MapperHelper.AsModel(model, new Material());
            _materialRepository.Add(dbModel);
            //dbModel.UpdatedBy = CurrentUser.UserId;
            dbModel.UpdatedAt = DateTime.Now;
            await _unitOfWork.CommitAsync();
            return MapperHelper.AsModel(dbModel, new MaterialModel());
        }

        public async Task<MaterialModel> Update(MaterialModel model)
        {
            var dbModel = await _materialRepository.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive && x.IsDelete == false);
            dbModel = MapperHelper.AsModel(model, dbModel);
            //dbModel.UpdatedBy = CurrentUser.UserId;
            dbModel.UpdatedAt = DateTime.Now;
            _materialRepository.Edit(dbModel);
            await _unitOfWork.CommitAsync();
            return MapperHelper.AsModel(dbModel, new MaterialModel());
        }

        public async Task<PagingModel<MaterialModel>> List(int page, int howmany)
        {
            var products = await _materialRepository.WhereAsync(x => x.IsActive && x.IsDelete == false);
            int total = products.Count();

            int skipRec = (page - 1) * howmany;
            int takeRec = howmany;
            //to do optimize
            var dbModel = (await _materialRepository.WhereAsync(x => x.IsActive && x.IsDelete == false))
                            .OrderBy(s => s.Id).Skip(skipRec).Take(takeRec).ToList();

            var output = new List<MaterialModel>();
            foreach (var item in dbModel)
                output.Add(MapperHelper.AsModel(item, new MaterialModel()));

            return new PagingModel<MaterialModel>
            {
                HowMany = total,
                Data = output
            };
        }

        public async Task<MaterialModel> Get(int id)
        {
            var dbModel = await _materialRepository.FirstOrDefaultAsync(x => x.Id == id && x.IsActive && x.IsDelete == false);
            return MapperHelper.AsModel(dbModel, new MaterialModel());
        }

    }
}
