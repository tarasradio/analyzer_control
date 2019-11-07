using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SteppersControlCore;

namespace WpfSteppersControlGUI.Models
{
    public class StepperModel : BaseModel
    {
        public int Number { get; set; }
        public string Name { get; set; }
        public bool Reverse { get; set; }
        public int NumberSteps { get; set; }
        public int FullSpeed { get; set; }

        public StepperModel(Stepper stepper)
        {
            this.Number = stepper.Number;
            this.Name = stepper.Name;
            this.Reverse = stepper.Reverse;
            this.NumberSteps = (int)stepper.NumberSteps;
            this.FullSpeed = (int)stepper.FullSpeed;
        }
    }
}
