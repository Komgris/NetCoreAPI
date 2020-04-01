using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace CIM.Model
{
    public class ErrorMessages
    {
        public class PRODUCTION_PLAN {
            public const string PLAN_STARTED = "Production plan already started.";

            public const string CANNOT_START_ROUTE_EMPTY = "Cannot start production plan without route.";
        }
    }
}
