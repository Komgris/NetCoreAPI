using CIM.BusinessLogic.Interfaces;
using CIM.DAL.Interfaces;
using CIM.Model;
using Microsoft.Extensions.Configuration;
using MMM.Domain.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static CIM.Model.Constans;

namespace CIM.BusinessLogic.Services {
    public class ReportService : BaseService, IReportService {

        private IDirectSqlRepository _directSqlRepository;
        private IResponseCacheService _responseCacheService;

        private JsonSerializerSettings JsonSetting = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public ReportService(IDirectSqlRepository directSqlRepository, 
            IResponseCacheService responseCacheService)
        {
            _directSqlRepository = directSqlRepository;
            _responseCacheService = responseCacheService;
        }

        #region Cim-oper active operation info

        public PagingModel<object> GetWasteHistory(string planId, int routeId, DateTime? from, DateTime? to, int page)
        {

            var paramsList = new Dictionary<string, object>() {
                {"@planid", planId },
                {"@routeid", routeId },
                {"@from", from },
                {"@to", to },
                {"@page", page }
            };

            var dt = _directSqlRepository.ExecuteSPWithQuery("sp_report_waste_history", paramsList);
            var totalcnt = dt.Rows[0].Field<int>("totalcount");
            var pagingmodel = ToPagingModel<object>(null, totalcnt, page, 10);
            pagingmodel.DataObject = dt;

            return pagingmodel;
        }
        public PagingModel<object> GetProductionWCMLossHistory(string planId, int routeId, DateTime? from, DateTime? to, int page)
        {

            var paramsList = new Dictionary<string, object>() {
                {"@planid", planId },
                {"@routeid", routeId },
                {"@from", from },
                {"@to", to },
                {"@page", page }
            };

            var dt = _directSqlRepository.ExecuteSPWithQuery("sp_Report_WCMLosses", paramsList);
            var totalcnt = dt.Rows.Count > 0 ? dt.Rows[0].Field<int>("totalcount") : 0;
            var pagingmodel = ToPagingModel<object>(null, totalcnt, page, 10);
            pagingmodel.DataObject = dt;

            return pagingmodel;
        }

        public PagingModel<object> GetMachineStatusHistory(int howMany, int page, string planId, int routeId, int? machineId, DateTime? from = null, DateTime? to = null)
        {
            var paramsList = new Dictionary<string, object>() {
                {"@planid", planId },
                {"@routeid", routeId },
                {"@mcid", machineId },
                {"@from", from },
                {"@to", to },
                {"@page", page },
                {"@howmany", howMany }
            };

            var dt = _directSqlRepository.ExecuteSPWithQuery("sp_Report_Machine_Status", paramsList);
            var totalcnt = dt.Rows[0].Field<int>("totalcount");
            var pagingmodel = ToPagingModel<object>(null, totalcnt, page, howMany);
            pagingmodel.DataObject = dt;

            return pagingmodel;
        }

        public Dictionary<string, int> GetActiveProductionPlanOutput()
        {
            return _directSqlRepository.ExecuteSPWithQuery("sp_report_activeproductionplan_output", null).AsEnumerable().ToDictionary<DataRow, string, int>(row => row.Field<string>(0), r => r.Field<int>(1)); ;
        }
        public UnitDataModel GetActiveMachineInfo(string planId, int routeId, int machineId)
        {
            var paramsList = new Dictionary<string, object>() {
                {"@planid", planId },
                {"@routeid", routeId },
                {"@mcid", machineId }
            };

            var dset = _directSqlRepository.ExecuteSPWithQueryDSet("sp_Report_Active_Machine_Details", paramsList);
            var result = new UnitDataModel
            {
                Name = "MachineDetails",
                JsonData = JsonConvert.SerializeObject(dset.Tables[0]),
                JsonSpecificData = JsonConvert.SerializeObject(dset.Tables[1])
            };

            return result;
        }

        #endregion

        #region Cim-Mng Report
        private Dictionary<string, object> ReportCreiteria(ReportTimeCriteriaModel data)
        {
            var paramsList = new Dictionary<string, object>() {
                {"@report_type", data.ReportType },
                {"@date_from", data.DateFrom },
                {"@date_to", data.DateTo },
                {"@week_from", data.WeekFrom },
                {"@week_to", data.WeekTo },
                {"@month_from", data.MonthFrom },
                {"@month_to", data.MonthTo },
                {"@year_from", data.YearFrom },
                {"@year_to", data.YearTo },
                {"@ipd_from", data.IPDFrom },
                {"@ipd_to", data.IPDTo },
            };

            return paramsList;
        }

        private Dictionary<string, object> ReportDate(ReportDateModel data)
        {
            var paramsList = new Dictionary<string, object>() {
                {"@date_from", data.DateFrom },
                {"@date_to", data.DateTo },
            };

            return paramsList;
        }

        private Dictionary<string, object> PlanId(string data)
        {
            var paramsList = new Dictionary<string, object>() {
                {"@plan_id", data },
            };

            return paramsList;
        }
        public DataTable GetOEEReport(ReportDateModel data)
        {
            var paramsList = ReportDate(data);
            return _directSqlRepository.ExecuteSPWithQuery("sp_Record_OperationFinish", paramsList);
        }

        public DataTable GetOutputReport(ReportDateModel data)
        {
            var paramsList = ReportDate(data);
            return _directSqlRepository.ExecuteSPWithQuery("sp_AdminReport_Output", paramsList);
        }

        public List<ReportProductionPlanListModel> GetProductionPlanList(ReportDateModel data)
        {
            var paramsList = ReportDate(data);
            var res = _directSqlRepository.ExecuteSPWithQuery("sp_AdminReport_ProductionPlanList", paramsList);
            var json = JsonConvert.SerializeObject(res);
            List<ReportProductionPlanListModel> reports = new List<ReportProductionPlanListModel>();
            foreach (DataRow row in res.Rows)
            {
                ReportProductionPlanListModel report = new ReportProductionPlanListModel();
                report.PlanId = row["PlanId"].ToString();
                report.ProductCode = row["ProductCode"].ToString();
                report.MachineType_Id = Convert.ToInt32(row["MachineType_Id"]);
                report.MachineName = row["MachineName"].ToString();
                report.ShopNo = row["ShopNo"].ToString();
                report.Sequence = Convert.ToInt32(row["Sequence"]);
                report.ActualStart = Convert.ToDateTime(row["ActualStart"]);
                report.ActualFinish = Convert.ToDateTime(row["ActualFinish"]);
                reports.Add(report);
            }
            return reports;

        }

        private T ConvertDataTable<T>(DataTable res)
        {
            throw new NotImplementedException();
        }

        public ReportProductionPlanGuillotineModel GetGuillotineReport(string data)
        {
            var paramsList = PlanId(data);
            var report = _directSqlRepository.ExecuteSPWithQueryDSet("sp_AdminReport_Guillotine", paramsList);
            //report.Tables[0].TableName = "ProductDescription";
            ReportProductionPlanGuillotineModel resultObject = new ReportProductionPlanGuillotineModel();

            var ProductDesc = (from rw in report.Tables[0].AsEnumerable()
                          select new
                          {
                              PlanId = Convert.ToString(rw["PlanId"]),
                              ProductCode = Convert.ToString(rw["ProductCode"]),
                              Description = Convert.ToString(rw["Description"]),
                              ShopNo = Convert.ToString(rw["ShopNo"]),
                              Target = Convert.ToString(rw["Target"])
                          }).ToList();

            var RawMaterials = (from rw in report.Tables[1].AsEnumerable()
                          select new
                          {
                              StockNo = Convert.ToString(rw["StockNo"]),
                              QCNo = Convert.ToString(rw["QCNo"]),
                              Color = Convert.ToString(rw["Color"]),
                          }).ToList();

            var MachinePreConditionAndSetup = (from rw in report.Tables[2].AsEnumerable()
                          select new
                          {
                              Description = Convert.ToString(rw["Description"]),
                              IsCheck = Convert.ToString(rw["IsCheck"]),
                              Remark = Convert.ToString(rw["Remark"]),
                          }).ToList();

            var CuttingPrograms = (from rw in report.Tables[3].AsEnumerable()
                          select new
                          {
                              Description = Convert.ToString(rw["Description"]),
                              IsCheck = Convert.ToString(rw["IsCheck"]),
                              TrimWaste = Convert.ToString(rw["TrimWaste"])
                          }).ToList();

            var ColorSorting = (from rw in report.Tables[4].AsEnumerable()
                          select new
                          {
                              Name = Convert.ToString(rw["Name"])
                          }).ToList();

            var CuttingProcess = (from rw in report.Tables[5].AsEnumerable()
                          select new
                          {
                              Date = Convert.ToString(rw["Date"]),
                              StartTime = Convert.ToString(rw["StartTime"]),
                              EndTime = Convert.ToString(rw["EndTime"]),
                              SetUpHr = Convert.ToString(rw["SetUpHr"]),
                              MachineHr = Convert.ToString(rw["MachineHr"]),
                              ReSetupHr = Convert.ToString(rw["ReSetupHr"]),
                              ProdDownHr = Convert.ToString(rw["ProdDownHr"]),
                              MachDownHr = Convert.ToString(rw["MachDownHr"]),
                              SumHr = Convert.ToString(rw["SumHr"]),
                              CalPad = Convert.ToString(rw["CalPad"]),
                              GoodPad = Convert.ToString(rw["GoodPad"]),
                              BadPad = Convert.ToString(rw["BadPad"]),
                              Remark = Convert.ToString(rw["Remark"]),
                              TheEmployee = Convert.ToString(rw["TheEmployee"])

                          }).ToList();

            var InProcessInspections = (from rw in report.Tables[6].AsEnumerable()
                          select new
                          {
                              Description = Convert.ToString(rw["Description"]),
                              IsCheck = Convert.ToString(rw["IsCheck"]),
                          }).ToList();

            var OutgoingInspection = (from rw in report.Tables[7].AsEnumerable()
                          select new
                          {
                              Date = Convert.ToString(rw["Date"]),
                              StartTime = Convert.ToString(rw["StartTime"]),
                              EndTime = Convert.ToString(rw["EndTime"]),
                              SumTime = Convert.ToString(rw["SumTime"]),
                              GoodPad = Convert.ToString(rw["GoodPad"]),
                              BadPad = Convert.ToString(rw["BadPad"]),
                              Remark = Convert.ToString(rw["Remark"]),
                              PackingEmployee = Convert.ToString(rw["PackingEmployee"])

                          }).ToList();

            var PackWrap = (from rw in report.Tables[8].AsEnumerable()
                          select new
                          {
                              PackOutsource = Convert.ToBoolean(rw["PackOutsource"]),
                              Wrap = Convert.ToBoolean(rw["Wrap"])
                          }).ToList();

            var WasteAnalysis = (from rw in report.Tables[9].AsEnumerable()
                          select new
                          {
                              Description = Convert.ToString(rw["Description"]),
                              Amount = Convert.ToString(rw["Amount"])
                          }).ToList();

            var PadNumber = (from rw in report.Tables[10].AsEnumerable()
                          select new
                          {
                              PadNumber = Convert.ToString(rw["PadNumber"]),
                          }).ToList();

            resultObject.PackWrap = new SubClassPackWrap();
            

            resultObject.Shop_No = ProductDesc[0].ShopNo;
            resultObject.PadNumber = int.Parse(PadNumber[0].PadNumber == "" ? "0" : PadNumber[0].PadNumber);
            resultObject.ProductDescription = new SubClassProductDescription
            {
                ProductCode = ProductDesc[0].ProductCode,
                Description = ProductDesc[0].Description,
                Target = ProductDesc[0].Target
            };

            resultObject.RawMaterials = new List<RawMaterial>();
            foreach (var item in RawMaterials)
            {
                resultObject.RawMaterials.Add(new RawMaterial
                {
                    StockNo = item.StockNo,
                    QCNo = item.QCNo,
                    Color = item.Color
                });

            }

            resultObject.MachinePreConditionAndSetup = new List<PreconditionsAndSetup>();
            foreach (var item in MachinePreConditionAndSetup)
            {
                resultObject.MachinePreConditionAndSetup.Add(new PreconditionsAndSetup
                {
                    Description = item.Description,
                    Remark = item.Remark,
                    IsCheck = bool.Parse(item.IsCheck)
                });

            }

            resultObject.CuttingPrograms = new List<CuttingProgram>();
            foreach (var item in CuttingPrograms)
            {
                resultObject.CuttingPrograms.Add(new CuttingProgram
                {
                    Description = item.Description,
                    IsCheck = bool.Parse(item.IsCheck == "" ? "false" : item.IsCheck),
                    TrimWaste = double.Parse(item.TrimWaste == "" ? "0" : item.TrimWaste),
                    //TrimWaste = double.TryParse(item.TrimWaste, out double TrimWaste) ? TrimWaste : 0,

                });

            }

            resultObject.ColorSorting = new List<ColorSortingRecord>();
            foreach (var item in ColorSorting)
            {
                resultObject.ColorSorting.Add(new ColorSortingRecord
                {
                    Name = item.Name
                });

            }

            resultObject.CuttingProcess = new SubClassCuttingProcess();
            resultObject.CuttingProcess.CuttingProcessRecord = new List<CuttingProcessRecord>();
            resultObject.CuttingProcess.TheEmployee = CuttingProcess[0].TheEmployee;
            foreach (var item in CuttingProcess)
            {
                resultObject.CuttingProcess.CuttingProcessRecord.Add(new CuttingProcessRecord
                {
                    Date = item.Date,
                    StartTime = item.StartTime,
                    EndTime = item.EndTime,
                    //SetUpHr = double.TryParse(item.SetUpHr, out double SetUpHr) ? SetUpHr : 0,
                    //MachineHr = double.TryParse(item.MachineHr, out double MachineHr) ? MachineHr : 0,
                    //ReSetupHr = double.TryParse(item.ReSetupHr, out double ReSetupHr) ? ReSetupHr : 0,
                    //ProdDownHr = double.TryParse(item.ProdDownHr, out double ProdDownHr) ? ProdDownHr : 0,
                    //MachDownHr = double.TryParse(item.MachDownHr, out double MachDownHr) ? MachDownHr : 0,
                    SetUpHr = double.Parse(item.SetUpHr == "" ? "0" : item.SetUpHr),
                    MachineHr = double.Parse(item.MachineHr == "" ? "0" : item.MachineHr),
                    ReSetupHr = double.Parse(item.ReSetupHr == "" ? "0" : item.ReSetupHr),
                    ProdDownHr = double.Parse(item.ProdDownHr == "" ? "0" : item.ProdDownHr),
                    MachDownHr = double.Parse(item.MachDownHr == "" ? "0" : item.MachDownHr),
                    SumHr = double.Parse(item.SumHr),
                    CalPad = double.Parse(item.GoodPad),
                    GoodPad = double.Parse(item.GoodPad),
                    BadPad = double.Parse(item.BadPad)

                });
            }

            resultObject.InProcessInspections = new List<InProcessInspection>();
            foreach (var item in InProcessInspections)
            {
                resultObject.InProcessInspections.Add(new InProcessInspection
                {
                    Description = item.Description,
                    //IsCheck = bool.TryParse(item.IsCheck, out bool IsCheck) ? IsCheck : false,
                    IsCheck = bool.Parse(item.IsCheck == "" ? "false" : item.IsCheck)
                });

            }

            resultObject.OutgoingInspection = new SubClassOutgoingInspection();
            resultObject.OutgoingInspection.OutgoingInspectionRecord = new List<OutgoingInspection>();
            foreach (var item in OutgoingInspection)
            {
                resultObject.OutgoingInspection.OutgoingInspectionRecord.Add(new OutgoingInspection
                {
                    Date = item.Date,
                    StartTime = item.StartTime,
                    EndTime = item.EndTime,
                    SumTime = double.Parse(item.SumTime == "" ? "0" : item.SumTime),
                    GoodPad = double.Parse(item.GoodPad),
                    BadPad = double.Parse(item.BadPad),
                    PackingEmployee = item.PackingEmployee,
                    Remark = item.Remark,


                });
            }
            resultObject.PackWrap.PackOutsource = PackWrap[0].PackOutsource;
            resultObject.PackWrap.Wrap = PackWrap[0].Wrap;

            resultObject.WasteAnalysis = new SubClassWasteAnalysis();
            resultObject.WasteAnalysis.WasteAnalysisRecord = new List<WasteAnalysis>();
            foreach (var item in WasteAnalysis)
            {
                resultObject.WasteAnalysis.WasteAnalysisRecord.Add(new WasteAnalysis
                {
                    Description = item.Description,
                    Amount = Convert.ToInt32(float.Parse(item.Amount == "" ? "0" : item.Amount))

                });

            }



            return resultObject;
        }

        public ReportProductionPlanPackingModel GetPackingReport(string data)
        {
            var paramsList = PlanId(data);
            var report = _directSqlRepository.ExecuteSPWithQueryDSet("sp_AdminReport_Packing", paramsList);
            //report.Tables[0].TableName = "ProductDescription";
            ReportProductionPlanPackingModel resultObject = new ReportProductionPlanPackingModel();


            var ProductDesc = (from rw in report.Tables[0].AsEnumerable()
                         select new
                         {
                             PlanId = Convert.ToString(rw["PlanId"]),
                             Header = Convert.ToString(rw["MachineName"]),
                             ProductCode = Convert.ToString(rw["ProductCode"]),
                             Description = Convert.ToString(rw["Description"]),
                             ShopNo = Convert.ToString(rw["ShopNo"]),
                             Target = Convert.ToString(rw["Target"])
                         }).ToList();

            var RawMaterials = (from rw in report.Tables[1].AsEnumerable()
                          select new
                          {
                              StockNo = Convert.ToString(rw["StockNo"]),
                              QCNo = Convert.ToString(rw["QCNo"]),
                              Color = Convert.ToString(rw["Color"]),
                          }).ToList();
            var listRawComponent = (from rw in report.Tables[2].AsEnumerable()
                                    select new
                                    {
                                        Component = Convert.ToString(rw["Component"]),
                                        StockNo = Convert.ToString(rw["StockNo"]),
                                        LotNo = Convert.ToString(rw["LotNo"]),
                                    }).ToList();

            var MachinePreConditionAndSetup = (from rw in report.Tables[3].AsEnumerable()
                          select new
                          {
                              Description = Convert.ToString(rw["Description"]),
                              IsCheck = Convert.ToString(rw["IsCheck"]),
                              Remark = Convert.ToString(rw["Remark"]),
                          }).ToList();

            var FiveTestingRecord = (from rw in report.Tables[4].AsEnumerable()
                          select new
                          {
                              Description = Convert.ToString(rw["Description"]),
                              Example_1 = Convert.ToBoolean(rw["Example_1"]),
                              Example_2 = Convert.ToBoolean(rw["Example_2"]),
                              Example_3 = Convert.ToBoolean(rw["Example_3"]),
                              Example_4 = Convert.ToBoolean(rw["Example_4"]),
                              Example_5 = Convert.ToBoolean(rw["Example_5"]),
                              Remark = Convert.ToString(rw["Remark"]),
                          }).ToList();

            var ColorSorting = (from rw in report.Tables[5].AsEnumerable()
                          select new
                          {
                              Name = Convert.ToString(rw["Name"])
                          }).ToList();

            var PackingProcess = (from rw in report.Tables[6].AsEnumerable()
                          select new
                          {
                              Date = Convert.ToString(rw["Date"]),
                              StartTime = Convert.ToString(rw["StartTime"]),
                              EndTime = Convert.ToString(rw["EndTime"]),
                              SetUpHr = Convert.ToString(rw["SetUpHr"]),
                              MachineHr = Convert.ToString(rw["MachineHr"]),
                              ReSetupHr = Convert.ToString(rw["ReSetupHr"]),
                              ProdDownHr = Convert.ToString(rw["ProdDownHr"]),
                              MachDownHr = Convert.ToString(rw["MachDownHr"]),
                              SumHr = Convert.ToString(rw["SumHr"]),
                              GoodPad = Convert.ToString(rw["GoodPad"]),
                              BadPad = Convert.ToString(rw["BadPad"]),
                              Remark = Convert.ToString(rw["Remark"]),
                              TheEmployee = Convert.ToString(rw["TheEmployee"])
                          }).ToList();

            var WasteAnalysis = (from rw in report.Tables[7].AsEnumerable()
                          select new
                          {
                              Description = Convert.ToString(rw["Description"]),
                              Amount = Convert.ToString(rw["Amount"]),
                              Remark = Convert.ToString(rw["Remark"])
                          }).ToList();

            resultObject.Header = ProductDesc[0].Header;
            resultObject.Shop_No = ProductDesc[0].ShopNo;
            resultObject.ProductDescription = new SubClassProductDescription
            {
                ProductCode = ProductDesc[0].ProductCode,
                Description = ProductDesc[0].Description,
                Target = ProductDesc[0].Target
            };
            resultObject.RawMaterials = new List<RawMaterial>();
            foreach (var item in RawMaterials)
            {
                resultObject.RawMaterials.Add(new RawMaterial
                {
                    StockNo = item.StockNo,
                    QCNo = item.QCNo,
                    Color = item.Color
                });

            }
            resultObject.RawComponents = new List<RawComponent>();
            foreach (var item in listRawComponent)
            {
                resultObject.RawComponents.Add(new RawComponent
                {
                    Component = item.Component,
                    StockNo = item.StockNo,
                    LotNo = item.LotNo
                });

            }
            resultObject.MachinePreConditionAndSetup = new List<PreconditionsAndSetup>();
            foreach (var item in MachinePreConditionAndSetup)
            {
                resultObject.MachinePreConditionAndSetup.Add(new PreconditionsAndSetup
                {
                    Description = item.Description,
                    Remark = item.Remark,
                    IsCheck = bool.Parse(item.IsCheck)
                });

            }
            resultObject.FiveTestingRecord = new List<FiveTesting>();
            foreach (var item in FiveTestingRecord)
            {
                resultObject.FiveTestingRecord.Add(new FiveTesting
                {
                    Description = item.Description,
                    Example_1 = item.Example_1,
                    Example_2 = item.Example_2,
                    Example_3 = item.Example_3,
                    Example_4 = item.Example_4,
                    Example_5 = item.Example_5,
                    Remark = item.Remark
                });

            }
            resultObject.ColorSorting = new List<ColorSortingRecord>();
            foreach (var item in ColorSorting)
            {
                resultObject.ColorSorting.Add(new ColorSortingRecord
                {
                    Name = item.Name
                });

            }

            resultObject.PackingProcess = new SubClassPackingProcess();
            resultObject.TheEmployee = PackingProcess[0].TheEmployee;
            resultObject.PackingProcess.PackingProcessRecord = new List<PackingProcessRecord>();
            foreach (var item in PackingProcess)
            {
                resultObject.PackingProcess.PackingProcessRecord.Add(new PackingProcessRecord
                {
                    Date = item.Date,
                    StartTime = item.StartTime,
                    EndTime = item.EndTime,
                    //SetUpHr = double.TryParse(item.SetUpHr, out double SetUpHr) ? SetUpHr : 0,
                    //MachineHr = double.TryParse(item.MachineHr, out double MachineHr) ? MachineHr : 0,
                    //ReSetupHr = double.TryParse(item.ReSetupHr, out double ReSetupHr) ? ReSetupHr : 0,
                    //ProdDownHr = double.TryParse(item.ProdDownHr, out double ProdDownHr) ? ProdDownHr : 0,
                    //MachDownHr = double.TryParse(item.MachDownHr, out double MachDownHr) ? MachDownHr : 0,
                    SetUpHr = double.Parse(item.SetUpHr == "" ? "0" : item.SetUpHr),
                    MachineHr = double.Parse(item.MachineHr == "" ? "0" : item.MachineHr),
                    ReSetupHr = double.Parse(item.ReSetupHr == "" ? "0" : item.ReSetupHr),
                    ProdDownHr = double.Parse(item.ProdDownHr == "" ? "0" : item.ProdDownHr),
                    MachDownHr = double.Parse(item.MachDownHr == "" ? "0" : item.MachDownHr),
                    SumHr = double.Parse(item.SumHr),
                    GoodPad = double.Parse(item.GoodPad),
                    BadPad = double.Parse(item.BadPad),
                    Remark = item.Remark

                });

            }

            resultObject.WasteAnalysis = new SubClassWasteAnalysis();
            resultObject.WasteAnalysis.WasteAnalysisRecord = new List<WasteAnalysis>();
            foreach (var item in WasteAnalysis)
            {
                resultObject.WasteAnalysis.WasteAnalysisRecord.Add(new WasteAnalysis
                {
                    Description = item.Description,
                    //Amount = int.Parse(item.Amount == "" ? "0": item.Amount),
                    Amount = Convert.ToInt32(float.Parse(item.Amount == "" ? "0" : item.Amount)),

                    Remark = item.Remark
                });

            }

            return resultObject;

        }

        public DataTable GetWasteReport(ReportDateModel data)
        {
            var paramsList = ReportDate(data);
            return _directSqlRepository.ExecuteSPWithQuery("sp_AdminReport_Waste", paramsList);
        }

        public DataSet GetWasteReports(ReportDateModel data)
        {
            var paramsList = ReportDate(data);
            return _directSqlRepository.ExecuteSPWithQueryDSet("sp_AdminReport_Waste", paramsList);
        }

        public DataTable GetMachineLossReport(ReportDateModel data)
        {
            var paramsList = ReportDate(data);
            return _directSqlRepository.ExecuteSPWithQuery("sp_AdminReport_Loss", paramsList);
        }

        public DataTable GetORReport(ReportDateModel data)
        {
            var paramsList = ReportDate(data);
            return _directSqlRepository.ExecuteSPWithQuery("sp_AdminReport_OR", paramsList);
        }

        public DataTable GetSPCReport(ReportTimeCriteriaModel data)
        {
            var paramsList = ReportCreiteria(data);
            return _directSqlRepository.ExecuteSPWithQuery("sp_Report_SPC", paramsList);
        }

        public DataTable GetElectricityReport(ReportTimeCriteriaModel data)
        {
            var paramsList = ReportCreiteria(data);
            return _directSqlRepository.ExecuteSPWithQuery("sp_Report_Electricity", paramsList);
        }

        public DataTable GetProductionSummaryReport(ReportTimeCriteriaModel data)
        {
            var paramsList = ReportCreiteria(data);
            return _directSqlRepository.ExecuteSPWithQuery("sp_Report_Production_Summary", paramsList);
        }

        public DataTable GetOperatingTimeReport(ReportTimeCriteriaModel data)
        {
            var paramsList = ReportCreiteria(data);
            return _directSqlRepository.ExecuteSPWithQuery("sp_Report_Operating_Time", paramsList);
        }

        public DataTable GetActualDesignSpeedReport(ReportTimeCriteriaModel data)
        {
            var paramsList = ReportCreiteria(data);
            return _directSqlRepository.ExecuteSPWithQuery("sp_Report_Actual_Design_Speed", paramsList);
        }

        public DataTable GetMaintenanceReport(ReportTimeCriteriaModel data)
        {
            var paramsList = ReportCreiteria(data);
            return _directSqlRepository.ExecuteSPWithQuery("sp_Report_Maintenance", paramsList);
        }

        public DataTable GetCostAnalysisReport(ReportTimeCriteriaModel data)
        {
            var paramsList = ReportCreiteria(data);
            return _directSqlRepository.ExecuteSPWithQuery("sp_Report_Cost_Analysis", paramsList);
        }

        public DataTable GetHSEReport(ReportTimeCriteriaModel data)
        {
            var paramsList = ReportCreiteria(data);
            return _directSqlRepository.ExecuteSPWithQuery("[sp_Report_HSE]", paramsList);
        }

        public DataTable GetAttendantReport(ReportTimeCriteriaModel data)
        {
            var paramsList = ReportCreiteria(data);
            return _directSqlRepository.ExecuteSPWithQuery("[sp_Report_Attendant]", paramsList);
        }


        #endregion

    }
}
