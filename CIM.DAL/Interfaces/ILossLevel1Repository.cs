using CIM.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;


namespace CIM.DAL.Interfaces
{
    //    class ILossLevel1Repository
    public interface ILossLevel1Repository
    {
        public List<LossLevel1Model> All();
    }
}
