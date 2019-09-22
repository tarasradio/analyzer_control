using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteppersControlCore.Controllers
{
    public class Controller
    {
        protected Dictionary<int, int> steppers;

        public Controller()
        {
            steppers = new Dictionary<int, int>();
        }
    }
}
