using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        int Commit();
    }
}
