﻿using CIM.BusinessLogic.Interfaces;
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
            }).FirstOrDefaultAsync(x => x.Id == id && x.IsActive.Value && x.IsDelete == false);

        }

        public async Task<PagingModel<RouteListModel>> List(string keyword, int page, int howmany)
        {
            int skipRec = (page - 1) * howmany;
            int takeRec = howmany;

            var dbModel = await _routeRepository.Where(x => x.IsActive.Value && x.IsDelete == false &
                string.IsNullOrEmpty(keyword) ? true : (x.Name.Contains(keyword)))
                .Select(
                    x => new RouteListModel
                    {
                        Id = x.Id,
                        ParentId = x.ParentId,
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
