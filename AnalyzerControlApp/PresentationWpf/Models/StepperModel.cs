
using AnalyzerConfiguration;

namespace PresentationWPF.Models
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
