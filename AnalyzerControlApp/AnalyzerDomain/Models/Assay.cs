using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerDomain.Models
{
    public enum AssayStepsTypes
    {
        F_NCS_PURGEWASH,
        ASP,
        AWER,
        DISPMIX,
        NCS_PURGEWASH,
        INC,
        F_PURGEWASH,
        WA,
        DISP,
        PURGEWASH,
        OM,
        CALC
    }

    public class AssayStep
    {
        public int StepNumber { get; set; } = 0;
        public AssayStepsTypes StepType { get; set; }
        public int SourceWell { get; set; } = 0;
        public int TargetWell { get; set; } = 0;
        public int Quantity { get; set; } = 0;
        public int Duration { get; set; } = 0;
        public bool Interruptible { get; set; } = false;
    }

    public class AssayDescription
    {
        public List<AssayStep> Steps = new List<AssayStep>();
    }

    public class AssayDescriptionFabric
    {
        public AssayDescription BuildSimpleAssayDescription(int sample_NDIL, int tw2, int tw3, int tacw, int inc1_duration, int inc2_duration)
        {
            AssayDescription description = new AssayDescription();

            List<AssayStep> steps = new List<AssayStep>();

            steps.Add(new AssayStep { StepNumber = 1, StepType = AssayStepsTypes.F_NCS_PURGEWASH });
            steps.Add(new AssayStep { StepNumber = 2, StepType = AssayStepsTypes.ASP, SourceWell = 2, Quantity = tw2});
            steps.Add(new AssayStep { StepNumber = 3, StepType = AssayStepsTypes.AWER });
            steps.Add(new AssayStep { StepNumber = 4, StepType = AssayStepsTypes.ASP, SourceWell = 7, Quantity = sample_NDIL });
            steps.Add(new AssayStep { StepNumber = 5, StepType = AssayStepsTypes.DISPMIX, TargetWell = 4, Quantity = (tw2 + sample_NDIL) });
            steps.Add(new AssayStep { StepNumber = 6, StepType = AssayStepsTypes.NCS_PURGEWASH });
            steps.Add(new AssayStep { StepNumber = 7, StepType = AssayStepsTypes.INC, Duration = inc1_duration, Interruptible = true });
            steps.Add(new AssayStep { StepNumber = 8, StepType = AssayStepsTypes.F_PURGEWASH });
            steps.Add(new AssayStep { StepNumber = 9, StepType = AssayStepsTypes.WA, SourceWell = 4 });
            steps.Add(new AssayStep { StepNumber = 10, StepType = AssayStepsTypes.ASP, SourceWell = 3, Quantity = tw3 });
            steps.Add(new AssayStep { StepNumber = 11, StepType = AssayStepsTypes.DISP, TargetWell = 4, Quantity = tw3 });
            steps.Add(new AssayStep { StepNumber = 12, StepType = AssayStepsTypes.PURGEWASH });
            steps.Add(new AssayStep { StepNumber = 13, StepType = AssayStepsTypes.INC, Duration = inc2_duration, Interruptible = true });
            steps.Add(new AssayStep { StepNumber = 14, StepType = AssayStepsTypes.F_PURGEWASH });
            steps.Add(new AssayStep { StepNumber = 15, StepType = AssayStepsTypes.OM, SourceWell = 5 });
            steps.Add(new AssayStep { StepNumber = 16, StepType = AssayStepsTypes.ASP, SourceWell = 4, Quantity = tacw });
            steps.Add(new AssayStep { StepNumber = 17, StepType = AssayStepsTypes.DISP, TargetWell = 5, Quantity = tacw });
            steps.Add(new AssayStep { StepNumber = 18, StepType = AssayStepsTypes.OM, SourceWell = 5 });
            steps.Add(new AssayStep { StepNumber = 19, StepType = AssayStepsTypes.NCS_PURGEWASH });
            steps.Add(new AssayStep { StepNumber = 20, StepType = AssayStepsTypes.CALC });

            description.Steps = steps;

            return description;
        }
    }
}
