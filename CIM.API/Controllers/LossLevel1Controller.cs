using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CIM.BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Data;
using CIM.Model;

namespace CIM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LossLevel1Controller : ControllerBase
    {
        private ILossLevel1Service _service;

        public LossLevel1Controller(ILossLevel1Service service)
        {
            _service = service;
        }


        /*

        // GET: api/LossLevel1/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // PUT: api/LossLevel1/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }


        // GET: api/LossLevel1
        [Route("api/[controller]/Get/{row}/{pages}")]
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/LossLevel1
        [Route("All")]
        [HttpGet]
        public string All()
        {
            var result = _service.All();
            return JsonSerializer.Serialize(result);
        }



        // GET: api/LossLevel1
        //[Route("api/[controller]/All")]
        [Route("All")]
        [HttpGet]
        public string All()
        {
            return "test";
        }


          // POST: api/LossLevel1

          [HttpPost]
          public void Post([FromBody] string value)
          {
          }

          // POST api/<controller>
          [Route("Insert")]
          [HttpPost]
          public bool Insert(List<LossLevel1Model> data)
          {
              //try
              //{
                  foreach (var row in data)
                  {

                      LossLevel1Model list = new LossLevel1Model();

                      list.Id = Convert.ToInt32(row.Id);
                      list.Description = row.Description.ToString();
                      list.IsActive = Convert.ToBoolean(row.IsActive);      //public bool IsActive { get; set; }
                      list.IsDelete = Convert.ToBoolean(row.IsDelete);      //public bool IsDelete { get; set; }
                      list.CreatedAt = Convert.ToDateTime(row.CreatedAt);   //public DateTime CreatedAt { get; set; }
                      list.CreatedBy = row.CreatedBy.ToString();            //public string CreatedBy { get; set; }
                      list.UpdatedAt = Convert.ToDateTime(row.UpdatedAt);   //public DateTime? UpdatedAt { get; set; }
                      list.UpdatedBy = row.UpdatedBy.ToString();            //public string UpdatedBy { get; set; }

                      _service.LossLevel1Insert(list);
                  }

                  return true;
              //}
              //catch (Exception ex)
              //{
              //    throw ex;
              //}
          }
          */

        [Route("Insert")]
        [HttpPost]
        public bool Insert(List<LossLevel1Model> model)
        {
            //try
            //{
            foreach (var row in model)
            {

                LossLevel1Model list = new LossLevel1Model();

                list.Id = Convert.ToInt32(row.Id);
                list.Description = row.Description.ToString();
                list.IsActive = Convert.ToBoolean(row.IsActive);      //public bool IsActive { get; set; }
                list.IsDelete = Convert.ToBoolean(row.IsDelete);      //public bool IsDelete { get; set; }
                list.CreatedAt = Convert.ToDateTime(row.CreatedAt);   //public DateTime CreatedAt { get; set; }
                list.CreatedBy = row.CreatedBy.ToString();            //public string CreatedBy { get; set; }
                list.UpdatedAt = Convert.ToDateTime(row.UpdatedAt);   //public DateTime? UpdatedAt { get; set; }
                list.UpdatedBy = row.UpdatedBy.ToString();            //public string UpdatedBy { get; set; }

                _service.Insert(list);
            }

            return true;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        // GET: api/LossLevel1
        [Route("ListAll")]
        [HttpGet]
        public string ListAll()
        {
            var result = _service.ListAllLossLevel1();
            return JsonSerializer.Serialize(result);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _service.DeleteByIdLossLevel1(id);
        }

        //[Route("api/[controller]/Edit")]
        //[HttpPost]
        //public async Task<object> Edit([FromBody]List<ProductModel> data)
        //{
        //    try
        //    {
        //        await _productService.BulkEdit(data);
        //        return new object();
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}
        [Route("Edit")]
        [HttpPost]
        public bool Edit(List<LossLevel1Model> data)
        {
            //try
            //{
            foreach (var row in data)
            {

                LossLevel1Model list = new LossLevel1Model();

                list.Id = Convert.ToInt32(row.Id);
                list.Description = row.Description.ToString();
                list.IsActive = Convert.ToBoolean(row.IsActive);      //public bool IsActive { get; set; }
                list.IsDelete = Convert.ToBoolean(row.IsDelete);      //public bool IsDelete { get; set; }
                list.CreatedAt = Convert.ToDateTime(row.CreatedAt);   //public DateTime CreatedAt { get; set; }
                list.CreatedBy = row.CreatedBy.ToString();            //public string CreatedBy { get; set; }
                list.UpdatedAt = Convert.ToDateTime(row.UpdatedAt);   //public DateTime? UpdatedAt { get; set; }
                list.UpdatedBy = row.UpdatedBy.ToString();            //public string UpdatedBy { get; set; }

                _service.EditLossLevel1(list);
            }

            return true;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }


        //// GET: api/LossLevel1/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        [Route("GetById")]
        [HttpGet]
        public string GetById(int Id)
        {
            var result = _service.GetByIdLossLevel1(Id);
            return JsonSerializer.Serialize(result);
        }

    }
}
