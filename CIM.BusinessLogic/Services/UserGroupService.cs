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

            foreach (var app in model.AppList)
            {
                var userGroupAppDbModel = new UserGroupsApps
                {
                    AppId = app.Id,
                    UserGroupId = dbModel.Id                    
                };

                foreach (var feature in app.FeatureList)
                {
                    userGroupAppDbModel.UserGroupsAppFeatures.Add(new UserGroupsAppFeatures { 
                        FeatureId = feature.FeatureId,
                        AppUserGroupId = userGroupAppDbModel.Id
                    });
                }

                dbModel.UserGroupsApps.Add(userGroupAppDbModel);
            }

            _userGroupRepository.Add(dbModel);
            await _unitOfWork.CommitAsync();
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
            var dbModel = await _userGroupRepository.Where(x => x.Id == id)
                .Select(x => new UserGroupModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    IsActive = x.IsActive,
                    IsDelete = x.IsDelete
                }).FirstOrDefaultAsync();

            var appList = new List<AppModel>();
            var appDbModel = await _userGroupAppRepository.WhereAsync(x => x.UserGroupId == id);
            foreach (var item in appDbModel)
            {
                var featureDbModel = await _userGroupAppFeatureRepository.Where(x => x.AppUserGroupId == item.Id)
                    .Select(x => new AppFeatureModel
                    {
                        FeatureId = x.FeatureId,
                        Name = x.Feature.Name,
                        AppId = item.AppId
                    }).ToListAsync();

                var app_model = new AppModel
                {
                    Id = item.AppId,
                    FeatureList = featureDbModel
                };

                appList.Add(app_model);
            }

            dbModel.AppList = appList;

            return dbModel;
        }

        public async Task Update(UserGroupModel model)
        {
            await DeleteMapping(model.Id);

            var dbModel = MapperHelper.AsModel(model, new UserGroups());

            foreach (var app in model.AppList)
            {
                var userGroupAppDbModel = new UserGroupsApps
                {
                    AppId = app.Id,
                    UserGroupId = dbModel.Id
                };

                foreach (var feature in app.FeatureList)
                {
                    userGroupAppDbModel.UserGroupsAppFeatures.Add(new UserGroupsAppFeatures
                    {
                        FeatureId = feature.FeatureId,
                        AppUserGroupId = userGroupAppDbModel.Id
                    });
                }

                dbModel.UserGroupsApps.Add(userGroupAppDbModel);
            }

            _userGroupRepository.Edit(dbModel);
            await _unitOfWork.CommitAsync();
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
