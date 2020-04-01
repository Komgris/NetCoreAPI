using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class Constans
    {
        public const string CURRENT_USER = "CurrentUser";

        public const string SIGNAL_R_CHANNEL_PRODUCTION_PLAN = "production-plan";

        public class RedisKey {

            public const string MACHINE = "machine:";
            public const string MACHINE_LIST = "machine-list:";
            public const string PRODUCTION_PLAN = "production-plan";
            public const string COMPONENT = "component-production-plan";
            public const string ACTIVE_PRODUCTION_PLAN = "active-production-plan";
            public const string MASTER_DATA = "master-data";
        }

        public class PRODUCTION_PLAN_STATUS {

            public const string STARTED = "STARTED";
            public const string STOP = "STOP";
        }

    }
}
