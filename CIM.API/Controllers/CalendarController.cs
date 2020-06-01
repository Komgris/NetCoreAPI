using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CIM.BusinessLogic.Interfaces;
using CIM.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CIM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarController : BaseController
    {
        private IResponseCacheService _responseCacheService;
        private ICalendarService _calendarService;

        public CalendarController(
            IResponseCacheService responseCacheService,
            ICalendarService calendarService
            )
        {
            _responseCacheService = responseCacheService;
            _calendarService = calendarService;
        }

        [Route("api/[controller]/List")]
        [HttpGet]
        public async Task<ProcessReponseModel<PagingModel<CalendarModel>>> List(string keyword = "", int page = 1, int howmany = 10, bool isActive = true)
        {
            var output = new ProcessReponseModel<PagingModel<CalendarModel>>();
            try
            {
                output.Data = await _calendarService.List(page, howmany, keyword, isActive);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        [Route("api/[controller]/Compare")]
        [HttpPost]
        public async Task<ProcessReponseModel<List<CalendarModel>>> Compare()
        {
            var output = new ProcessReponseModel<List<CalendarModel>>();
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Calendar");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }


                    //var fromExcel = _productionPlanService.ReadImport(fullPath);
                    //var result = await _productionPlanService.Compare(fromExcel);
                    //output.Data = result;
                    output.IsSuccess = true;
                }
                else
                {
                    output.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }
    }
}