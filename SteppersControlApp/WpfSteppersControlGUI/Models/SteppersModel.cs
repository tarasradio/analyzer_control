using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using SteppersControlCore;

namespace WpfSteppersControlGUI.Models
{
    public class SteppersModel : BaseModel
    {
        private StepperModel selectedStepper;
        public ObservableCollection<Stepper> Steppers { get; set; }

        public StepperModel SelectedStepper
        {
            get { return selectedStepper; }
            set
            {
                selectedStepper = value;
                OnPropertyChanged("SelectedStepper");
            }
        }

        public SteppersModel(IList<Stepper> steppers)
        {
            this.Steppers = new ObservableCollection<Stepper>(steppers);
        }

        public void UpdateSteppersStates(ushort[] states)
        {
            int i = 0;
            foreach(ushort state in states)
            {
                Steppers[i].Status = state;
            }
        }
    }
}
