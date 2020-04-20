﻿using System;
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
            New                    = 1,	
            Production             = 2,	
            Finished               = 3,	
            Preparatory            = 4,	
            Changeover             = 5,	
            CleaningAndSanitation  = 6,	
            MealTeaBreak           = 7,	
            Hold                   = 8,	
            Cancel                 = 9
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

        public enum ImportProductionPlanFile : int
        {
            OFFSET_TOP_ROW = 5,
            OFFSET_BOTTOM_ROW = 2,
            PLAN_COL = 3,
            ROUTE_COL = 4,
            PRODUCT_COL = 5,
            TARGET_COL = 15,
            UNIT_COL = 16,
            PLANSTART_COL = 17,
            PLANFINISH_COL = 18
        }

        public enum ProductionPlanLimit : int
        {
            TARGET_LIMIT = 100,
            HOUR_LIMIT = 6
        }

        public class SIGNAL_R_CHANNEL
        {

            public const string CHANNEL_MESSAGE = "transfer-message";
            public const string CHANNEL_COMMAND = "command-channel";
            public const string CHANNEL_PRODUCTION_PLAN = "production-plan";

        }
          
    }
}
