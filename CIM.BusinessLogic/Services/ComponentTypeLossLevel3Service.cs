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
    public class ComponentTypeLossLevel3Service : BaseService, IComponentTypeLossLevel3Service
    {
        private readonly IComponentTypeLossLevel3Repository _componentTypeLossLevel3Repository;
        private IUnitOfWorkCIM _unitOfWork;

        public ComponentTypeLossLevel3Service(
            IUnitOfWorkCIM unitOfWork,
            IComponentTypeLossLevel3Repository componentTypeLossLevel3Repository
            )
        {
            _componentTypeLossLevel3Repository = componentTypeLossLevel3Repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PagingModel<ComponentTypeLossLevel3ListModel>> List(int? componentTypeId, int? lossLevel3Id, int page, int howmany)
        {
            var output = await _componentTypeLossLevel3Repository.List( componentTypeId, lossLevel3Id, page,  howmany);
            return output;
        }

        public async Task Update(List<int> lossLevel3Ids, int componentTypeId)
        {
            var list = _componentTypeLossLevel3Repository.Where(x => x.ComponentTypeId == componentTypeId);
            foreach (var model in list)
            {
                _componentTypeLossLevel3Repository.Delete(model);
            }

            foreach (var lossLevel3Id in lossLevel3Ids)
            {
                var db_model = new ComponentTypeLossLevel3();
                db_model.LossLevel3Id = lossLevel3Id;
                db_model.ComponentTypeId = componentTypeId;
                _componentTypeLossLevel3Repository.Add(db_model);
            }
            await _unitOfWork.CommitAsync();
        }
    }
}
