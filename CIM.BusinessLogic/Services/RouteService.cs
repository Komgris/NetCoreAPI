using CIM.BusinessLogic.Interfaces;
using CIM.BusinessLogic.Utility;
using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Services
{
    public class RouteService : BaseService, IRouteService
    {
        private IResponseCacheService _responseCacheService;
        private IUnitOfWorkCIM _unitOfWork;
        private IRouteRepository _routeRepository;

        public RouteService(
            IResponseCacheService responseCacheService,
            IUnitOfWorkCIM unitOfWork,
            IRouteRepository routeRepository
            )
        {
            _responseCacheService = responseCacheService;
            _unitOfWork = unitOfWork;
            _routeRepository = routeRepository;
        }

        public async Task Create(RouteListModel data)
        {
            var db_model = MapperHelper.AsModel(data, new Route());
            db_model.CreatedAt = DateTime.Now;
            db_model.CreatedBy = CurrentUser.UserId;
            db_model.IsActive = true;
            db_model.IsDelete = false;
            _routeRepository.Add(db_model);
            await _unitOfWork.CommitAsync();
        }

        public async Task<RouteListModel> Get(int id)
        {
            return await _routeRepository.All().Select(
            x => new RouteListModel
            {
                Id = x.Id,
                Name = x.Name,
                ParentId = x.ParentId,
                IsActive = x.IsActive,
                IsDelete = x.IsDelete,
                CreatedAt = x.CreatedAt,
                CreatedBy = x.CreatedBy,
                UpdatedAt = x.UpdatedAt,
                UpdatedBy = x.UpdatedBy
            }).FirstOrDefaultAsync(x => x.Id == id);

        }

        public async Task<PagingModel<RouteListModel>> List(string keyword, int page, int howmany,bool isActive)
        {
            var output = await _routeRepository.List(page, howmany, keyword, isActive);
            return output;
        }

        public async Task Update(RouteListModel data)
        {
            var db_model = MapperHelper.AsModel(data, new Route());
            db_model.UpdatedAt = DateTime.Now;
            db_model.UpdatedBy = CurrentUser.UserId;
            _routeRepository.Edit(db_model);
            await _unitOfWork.CommitAsync();
        }
    }
}
