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
    public class LossLevel1Service : BaseService, ILossLevel1Service
    {
        private readonly ILossLevel1Repository _lossLevel1Repository;
        private IUnitOfWorkCIM _unitOfWork;

        public LossLevel1Service(
            IUnitOfWorkCIM unitOfWork,
            ILossLevel1Repository lossLevel1Repository
            )
        {
            _lossLevel1Repository = lossLevel1Repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PagingModel<Model.LossLevel1Model>> List(string keyword, int page, int howmany, bool isActive)
        {
            var output = await _lossLevel1Repository.List(page, howmany, keyword, isActive);
            return output;
        }
    }
}
