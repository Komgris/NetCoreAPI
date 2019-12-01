using CIM.Model;
using System.Collections.Generic;

namespace CIM.BusinessLogic.Interfaces
{
    public interface ISiteService
    {
        List<SiteModel> List();
    }
}
