using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CIM.BusinessLogic.Interfaces {
    public interface IReportService : IBaseService {
        DataTable GetProductionSummary(string planid,int routeid,DateTime? from,DateTime? to);
        DataTable GetProductionPlanInfomation(string planid, int routeid);
        DataTable GetMachineSpeed(string planid, int routeid, DateTime? from, DateTime? to);
        DataTable GetProductionEvents(string planid, int routeid, DateTime? from, DateTime? to);
        DataTable GetProductionOperators(string planid, int routeid); 
        DataTable GetProductionWCMLoss(string planid, int routeid, int? losslv, int? mcid, DateTime? from, DateTime? to); 

    }
}
