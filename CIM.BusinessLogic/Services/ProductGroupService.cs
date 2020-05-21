﻿using CIM.BusinessLogic.Interfaces;
using CIM.BusinessLogic.Utility;
using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Services
{
    public class ProductGroupService : BaseService, IProductGroupService
    {
        private IProductGroupRepository _productGroupRepository;
        private IRouteProductGroupRepository _routeProductGroupRepository;
        private IUnitOfWorkCIM _unitOfWork;
        public ProductGroupService(
            IProductGroupRepository productGroupRepository,
            IRouteProductGroupRepository routeProductGroupRepository,
            IUnitOfWorkCIM unitOfWork
            )
        {
            _productGroupRepository = productGroupRepository;
            _routeProductGroupRepository = routeProductGroupRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Create(ProductGroupModel data)
        {
            var db_model = MapperHelper.AsModel(data, new ProductGroup());
            db_model.CreatedAt = DateTime.Now;
            db_model.CreatedBy = CurrentUser.UserId;
            db_model.IsActive = true;
            db_model.IsDelete = false;
            _productGroupRepository.Add(db_model);
            await _unitOfWork.CommitAsync();
        }

        public async Task<ProductGroupModel> Get(int id)
        {
            var dbModels = await _productGroupRepository.Where(x => x.Id == id).FirstOrDefaultAsync();
            return MapperHelper.AsModel(dbModels, new ProductGroupModel());
        }

        public async Task InsertMappingRouteProductGroup(List<RouteProductGroupModel> data)
        {
            await DeleteMapping(data[0].ProductGroupId);
            foreach (var model in data)
            {
                var db = MapperHelper.AsModel(model, new RouteProductGroup());
                db.CreatedAt = DateTime.Now;
                db.CreatedBy = CurrentUser.UserId;
                _routeProductGroupRepository.Add(db);
            }
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteMapping(int id)
        {
            var list = _routeProductGroupRepository.Where(x => x.ProductGroupId == id);
            foreach (var model in list)
            {
                _routeProductGroupRepository.Delete(model);
            }
            await _unitOfWork.CommitAsync();
        }

        public async Task<PagingModel<ProductGroupModel>> List(string keyword, int page, int howmany)
        {
            var output = await _productGroupRepository.List(page, howmany, keyword);
            return output;
        }

        public async Task<List<RouteProductGroupModel>> ListRouteByProductGroup(int productGroupId)
        {
            var output = await _productGroupRepository.ListRouteByProductGroup(productGroupId);
            return output;
        }

        public async Task Update(ProductGroupModel data)
        {
            var db_model = MapperHelper.AsModel(data, new ProductGroup());
            db_model.UpdatedAt = DateTime.Now;
            db_model.UpdatedBy = CurrentUser.UserId;
            db_model.IsActive = true;
            db_model.IsDelete = false;
            _productGroupRepository.Edit(db_model);
            await _unitOfWork.CommitAsync();
        }
    }
}
