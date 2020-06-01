using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Interfaces
{
    public interface ICalendarService : IBaseService
    {
        public Task<PagingModel<CalendarModel>> List(int page, int howmany, string keyword, bool isActive);
    }
}
