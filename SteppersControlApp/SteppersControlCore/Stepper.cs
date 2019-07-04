using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteppersControlCore
{
    public class Stepper
    {
        public int Number { get; set; }
        public string Name { get; set; }
        public int Speed { get; set; }

        public Stepper()
        {
            Number = 0;
            Name = "Some stepper";
            Speed = 100;
        }
    }
}
