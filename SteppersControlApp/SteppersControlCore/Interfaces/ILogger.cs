using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteppersControlCore.Interfaces
{
    public interface ILogger
    {
        void Info(string message);
        void DemoInfo(string message);
        void ControllerInfo(string message);
    }
}
