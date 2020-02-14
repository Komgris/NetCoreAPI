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
        [Route("api/[controller]/Insert")]
        [HttpPost]
        public async Task<object> insert([FromBody]List<ProductModel> data)
        {
            try
            {
                await Task.Run(() =>
                {
                    _productService.Create(data);
                });
                return new object();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        [Route("api/[controller]/Get/{row}/{pages}")]
        [HttpGet]
        public string Get(int row, int pages)
        {
            var fromDb = _productService.Paging(pages, row);
            return JsonSerializer.Serialize(fromDb);
        }

    }
}
