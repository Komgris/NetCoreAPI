using System;
using System.Collections.Generic;
using System.Text;

using CIM.BusinessLogic.Interfaces;
using CIM.BusinessLogic.Utility;
using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using System.Linq;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Services
{
    public class LossLevel3Service : BaseService, ILossLevel3Service
    {
        private readonly ILossLevel3Repository _repository;
        private IUnitOfWorkCIM _unitOfWork;

        public LossLevel3Service(
            IUnitOfWorkCIM unitOfWork,
            ILossLevel3Repository repository
            )
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public Task<LossLevel3Model> Create(LossLevel3Model model)
        {
            throw new NotImplementedException();
        }

        public Task<LossLevel3ViewModel> Get(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<PagingModel<LossLevel3ViewModel>> List(string keyword, int page, int howmany)
        {
            int skipRec = (page - 1) * howmany;
            int takeRec = howmany;

            //to do optimize
            var dbModel = _repository.Where(
                x => x.IsActive == true && x.IsDelete == false
                )
                .Select(
                x => new
                {
                    x.Id,
                    x.Name,
                    x.Description,
                    LossLevel1Id = x.LossLevel2.LossLevel1.Id,
                    LossLevel1Name = x.LossLevel2.LossLevel1.Name,
                    x.LossLevel2Id,
                    LossLevel2Name = x.LossLevel2.Name
                }
                )
                .Where(
                x =>
                                x.Name.Contains(keyword)
                                || x.Description.Contains(keyword)
                                || x.LossLevel1Name.Contains(keyword)
                                || x.LossLevel2Name.Contains(keyword)

                )
                .ToList();

            //var dbModel = (await _repository.AllAsync())
            //.Select(
            //x => new
            //{
            //    x.Id,
            //    x.Name,
            //    x.Description,
            //    LossLevel1Id = x.LossLevel2.LossLevel1.Id,
            //    LossLevel1Name = x.LossLevel2.LossLevel1.Name,
            //    x.LossLevel2Id,
            //    LossLevel2Name = x.LossLevel2.Name
            //}
            //)
            //.Where(
            //x =>
            //                x.Name.Contains(keyword)
            //                || x.Description.Contains(keyword)
            //                || x.LossLevel1Name.Contains(keyword)
            //                || x.LossLevel2Name.Contains(keyword)

            //)
            //.ToList();


            int total = dbModel.Count();
            dbModel = dbModel.OrderBy(s => s.Id).Skip(skipRec).Take(takeRec).ToList();

            var output = new List<LossLevel3ViewModel>();
            foreach (var item in dbModel)
                output.Add(MapperHelper.AsModel(item, new LossLevel3ViewModel()));

            return new PagingModel<LossLevel3ViewModel>
            {
                HowMany = total,
                Data = output
            };

            //throw new NotImplementedException();
        }

        public Task<LossLevel3Model> Update(LossLevel3Model model)
        {
            throw new NotImplementedException();
        }
    }
}
