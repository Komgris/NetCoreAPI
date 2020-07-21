using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CIM.BusinessLogic.Interfaces;
using CIM.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CIM.API.Controllers
{
    [ApiController]
    public class TeamController : BaseController
    {
        private IResponseCacheService _responseCacheService;
        private ITeamService _service;

        public TeamController(
            IResponseCacheService responseCacheService,
            ITeamService service,
            IMasterDataService masterDataService
        )
        {
            _responseCacheService = responseCacheService;
            _service = service;
            _masterDataService = masterDataService;
        }

        [Route("api/[controller]/List")]
        [HttpGet]
        public async Task<ProcessReponseModel<PagingModel<TeamModel>>> List(string keyword = "", int page = 1, int howMany = 10,bool isActive = true)
        {
            var output = new ProcessReponseModel<PagingModel<TeamModel>>();
            try
            {
                output.Data = await _service.List(keyword, page, howMany, isActive);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        [HttpGet]
        [Route("api/[controller]/Get")]
        public async Task<ProcessReponseModel<TeamModel>> Get(int id)
        {
            var output = new ProcessReponseModel<TeamModel>();
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

        [Route("api/[controller]/Update")]
        [HttpPut]
        public async Task<ProcessReponseModel<TeamModel>> Update([FromBody] TeamModel data)
        {
            var output = new ProcessReponseModel<TeamModel>();
            try
            {
                await _service.Update(data);
                await _masterDataService.Refresh(Constans.MasterDataType.Team);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        [Route("api/[controller]/Create")]
        [HttpPost]
        public async Task<ProcessReponseModel<TeamModel>> Create([FromBody] TeamModel data)
        {
            var output = new ProcessReponseModel<TeamModel>();
            try
            {
                await _service.Create(data);
                await _masterDataService.Refresh(Constans.MasterDataType.Team);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        [HttpGet]
        [Route("api/[controller]/GetEmployeesByTeam")]
        public async Task<ProcessReponseModel<List<TeamEmployeesModel>>> GetEmployeesByTeam(int teamId)
        {
            var output = new ProcessReponseModel<List<TeamEmployeesModel>>();
            try
            {

                output.Data = await _service.GetEmployeesByTeam(teamId);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        [Route("api/[controller]/InsertEmployeesMappingByTeam")]
        [HttpPost]
        public async Task<ProcessReponseModel<object>> InsertEmployeesMappingByTeam([FromBody] List<TeamEmployeesModel> data)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                await _service.InsertEmployeesMappingByTeam(data);               
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