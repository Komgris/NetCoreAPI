using CIM.Domain.Models;
using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.DAL.Interfaces
{
    public interface ICalendarRepository :IRepository<Calendar>
    {
        Task<PagingModel<CalendarModel>> List(int page, int howmany, string keyword, bool isActive);
    }
}
