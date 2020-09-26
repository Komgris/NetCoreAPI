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
    public class UserGroupAppFeatureService : BaseService, IUserGroupAppFeatureService
    {
        private IUserGroupAppFeatureRepository _userGroupAppFeatureRepository;
        private IUnitOfWorkCIM _unitOfWork;

        public UserGroupAppFeatureService(
            IUserGroupAppFeatureRepository userGroupAppFeatureRepository,
            IUnitOfWorkCIM unitOfWork
            )
        {
            _userGroupAppFeatureRepository = userGroupAppFeatureRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<UserGroupAppFeatureModel>> Get(int id)
        {
            var db_model = await _userGroupAppFeatureRepository.Where(x => x.AppUserGroupId == id)
                .Select(x => new UserGroupAppFeatureModel
                {
                    FeatureId = x.FeatureId,
                    FeatureName = x.Feature.Name,
                    AppUserGroupId = x.AppUserGroupId
                }).ToListAsync();
            return db_model;
        }

    }
}
