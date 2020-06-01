using CIM.BusinessLogic.Interfaces;
using CIM.DAL.Interfaces;
using CIM.Model;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Services
{
    public class CalendarService : BaseService, ICalendarService
    {
        private ICalendarService _calendarService;
        private IUnitOfWorkCIM _unitOfWork;

        public CalendarService(
            ICalendarService calendarService,
            IUnitOfWorkCIM unitOfWork
            )
        {
            _calendarService = calendarService;
            _unitOfWork = unitOfWork;
        }

        public async Task<PagingModel<CalendarModel>> List(int page, int howmany, string keyword, bool isActive)
        {
            var output = await _calendarService.List(page, howmany, keyword, isActive);
            return output;
        }

        //public List<CalendarModel> ReadImport(string path)
        //{
        //    //FileInfo excel = new FileInfo(path);
        //    //using (var package = new ExcelPackage(excel))
        //    //{
        //    //    var workbook = package.Workbook;
        //    //    var worksheet = workbook.Worksheets.First();
        //    //    List<CalendarModel> intList = ConvertImportToList(worksheet);
        //    //    return intList;
        //    //}
        //}
    }
}
