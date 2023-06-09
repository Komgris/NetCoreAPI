﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CIM.API.HubConfig;
using CIM.BusinessLogic.Interfaces;
using CIM.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace CIM.API.Controllers
{
    [ApiController]
    public class UserGroupController : BaseController
    {
        private IUserGroupService _service;

        public UserGroupController(
            IHubContext<GlobalHub> hub,
            IUserGroupService service,
            IMasterDataService masterDataService
            )
        {
            _hub = hub;
            _service = service;
            _masterDataService = masterDataService;
        }

        [HttpPost]
        [Route("api/[controller]/Create")]
        public async Task<ProcessReponseModel<UserGroupModel>> Create(UserGroupModel model)
        {
            var output = new ProcessReponseModel<UserGroupModel>();
            try
            {
                await _service.Create(model);
                await RefreshMasterData(Constans.MasterDataType.UserGroup);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }


        [HttpGet]
        [Route("api/[controller]/List")]
        public async Task<ProcessReponseModel<PagingModel<UserGroupModel>>> List(string keyword = "", int page = 1, int howMany = 10, bool isActive = true)
        {
            var output = new ProcessReponseModel<PagingModel<UserGroupModel>>();
            try
            {
                output.Data = await _service.List(keyword, page, howMany, isActive);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.ToString();
            }
            return output;
        }

        [HttpGet]
        [Route("api/[controller]/Get")]
        public async Task<ProcessReponseModel<UserGroupModel>> Get(int id)
        {
            var output = new ProcessReponseModel<UserGroupModel>();
            try
            {
                output.Data = await _service.Get(id);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        [HttpPut]
        [Route("api/[controller]/Update")]
        public async Task<ProcessReponseModel<UserGroupModel>> Update([FromBody] UserGroupModel model)
        {
            var output = new ProcessReponseModel<UserGroupModel>();
            try
            {
                await _service.Update(model);
                await RefreshMasterData(Constans.MasterDataType.UserGroup);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

    }
}