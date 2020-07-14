using CIM.BusinessLogic.Interfaces;
using CIM.BusinessLogic.Utility;
using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Services
{
    public class UserGroupService : BaseService, IUserGroupService
    {
        private IUserGroupRepository _userGroupRepository;
        private IUserGroupAppRepository _userGroupAppRepository;
        private IUserGroupAppFeatureRepository _userGroupAppFeatureRepository;
        private IUnitOfWorkCIM _unitOfWork;

        public UserGroupService(
            IUserGroupRepository userGroupRepository,
            IUserGroupAppRepository userGroupAppRepository,
            IUserGroupAppFeatureRepository userGroupAppFeatureRepository,
            IUnitOfWorkCIM unitOfWork
            )
        {
            _userGroupRepository = userGroupRepository;
            _userGroupAppRepository = userGroupAppRepository;
            _userGroupAppFeatureRepository = userGroupAppFeatureRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task Create(UserGroupModel model)
        {
            var dbModel = new UserGroups
            {
                Name = model.Name,
                IsActive = true,
                IsDelete = false
            };
            _userGroupRepository.Add(dbModel);
            await _unitOfWork.CommitAsync();

            await InsertAppAndFeatureMapping(model, dbModel.Id);
        }

        public async Task<PagingModel<UserGroupModel>> List(string keyword, int page, int howMany, bool isActive)
        {
            var output = await _userGroupRepository.ListAsPaging("sp_ListUserGroup", new Dictionary<string, object>()
                {
                    {"@keyword", keyword},
                    {"@howmany", howMany},
                    {"@page", page},
                    {"@is_active", isActive}
                }, page, howMany);
            return output;
        }

        public async Task<UserGroupModel> Get(int id)
        {
            var db_model = await _userGroupRepository.Where(x => x.Id == id)
                .Select(x => new UserGroupModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    IsActive = x.IsActive,
                    IsDelete = x.IsDelete
                }).FirstOrDefaultAsync();

            var appList = new List<AppModel>();
            var appDb_model = await _userGroupAppRepository.WhereAsync(x => x.UserGroupId == id);
            foreach (var item in appDb_model)
            {
                var feature_model = await _userGroupAppFeatureRepository.Where(x => x.AppUserGroupId == item.Id)
                    .Select(x => new AppFeatureModel
                    {
                        FeatureId = x.FeatureId,
                        Name = x.Feature.Name,
                        AppId = item.AppId
                    }).ToListAsync();

                var app_model = new AppModel
                {
                    Id = item.AppId,
                    FeatureList = feature_model
                };

                appList.Add(app_model);
            }

            db_model.AppList = appList;

            return db_model;
        }

        public async Task Update(UserGroupModel model)
        {
            var db_model = MapperHelper.AsModel(model, new UserGroups());
            _userGroupRepository.Edit(db_model);
            await _unitOfWork.CommitAsync();

            await DeleteMapping(model.Id);
            await InsertAppAndFeatureMapping(model, model.Id);
        }

        public async Task InsertAppAndFeatureMapping(UserGroupModel model, int userGroupId)
        {
            foreach (var item in model.AppList)
            {
                var appDBModel = new UserGroupsApps
                {
                    AppId = item.Id,
                    UserGroupId = userGroupId
                };
                _userGroupAppRepository.Add(appDBModel);
                await _unitOfWork.CommitAsync();

                var appUserGroupId = appDBModel.Id;

                foreach (var feature in item.FeatureList)
                {
                    var appFeatureDBModel = new UserGroupsAppFeatures
                    {
                        FeatureId = feature.FeatureId,
                        AppUserGroupId = appUserGroupId
                    };
                    _userGroupAppFeatureRepository.Add(appFeatureDBModel);
                    await _unitOfWork.CommitAsync();
                }
            }            
        }

        public async Task DeleteMapping(int userGroupId)
        {
            var appList = await _userGroupAppRepository.WhereAsync(x => x.UserGroupId == userGroupId);
            foreach (var item in appList)
            {
                var featureList = await _userGroupAppFeatureRepository.WhereAsync(x => x.AppUserGroupId == item.Id);
                foreach (var featureItem in featureList)
                {
                    _userGroupAppFeatureRepository.Delete(featureItem);
                }
                _userGroupAppRepository.Delete(item);
            }


        }
    }
}
