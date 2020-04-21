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
    public class MaterialController : ControllerBase
    {
        private IMaterialService _service;
        public MaterialController(IMaterialService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("api/[controller]/Create")]
        public async Task<MaterialModel> Create([FromBody]MaterialModel model)
        {
            try
            {
                // todo
                //var currentUser = (CurrentUserModel)HttpContext.Items[Constans.CURRENT_USER];
                //_service.CurrentUser = currentUser;
                _service.CurrentUser = new CurrentUserModel { UserId = "64c679a2-795c-4ea9-a35a-a18822fa5b8e" };
                
               return await _service.Create(model);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPost]
        [Route("api/[controller]/Update")]
        public async Task<MaterialModel> Update([FromBody]MaterialModel model)
        {
            try
            {
                // todo
                //var currentUser = (CurrentUserModel)HttpContext.Items[Constans.CURRENT_USER];
                //_service.CurrentUser = currentUser;
                _service.CurrentUser = new CurrentUserModel { UserId = "64c679a2-795c-4ea9-a35a-a18822fa5b8e" };

                return await _service.Update(model);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet]
        [Route("api/[controller]/List")]
        public async Task<PagingModel<MaterialModel>> List(string keyword = "", int page = 1, int howmany = 10)
        {
            try
            {
                return await _service.List(keyword,page, howmany);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet]
        [Route("api/[controller]/Get")]
        public async Task<MaterialModel> Get(int id)
        {
            try
            {
                return await _service.Get(id);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}