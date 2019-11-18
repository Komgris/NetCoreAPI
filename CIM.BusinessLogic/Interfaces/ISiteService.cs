using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.BusinessLogic
{
    public interface ISiteService
    {
        List<SiteModel> List();
    }
}
