using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IWTFMachine
    {
        void StepperMove(uint stepperNumber, uint steps, uint speed);
        void StepperRun(uint stepperNumber, uint speed);
        void StepperStop(uint stepperNumber);
    }
}
