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
    public class LossLevel2Service : BaseService, ILossLevel2Service
    {
        private readonly ILossLevel2Repository _lossLevel2Repository;
        private IUnitOfWorkCIM _unitOfWork;

        public LossLevel2Service(
            IUnitOfWorkCIM unitOfWork,
            ILossLevel2Repository lossLevel2Repository
            )
        {
            _lossLevel2Repository = lossLevel2Repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PagingModel<Model.LossLevel2ListModel>> List(string keyword, int page, int howmany, bool isActive)
        {
            var output = await _lossLevel2Repository.List(page, howmany, keyword, isActive);
            return output;
        }
    }
}
