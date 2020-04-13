using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CIM.BusinessLogic.Interfaces {
    public interface IReportService : IBaseService {
        DataTable GetProductionSummary(int planid,int routeid,DateTime? from,DateTime? to);
        string GetProductionPlanInfomation(int planid, int routeid);
        string GetMachineSpeed(int planid, int routeid, DateTime? from, DateTime? to);
        string GetProductionEvents(int planid, int routeid, DateTime? from, DateTime? to);
        string GetProductionOperators(int planid, int routeid);
    }
}
