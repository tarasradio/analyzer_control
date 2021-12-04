using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AnalyzerDomain.Models
{
    public abstract class ModelBase : INotifyPropertyChanged
    {
        protected void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
