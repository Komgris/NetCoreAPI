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
        void Insert(LossLevel1Model parameters);
        void DeleteByIdLossLevel1(int Id);
        List<LossLevel1Model> GetByIdLossLevel1(int Id);
        void EditLossLevel1(LossLevel1Model lossLevel1);
    }
}
