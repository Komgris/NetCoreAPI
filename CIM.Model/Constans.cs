using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class Constans
    {
        public const string CURRENT_USER = "CurrentUser";

        public const string SIGNAL_R_CHANNEL_PRODUCTION_PLAN = "production-plan";

        public static int DEFAULT_LOSS_LV3 = 1;

        public class RedisKey {

            public const string MACHINE = "machine";
            public const string MACHINE_LIST = "machine-list";
            public const string PRODUCTION_PLAN = "production-plan";
            public const string COMPONENT = "component-production-plan";
            public const string ACTIVE_PRODUCTION_PLAN = "active-production-plan";
            public const string MASTER_DATA = "master-data";
        }

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
            Cancel = 9
        }

        public enum AlertType : int
        {
            Component = 0,
            MACHINE = 1
        }

        public enum AlertStatus : int
        {
            New = 0,
            Processing = 1
        }

        public enum ComponentStatus : int
        {
            Ready = 1
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
        }

        public class SIGNAL_R_CHANNEL
        {

            public const string CHANNEL_MESSAGE = "transfer-message";
            public const string CHANNEL_COMMAND = "command-channel";
            public const string CHANNEL_PRODUCTION_PLAN = "production-plan";

        }
    } 
}
