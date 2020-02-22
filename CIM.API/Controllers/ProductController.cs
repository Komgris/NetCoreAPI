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
        [Route("api/[controller]/Edit")]
        [HttpPost]
        public async Task<object> Edit([FromBody]List<ProductModel> data)
        {
            try
            {
                await _productService.BulkEdit(data);
                return new object();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        // POST api/<controller>
        [Route("api/[controller]/Insert")]
        [HttpPost]
        public async Task<List<ProductModel>> Insert([FromBody]List<ProductModel> data)
        {
            try
            {
                return await _productService.Create(data);                
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        [Route("api/[controller]/Delete/{id}")]
        [HttpGet]
        public async Task<object> Delete(int id)
        {
            await _productService.Delete(id);
            return new object();
        }


        [Route("api/[controller]/Get/{row}/{pages}")]
        [HttpGet]
        public string Get(int row, int pages)
        {
            var fromDb = _productService.Paging(pages, row);
            return JsonSerializer.Serialize(fromDb);
        }
        [Route("api/[controller]/GetNoPaging")]
        [HttpGet]
        public string GetNoPaging()
        {
            var fromDb = _productService.Get();
            return JsonSerializer.Serialize(fromDb);
        }
          
    }
}
