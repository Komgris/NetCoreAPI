using CIM.BusinessLogic.Interfaces;
using CIM.DAL.Interfaces;
using CIM.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Services
{
    public class BomService : BaseService, IBomService
    {
        private IResponseCacheService _responseCacheService;
        private IBomRepository _bomRepository;
        private IUnitOfWorkCIM _unitOfWork;

        public BomService(
        IResponseCacheService responseCacheService,
        IUnitOfWorkCIM unitOfWork,
        IBomRepository bomRepository
    )
        {
            _responseCacheService = responseCacheService;
            _bomRepository = bomRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PagingModel<BomModel>> List(string keyword, int page, int howmany)
        {
            var output = await _bomRepository.ListBom(page, howmany, keyword);
            return output;
        }

        public async Task<List<BomMaterialModel>> ListBomMapping(int bomId)
        {
            var output = await _bomRepository.ListMaterialByBom(bomId);
            return output;
        }


    }
}
