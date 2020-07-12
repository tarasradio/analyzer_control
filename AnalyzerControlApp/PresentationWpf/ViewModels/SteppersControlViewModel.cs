
using PresentationWPF.Models;

namespace PresentationWPF.ViewModels
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
