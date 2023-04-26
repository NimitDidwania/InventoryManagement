using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Common.Logger
{
    public interface ICustomLogger
    {
        void Debug(string message);
        Task Error(Exception ex);
    }
}