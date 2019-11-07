using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteppersControlCore.Controllers
{
    public abstract class ControllerBase
    {
        protected Dictionary<int, int> steppers;

        public ControllerBase()
        {
            steppers = new Dictionary<int, int>();
        }
    }
}
