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
    public class UserGroupAppService : BaseService, IUserGroupAppService
    {
        private IUserGroupRepository _userGroupRepository;
        private IUserGroupAppRepository _userGroupAppRepository;
        private IUserGroupAppFeatureRepository _userGroupAppFeatureRepository;
        private IUnitOfWorkCIM _unitOfWork;

        public UserGroupAppService(
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

        public async Task<List<UserGroupAppModel>> Get(int id)
        {
            var db_model = await _userGroupAppRepository.Where(x => x.UserGroupId == id)
                .Select(x => new UserGroupAppModel
                {
                    Id = x.Id,
                    AppId = x.AppId,
                    AppName = x.App.Name,
                    UserGroupId = x.UserGroupId
                }).ToListAsync();
            return db_model;
        }
    }
}
