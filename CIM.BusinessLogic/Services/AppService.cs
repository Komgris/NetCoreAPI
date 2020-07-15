using CIM.BusinessLogic.Interfaces;
using CIM.BusinessLogic.Utility;
using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Services
{
    public class AppService : BaseService, IAppService
    {
        private IResponseCacheService _responseCacheService;
        private IAppRepository _appRepository;
        private IAppFeatureRepository _appFeatureRepository;
        private IUserGroupAppRepository _userGroupAppRepository;
        private IUserGroupAppFeatureRepository _userGroupAppFeatureRepository;
        private IUnitOfWorkCIM _unitOfWork;

        public AppService(
            IResponseCacheService responseCacheService,
            IAppRepository appRepository,
            IAppFeatureRepository appFeatureRepository,
            IUserGroupAppRepository userGroupAppRepository,
            IUserGroupAppFeatureRepository userGroupAppFeatureRepository,
            IUnitOfWorkCIM unitOfWork
        )
        {
            _responseCacheService = responseCacheService;
            _appRepository = appRepository;
            _appFeatureRepository = appFeatureRepository;
            _userGroupAppRepository = userGroupAppRepository;
            _userGroupAppFeatureRepository = userGroupAppFeatureRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<AppModel>> Get(int userGroupId)
        {
            var output = new List<AppModel>();
            var appDb = await _userGroupAppRepository.WhereAsync(x => x.UserGroupId == userGroupId);
            foreach(var item in appDb)
            {
                var appModel = new AppModel();
                appModel.Id = item.AppId;
                appModel.Name = _appRepository.Where(x => x.Id == item.AppId).FirstOrDefault().Name;

                var appFeatureDb = await _appFeatureRepository.List("sp_ListAppFeatureByUserGroup", new Dictionary<string, object>()
                {
                    {"@usergroup", userGroupId},
                    {"@appid", item.AppId}
                });

                if (appFeatureDb.Count > 0)
                    if (appFeatureDb[0].Name != null)
                        appModel.FeatureList = appFeatureDb;

                output.Add(appModel);
            }
            return output;
        }

    }
}
