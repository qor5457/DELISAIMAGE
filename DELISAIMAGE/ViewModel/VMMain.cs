using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using DELISAIMAGE.Model;
using JetBrains.Annotations;

namespace DELISAIMAGE.ViewModel
{
    public class VMMain : INotifyPropertyChanged
    {
        #region Notify
        public ObservableCollection<ModelImage> ModelImages { get; set; } = new();
        public ObservableCollection<BoxLocation> BoxLocations { get; set; } = new();
        public void BoxDataAdd()
        {
            BoxLocation boxLocation = new BoxLocation()
            {
                Height = 100,
                Width = 100,
                X = 100,
                Y = 100
            };
            BoxLocations.Add(boxLocation);
        }
        
        #endregion
        public event PropertyChangedEventHandler? PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}