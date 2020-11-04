using CIM.BusinessLogic.Interfaces;
using CIM.DAL.Interfaces;
using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Services
{
    public class ProductionPlanCheckListServices : BaseService, IProductionPlanCheckListService
    {
        private IProductionPlanCheckListRepository _productionPlanCheckListRepository;
        private IUnitOfWorkCIM _unitOfWork;

        public ProductionPlanCheckListServices(
            IUnitOfWorkCIM unitOfWork,
            IProductionPlanCheckListRepository productionPlanCheckListRepository
            )
        {
            _productionPlanCheckListRepository = productionPlanCheckListRepository;
            _unitOfWork = unitOfWork;
        }


        public async Task<List<ProductionPlanCheckListModel>> List(int machineId, int CheckListTypeId)
        {
            var output = await _productionPlanCheckListRepository.List("sp_ListCheckList", new Dictionary<string, object>()
                {
                    {"@machine_id", machineId},
                    {"@CheckListType_id", CheckListTypeId}
                });
            return output;
        }
    }
}
