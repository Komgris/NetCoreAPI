using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Interfaces
{
    public interface ILossLevel1Service
    {
        List<LossLevel1Model> ListAllLossLevel1();
        void InsertLossLevel1(LossLevel1Model parameters);
    }
}
