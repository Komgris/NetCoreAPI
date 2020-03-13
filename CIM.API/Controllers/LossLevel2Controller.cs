using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using CIM.BusinessLogic.Interfaces;
using System.Text.Json;
using System.Data;
using CIM.Model;

namespace CIM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LossLevel2Controller : ControllerBase
    {
        private ILossLevel2Service _service;

        public LossLevel2Controller(ILossLevel2Service service)
        {
            _service = service;
        }

        /*

        // GET: api/LossLevel2
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // POST: api/LossLevel2
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/LossLevel2/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        */

        [Route("Insert")]
        [HttpPost]
        public bool Insert(List<LossLevel2Model> data)
        {
            //try
            //{
            foreach (var row in data)
            {

                LossLevel2Model list = new LossLevel2Model();

                list.Id = Convert.ToInt32(row.Id);                      //public int Id { get; set; }
                list.Description = row.Description.ToString();          //public string Description { get; set; }
                list.IsActive = Convert.ToBoolean(row.IsActive);        //public bool IsActive { get; set; }
                list.IsDelete = Convert.ToBoolean(row.IsDelete);        //public bool IsDelete { get; set; }
                list.CreatedAt = Convert.ToDateTime(row.CreatedAt);     //public DateTime CreatedAt { get; set; }
                list.CreatedBy = row.CreatedBy.ToString();              //public string CreatedBy { get; set; }
                list.UpdatedAt = Convert.ToDateTime(row.UpdatedAt);     //public DateTime? UpdatedAt { get; set; }
                list.UpdatedBy = row.UpdatedBy.ToString();              //public string UpdatedBy { get; set; }
                list.LossLevel1Id = Convert.ToInt32(row.LossLevel1Id);  //public int? LossLevel1Id { get; set; }

                _service.InsertLossLevel2(list);
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
            var result = _service.ListAllLossLevel2();
            return JsonSerializer.Serialize(result);
        }

    }
}
