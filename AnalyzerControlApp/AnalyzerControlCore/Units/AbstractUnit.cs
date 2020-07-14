using AnalyzerControlCore.MachineControl;
using System.Collections.Generic;
using AnalyzerConfiguration;
using System.Runtime.CompilerServices;

namespace AnalyzerControlCore.Units
{
    // Calculation of necessary steps using the target and current position (in steps)
    // The target position is determined by the absolute number of steps from the starting position (0)
    // General rule: needed steps = taget_position - current_position
    // Example 1: position = 3000; target position = 2000; necessary steps = 2000 - 3000 = -1000
    // Example 2: position = 2000; target position = 3000; necessary steps = 3000 - 2000 = 1000
    // Example 3: position = -3000; target position = -2000; necessary steps = -2000 - (-3000) = 1000
    // Example 4: position = -2000; target position = -3000; necessary steps = -3000 - (-2000) = -1000

    public abstract class AbstractUnit
    {
        protected Dictionary<int, int> steppers;
        protected ICommandExecutor executor;

        public AbstractUnit(ICommandExecutor executor)
        {
            this.executor = executor;
            steppers = new Dictionary<int, int>();
        }
    }
}
