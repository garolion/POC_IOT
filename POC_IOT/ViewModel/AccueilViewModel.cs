using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace POC_IOT.ViewModel
{
    public class AccueilViewModel : INotifyPropertyChanged
    {
        private string temperature;
        public string Temperature
        {
            get { return temperature; }
            set
            {
                temperature = value;
                this.OnPropertyChanged();
            }
        }

        private string humidity;
        public string Humidity
        {
            get { return humidity; }
            set
            {
                humidity = value;
                this.OnPropertyChanged();
            }
        }

        private string sound;
        public string Sound
        {
            get { return sound; }
            set
            {
                sound = value;
                this.OnPropertyChanged();
            }
        }

        private string iotHubStatus;
        public string IotHubStatus
        {
            get { return iotHubStatus; }
            set
            {
                iotHubStatus = value;
                this.OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public AccueilViewModel()
        {
            this.Temperature = "N/A";
            this.Humidity = "N/A";
            this.Sound = "N/A";
            this.IotHubStatus = "Déconnecté";
        }
    }
}
