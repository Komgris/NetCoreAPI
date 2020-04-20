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

        public Task<LossLevel3Model> Update(LossLevel3Model model)
        {
            throw new NotImplementedException();
        }

        public async Task<PagingModel<LossLevel3ViewModel>> List(string keyword, int page, int howmany)
        {
            bool isActive = true;
            var output = await _repository.ListAsPaging(page, howmany, keyword, isActive);
            return output;
        }
    }
}
