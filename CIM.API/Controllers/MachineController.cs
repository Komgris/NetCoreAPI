﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CIM.BusinessLogic.Interfaces;
using CIM.Model;
using Microsoft.AspNetCore.Mvc;

namespace CIM.API.Controllers
{
    //[MiddlewareFilter(typeof(CustomAuthenticationMiddlewarePipeline))]
    [ApiController]
    public class MachineController : ControllerBase
    {
        private IMachineService _service;
        public MachineController(IMachineService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("api/[controller]/Create")]
        public async Task<ProcessReponseModel<MachineModel>> Create([FromBody]MachineModel model)
        {
            var output = new ProcessReponseModel<MachineModel>();
            try
            {
                // todo
                //var currentUser = (CurrentUserModel)HttpContext.Items[Constans.CURRENT_USER];
                _service.CurrentUser = new CurrentUserModel { UserId = "64c679a2-795c-4ea9-a35a-a18822fa5b8e" };

                output.Data = await _service.Create(model);
            }
            catch (Exception ex)
            {
                output.Message = ex.ToString();
            }
            return output;
        }

        [HttpPost]
        [Route("api/[controller]/Update")]
        public async Task<ProcessReponseModel<MachineModel>> Update([FromBody]MachineModel model)
        {
            var output = new ProcessReponseModel<MachineModel>();
            try
            {
                // todo
                //var currentUser = (CurrentUserModel)HttpContext.Items[Constans.CURRENT_USER];
                _service.CurrentUser = new CurrentUserModel { UserId = "64c679a2-795c-4ea9-a35a-a18822fa5b8e" };

                output.Data = await _service.Update(model);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.ToString();
            }
            return output;
        }

        [HttpGet]
        [Route("api/[controller]/{page}/{howmany}/{keyword}")]
        public async Task<ProcessReponseModel<PagingModel<MachineModel>>> List(string keyword = "", int page = 1, int howmany = 10)
        {
            var output = new ProcessReponseModel<PagingModel<MachineModel>>();
            try
            {
                // todo
                //var currentUser = (CurrentUserModel)HttpContext.Items[Constans.CURRENT_USER];
                output.Data = await _service.List(keyword, page, howmany);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.ToString();
            }
            return output;
        }

        [HttpGet]
        [Route("api/[controller]/{id}")]
        public async Task<ProcessReponseModel<MachineModel>> Get(int id)
        {
            var output = new ProcessReponseModel<MachineModel>();
            try
            {
                // todo
                //var currentUser = (CurrentUserModel)HttpContext.Items[Constans.CURRENT_USER];
                output.Data = await _service.Get(id);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.ToString();
            }
            return output;
        }

    }
}