using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace FromRendererToCore.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            TappedCommand = new Command<string>(s =>
            {
                DynamicContent = s;
            });
        }

        public ICommand TappedCommand { get; }

        private string _dynamicContent;
        public string DynamicContent
        {
            get => _dynamicContent;
            set
            {
                if (_dynamicContent != value)
                {
                    _dynamicContent = value;
                    RaisePropertyChanged();
                }
            }
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
