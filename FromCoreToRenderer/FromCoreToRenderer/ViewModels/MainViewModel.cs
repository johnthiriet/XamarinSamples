using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FromCoreToRenderer.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _colorName = "Black";

        public string ColorName
        {
            get => _colorName;
            set
            {
                if (_colorName != value)
                {
                    _colorName = value;
                    RaisePropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
