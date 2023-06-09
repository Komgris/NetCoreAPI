﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class Constans
    {

        public const string CURRENT_USER = "CurrentUser";

        #region redis

        public const string SIGNAL_R_CHANNEL_PRODUCTION_PLAN = "production-plan";
        public const string SIGNAL_R_CHANNEL_DASHBOARD = "dashboard";

        public class SIGNAL_R_CHANNEL
        {
            public const string CHANNEL_MESSAGE = "transfer-message";
            public const string CHANNEL_COMMAND = "command-channel";
            public const string CHANNEL_PRODUCTION_PLAN = "production-plan";
            public const string CHANNEL_MASTER_DATA = "master-data";
            public const string CHANNEL_MASTER_DATA_OPERATION = "master-data-operation";
            public const string CHANNEL_MACHINE = "machine";
            public const string CHANNEL_DASHBOARD = "dashboard-CachedCH-3M_Custom_Dashboard";
        }

        public class RedisKey
        {

            public const string MACHINE = "machine-3m";
            public const string MACHINE_LIST = "machine-list";
            public const string PRODUCTION_PLAN = "production-plan";
            public const string COMPONENT = "component-production-plan";
            public const string ACTIVE_PRODUCTION_PLAN = "active-production-plan";
            public const string MASTER_DATA = "master-data";
            public const string MASTER_DATA_Oper = "master-data-Oper";
            public const string TOKEN = "token";
            public const string PRODUCTION = "production";
            public const string MACHINE_INFO = "machine-info";
            public const string Dashboard = "dashboard-info";
            public const string Dashboard_ActiveProcess = "active-process";
        }

        public enum DashboardCachedCH
        {
            Dole_Custom_Dashboard = 0
        }

        #endregion

        #region State

        public enum PRODUCTION_PLAN_STATUS : int
        {
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

        public static class MACHINE_STATUS
        {
            public const int Stop = 0;
            public const int Running = 1;
            public const int Idle = 2;
            public const int Error = 3;
            public const int NA = 4;
        }

        public enum AlertStatus : int
        {
            New = 0,
            Processing = 1,
            Edited = 2
        }

        public enum ComponentStatus : int
        {
            Ready = 1
        }

        public enum NetworkState
        {
            Good,
            Poor,
            Bad
        }

        public enum ProductionPlanBuffer : int
        {
            TARGET_BUFFER = 100,
            HOUR_BUFFER = 6
        }

        public static class CompareMapping
        {
            public const int InvalidDateTime = 1;
            public const int InvalidTarget = 2;
            public const int PlanFinished = 3;
            public const int NEW = 4;
            public const int NoProduct = 5;
            public const int Inprocess = 6;
            public const int NoMachine = 7;
            public const int Preparatory = 8;
            public const int Production = 9;
            public const int InvalidStandardRate = 10;
        }

        #endregion

        #region Typeof
        public enum AlertType : int
        {
            Component = 0,
            MACHINE = 1
        }

        public enum DataFrame
        {
            Default = 0,
            Daily = 1,
            Weekly = 2,
            Monthly = 3,
            Yearly = 4,
            Custom = 5
        }

        public enum MasterDataType
        {
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
            , WastesLevel1
            , WastesLevel2
            , MachineType
            , ComponentType
            , ProductFamily
            , ProductGroup
            , ProductType
            , MaterialType
            , TeamType
            , Team
            , SystemParameter
            , UserPosition
            , Education
            , ProcessType
            , UserGroup
            , Language
        }

        public enum ReportType
        {
            Daily = 0,
            Weekly = 1,
            Monthly = 2,
            Yearly = 3,
            DoleCalendar = 4,
            Details = 99
        }

        public enum BoardcastType
        {
            All = 0,
            KPI = 1,
            Output = 2,
            Waste = 3,
            Loss = 4,
            TimeUtilisation = 5,
            ActiveKPI = 6,
            ActiveProductionSummary = 7,
            ActiveProductionOutput = 8,
            ActiveWaste = 9,
            ActiveWasteMat = 10,
            ActiveWasteCase = 11,
            ActiveWasteMC = 12,
            ActiveWasteTime = 13,
            ActiveLoss = 14,
            ActiveTimeUtilisation = 15,
            ActiveProductionEvent = 16,
            ActiveOperator = 17,
            ActiveMachineInfo = 18,
            ActiveMachineSpeed = 19,
            ActiveMachineLossEvent = 20,
            ActiveMachineStatus = 21,
            End = 99
        }

        public enum ManagementDashboardType
        {
            KPI = 0,
            ProductionSummary = 1,
            WasteByMaterial = 2,
            WasteBySymptom = 3,
            MachineLossTree = 4,
            MachineLossLvl1 = 5,
            MachineLossLvl2 = 6,
            MachineLossLvl3 = 7,
            MachineLossHighLight = 8,
            CapacityUtilization = 9
        }

        public enum CustomDashboardType
        {
            OEE = 0,
            Production = 1,
            HSE = 2,
            Quality = 3,
            Delivery = 4,
            Spoilage = 5,
            NonePrime = 6,
            Attendance = 7,

            MachineStatus = 10,
            PlanvsActual = 11
        }

        public enum DataTypeGroup
        {
            All = 0,
            Loss = 1,
            Waste = 2,
            Operators = 3,
            Produce = 4,
            Process = 5,
            HSE = 6,
            Machine = 7,
            PlanActual = 8,
            McCalc = 9,
            ProduceCalc = 10,
            None = 100
        }

        public enum TriggerType
        {
            CustomDashboard = 1,
            ActiveProcess = 2,
            CalcHour = 3,
            MachineStatus = 4,
            MachineCounter = 5
        }

        public enum LossRecordingType
        {
            Auto = 0,
            Manual = 1,
            None = 2
        }

        public static Dictionary<int, string> MachineStatusString = new Dictionary<int, string>()
        {
            { 0,"Stop"},{ 1,"Run"},{ 2,"Idle"}
        };

        public static Dictionary<string, string> Dashboard3MConfig = new Dictionary<string, string>()
        {
                       {"active-process"             , "sp_get_active_process"},
                       {"sp_get_machine"                    , "sp_get_machine"},
                       {"sp_get_output"                     , "sp_get_output"},
                       {"sp_get_production_target_output"   , "sp_get_production_target_output"},
                       {"sp_get_quality"                    , "sp_get_quality"},
                       {"TopDowntime"             , "sp_get_runtime_losses"},
                       {"ProductionEvent"             , "sp_get_machine_event"},
                       {"ProductionPerformance"             , "sp_get_machine_performance"}
        };
        #endregion

    }
}
