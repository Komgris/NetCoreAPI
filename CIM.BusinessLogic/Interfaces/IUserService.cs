﻿using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.BusinessLogic.Interfaces
{
    public interface IUserService
    {
        void Register(RegisterUserModel model);
    }
}
