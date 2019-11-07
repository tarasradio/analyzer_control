using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

using WpfSteppersControlGUI.Models;
using SteppersControlCore;

namespace WpfSteppersControlGUI.ViewModels
{
    public class SteppersControlViewModel : BaseModel
    {
        public SteppersModel Steppers { get; set; }

        public SteppersControlViewModel(SteppersModel model)
        {
            this.Steppers = model;
        }
    }
}
