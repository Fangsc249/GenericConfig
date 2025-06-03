using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ConfigTool.ConfigCore
{
    public abstract class ConfigBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
