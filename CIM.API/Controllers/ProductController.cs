using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CIM.BusinessLogic.Interfaces;
using CIM.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using System.Text.Json;
using System.IO;
using System.Net.Http.Headers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CIM.API.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IProductService _productService;
        public ProductController(
            IProductService productService
            )
        {
            _productService = productService;
        }

        // POST api/<controller>
        [Route("api/[controller]/Create")]
        [HttpPost]
        public async Task<ProcessReponseModel<object>> Create([FromBody]List<ProductModel> data)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                // todo
                //var currentUser = (CurrentUserModel)HttpContext.Items[Constans.CURRENT_USER];
                _productService.CurrentUser = new CurrentUserModel { UserId = "64c679a2-795c-4ea9-a35a-a18822fa5b8e" };

                await _productService.Create(data);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.IsSuccess = false;
                output.Message = ex.ToString();
            }
            return output;
        }

        [Route("api/[controller]/Delete/{id}")]
        [HttpGet]
        public async Task<ProcessReponseModel<object>> Delete(int id)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                // todo
                //var currentUser = (CurrentUserModel)HttpContext.Items[Constans.CURRENT_USER];
                _productService.CurrentUser = new CurrentUserModel { UserId = "64c679a2-795c-4ea9-a35a-a18822fa5b8e" };

                await _productService.Delete(id);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.IsSuccess = false;
                output.Message = ex.ToString();
            }
            return output;
        }

        [HttpGet]
        [Route("api/[controller]/{id}")]
        public async Task<ProcessReponseModel<ProductModel>> Get(int id)
        {
            var output = new ProcessReponseModel<ProductModel>();
            try
            {
                // todo
                //var currentUser = (CurrentUserModel)HttpContext.Items[Constans.CURRENT_USER];
                output.Data = await _productService.Get(id);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.IsSuccess = false;
                output.Message = ex.ToString();
            }
            return output;
        }

        [HttpGet]
        [Route("api/[controller]/List/{page}/{howmany}")]
        public async Task<ProcessReponseModel<PagingModel<ProductModel>>> List(string keyword = "", int page = 1, int howmany = 10)
        {
            var output = new ProcessReponseModel<PagingModel<ProductModel>>();
            try
            {
                // todo
                //var currentUser = (CurrentUserModel)HttpContext.Items[Constans.CURRENT_USER];
                output.Data = await _productService.List(keyword, page, howmany);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.IsSuccess = false;
                output.Message = ex.ToString();
            }
            return output;
        }

        [HttpPost]
        [Route("api/[controller]/Update")]
        public async Task<ProcessReponseModel<object>> Update([FromBody]ProductModel model)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                // todo
                //var currentUser = (CurrentUserModel)HttpContext.Items[Constans.CURRENT_USER];
                _productService.CurrentUser = new CurrentUserModel { UserId = "64c679a2-795c-4ea9-a35a-a18822fa5b8e" };

                await _productService.Update(model);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.IsSuccess = false;
                output.Message = ex.ToString();
            }
            return output;
        }
    }
}
