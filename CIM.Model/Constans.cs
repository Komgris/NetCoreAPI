using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class Constans {

        public const string CURRENT_USER = "CurrentUser";

        #region redis

        public const string SIGNAL_R_CHANNEL_PRODUCTION_PLAN = "production-plan";
        public const string SIGNAL_R_CHANNEL_DASHBOARD = "dashboard";

        public class SIGNAL_R_CHANNEL {
            public const string CHANNEL_MESSAGE = "transfer-message";
            public const string CHANNEL_COMMAND = "command-channel";
            public const string CHANNEL_PRODUCTION_PLAN = "production-plan";
        }

        public class RedisKey {

            public const string MACHINE = "machine";
            public const string MACHINE_LIST = "machine-list";
            public const string PRODUCTION_PLAN = "production-plan";
            public const string COMPONENT = "component-production-plan";
            public const string ACTIVE_PRODUCTION_PLAN = "active-production-plan";
            public const string MASTER_DATA = "master-data";
        }

        public enum DashboardCachedCH {
            Dole_Production_Cup = 0,
            Dole_Production_PKO = 1,
            Dole_Overview_Cup = 2,
            Dole_Overview_PKO = 3,
        }

        #endregion

        #region State

        public enum PRODUCTION_PLAN_STATUS : int {
            New = 1,
            Production = 2,
            Finished = 3,
            Preparatory = 4,
            Changeover = 5,
            CleaningAndSanitation = 6,
            MealTeaBreak = 7,
            Hold = 8,
            Cancel = 9,
            DownTime = 10,
            Pause = 11
        }

        public static class MACHINE_STATUS {
            public const int Unknown = 0;
            public const int Idle = 1;
            public const int Running = 2;
            public const int Stop = 3;
            public const int Error = 4;
            public const int NA = 5;
        }

        public enum AlertStatus : int {
            New = 0,
            Processing = 1,
            Edited = 2
        }

        public enum ComponentStatus : int {
            Ready = 1
        }

        public enum NetworkState {
            Good,
            Poor,
            Bad
        }

        public enum ProductionPlanBuffer : int {
            TARGET_BUFFER = 100,
            HOUR_BUFFER = 6
        }

        public static class CompareMapping {
            public const int InvalidDateTime = 1;
            public const int InvalidTarget = 2;
            public const int PlanFinished = 3;
            public const int NEW = 4;
            public const int NoProduct = 5;
            public const int Inprocess = 6;
        }

        #endregion

        #region Typeof
        public enum AlertType : int {
            Component = 0,
            MACHINE = 1
        }

        public enum DataFrame {
            Default,
            Daily,
            Weekly,
            Monthly,
            Yearly,
            Custom
        }

        public enum BoardcastType {
            All,
            KPI,
            Output,
            Waste,
            Loss,
            TimeUtilisation,
            ActiveKPI,
            ActiveProductionSummary,
            ActiveProductionOutput,
            ActiveWaste,
            ActiveWasteMat,
            ActiveWasteCase,
            ActiveWasteMC,
            ActiveWasteTime,
            ActiveLoss,
            ActiveTimeUtilisation,
            ActiveProductionEvent,
            ActiveOperator,
            ActiveMachineInfo,
            ActiveMachineSpeed,
            ActiveMachineLossEvent,
            ActiveMachineStatus,
            End
        }

        public enum MasterDataType {
            All
            , LossLevel3s
            , RouteMachines
            , Components
            , Machines
            , Routes
            , Products
            , ProductionPlan
            , ProductGroupRoutes
            , WastesByProductType
            , ProcessDriven
            , ProductsByCode
            , ProductionStatus
            , Units
            , CompareResult
            , WastesLevel2
            , MachineType
            , ComponentType
            , ProductFamily
            , ProductGroup
            , ProductType
            , MaterialType
            , TeamType
            , Team
            , UserPosition
            , Education
            , ProcessType
            , UserGroup
            , Language
        }

        public enum ReportType {
            Daily,
            Weekly,
            Monthly,
            Yearly,
            DoleCalendar
        }

        public enum ManagementDashboardType {
            KPI,
            ProductionSummary,
            WasteByMaterial,
            WasteBySymptom,
            MachineLossTree,
            MachineLossLvl1,
            MachineLossLvl2,
            MachineLossLvl3,
            MachineLossHighLight,
            CapacityUtilization
        }

        #endregion

    }
}
