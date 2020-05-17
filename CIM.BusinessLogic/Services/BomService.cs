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
            int skipRec = (page - 1) * howmany;
            int takeRec = howmany;

            var dbModel = await _bomRepository.Where(x => x.IsActive && x.IsDelete == false &
                string.IsNullOrEmpty(keyword) ? true : (x.Name.Contains(keyword)))
                .Select(
                    x => new BomModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        IsActive = x.IsActive,
                        IsDelete = x.IsDelete,
                        CreatedAt = x.CreatedAt,
                        CreatedBy = x.CreatedBy,
                        UpdatedAt = x.UpdatedAt,
                        UpdatedBy = x.UpdatedBy
                    }).ToListAsync();

            int totalCount = dbModel.Count();
            dbModel = dbModel.OrderBy(s => s.Id).Skip(skipRec).Take(takeRec).ToList();
            return ToPagingModel(dbModel, totalCount, page, howmany);

        }

        public async Task<List<BomMaterialModel>> ListBomMapping(int bomId)
        {
            var output = await _bomRepository.ListMaterialByBom(bomId);
            return output;
        }
    }
}
