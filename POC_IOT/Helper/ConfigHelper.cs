using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

using Newtonsoft.Json;
using GrovePi;

namespace POC_IOT.Helper
{
    public class ConfigHelper
    {
        private class ConfigMetaData
        {
            public string deviceId = "Rpi3arfontai";
            public string iotHubUri = "iotdemoarfontaicc5f7";
            public string iotHubKey = "0lGxEtYycs9DFO9ig1/TycT0EVM5xRU6dxSId9vzomI=";

            public long pasDeTemps = 5000;

            public bool temperatureIsConnected = false;
            public Pin temperaturePort = Pin.AnalogPin0;
            public bool temperatureAndHumidityIsConnected = false;
            public Pin temperatureAndHumidityPort = Pin.DigitalPin2;
            public bool humidityIsConnected = false;
            public Pin humidityPort = Pin.AnalogPin1;
            public bool soundIsConnected = false;
            public Pin soundPort = Pin.AnalogPin2;
            public bool greenLedIsConnected = false;
            public Pin greenLedPort = Pin.DigitalPin5;
            public bool redLedIsConnected = false;
            public Pin redLedPort = Pin.DigitalPin6;
            public bool buzzerIsConnected = false;
            public Pin buzzerPort = Pin.DigitalPin7;
            public bool buttonIsConnected = false;
            public Pin buttonPort = Pin.DigitalPin8;
        }

        public static ConfigHelper Config;
        private ConfigMetaData config = new ConfigMetaData();
        private IStorageFile file;
        
        public string DeviceId
        {
            set
            {
                if (Config != null)
                {
                    config.deviceId = value;
                }
            }
            get
            {
                if (Config != null)
                {
                    return config.deviceId;
                }
                else
                {
                    return new ConfigMetaData().deviceId;
                }
            }
        }

        // on ne sauvegarde pas la clé du device (ie pas dans ConfigMetaData)
        private string _deviceKey;
        public string Devicekey
        {
            set
            {
                _deviceKey = value;
            }
            get
            {
                return _deviceKey;
            }
        }

        public string IoTHubName
        {
            set
            {
                if (Config != null)
                {
                    config.iotHubUri = value;
                }
            }
            get
            {
                if (Config != null)
                {
                    return config.iotHubUri;
                }
                else
                {
                    return new ConfigMetaData().iotHubUri;
                }
            }
        }

        public string IoTHubKey
        {
            set
            {
                if (Config != null)
                {
                    config.iotHubKey = value;
                }
            }
            get
            {
                if (Config != null)
                {
                    return config.iotHubKey;
                }
                else
                {
                    return new ConfigMetaData().iotHubKey;
                }
            }
        }

        public string IoTHubConnectionString
        {
            get
            {
                return string.Format("HostName={0}.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey={1}", 
                    IoTHubName, IoTHubKey);
            }
        }

        public long PasDeTemps
        {
            set
            {
                if (Config != null)
                {
                    config.pasDeTemps = value;
                }
            }
            get
            {
                if (Config != null)
                {
                    return config.pasDeTemps;
                }
                else
                {
                    return new ConfigMetaData().pasDeTemps;
                }
            }
        }

        public bool TemperatureIsConnected
        {
            set
            {
                if (Config != null)
                {
                    config.temperatureIsConnected = value;
                }
            }
            get
            {
                if (Config != null)
                {
                    return config.temperatureIsConnected;
                }
                else
                {
                    return new ConfigMetaData().temperatureIsConnected;
                }
            }
        }

        public Pin TemperaturePort
        {
            set
            {
                if (Config != null)
                {
                    config.temperaturePort = value;
                }
            }
            get
            {
                if (Config != null)
                {
                    return config.temperaturePort;
                }
                else
                {
                    return new ConfigMetaData().temperaturePort;
                }
            }
        }

        public bool TemperatureAndHumidityIsConnected
        {
            set
            {
                if (Config != null)
                {
                    config.temperatureAndHumidityIsConnected = value;
                }
            }
            get
            {
                if (Config != null)
                {
                    return config.temperatureAndHumidityIsConnected;
                }
                else
                {
                    return new ConfigMetaData().temperatureAndHumidityIsConnected;
                }
            }
        }

        public Pin TemperatureAndHumidityPort
        {
            set
            {
                if (Config != null)
                {
                    config.temperatureAndHumidityPort = value;
                }
            }
            get
            {
                if (Config != null)
                {
                    return config.temperatureAndHumidityPort;
                }
                else
                {
                    return new ConfigMetaData().temperatureAndHumidityPort;
                }
            }
        }

        public bool HumidityIsConnected
        {
            set
            {
                if (Config != null)
                {
                    config.humidityIsConnected = value;
                }
            }
            get
            {
                if (Config != null)
                {
                    return config.humidityIsConnected;
                }
                else
                {
                    return new ConfigMetaData().humidityIsConnected;
                }
            }
        }

        public Pin HumidityPort
        {
            set
            {
                if (Config != null)
                {
                    config.humidityPort = value;
                }
            }
            get
            {
                if (Config != null)
                {
                    return config.humidityPort;
                }
                else
                {
                    return new ConfigMetaData().humidityPort;
                }
            }
        }

        public bool SoundIsConnected
        {
            set
            {
                if (Config != null)
                {
                    config.soundIsConnected = value;
                }
            }
            get
            {
                if (Config != null)
                {
                    return config.soundIsConnected;
                }
                else
                {
                    return new ConfigMetaData().soundIsConnected;
                }
            }
        }

        public Pin SoundPort
        {
            set
            {
                if (Config != null)
                {
                    config.soundPort = value;
                }
            }
            get
            {
                if (Config != null)
                {
                    return config.soundPort;
                }
                else
                {
                    return new ConfigMetaData().soundPort;
                }
            }
        }

        public bool GreenLedIsConnected
        {
            set
            {
                if (Config != null)
                {
                    config.greenLedIsConnected = value;
                }
            }
            get
            {
                if (Config != null)
                {
                    return config.greenLedIsConnected;
                }
                else
                {
                    return new ConfigMetaData().greenLedIsConnected;
                }
            }
        }

        public Pin GreenLedPort
        {
            set
            {
                if (Config != null)
                {
                    config.greenLedPort = value;
                }
            }
            get
            {
                if (Config != null)
                {
                    return config.greenLedPort;
                }
                else
                {
                    return new ConfigMetaData().greenLedPort;
                }
            }
        }

        public bool RedLedIsConnected
        {
            set
            {
                if (Config != null)
                {
                    config.redLedIsConnected = value;
                }
            }
            get
            {
                if (Config != null)
                {
                    return config.redLedIsConnected;
                }
                else
                {
                    return new ConfigMetaData().redLedIsConnected;
                }
            }
        }

        public Pin RedLedPort
        {
            set
            {
                if (Config != null)
                {
                    config.redLedPort = value;
                }
            }
            get
            {
                if (Config != null)
                {
                    return config.redLedPort;
                }
                else
                {
                    return new ConfigMetaData().redLedPort;
                }
            }
        }


        public bool BuzzerIsConnected
        {
            set
            {
                if (Config != null)
                {
                    config.buzzerIsConnected = value;
                }
            }
            get
            {
                if (Config != null)
                {
                    return config.buzzerIsConnected;
                }
                else
                {
                    return new ConfigMetaData().buzzerIsConnected;
                }
            }
        }

        public Pin BuzzerPort
        {
            set
            {
                if (Config != null)
                {
                    config.buzzerPort = value;
                }
            }
            get
            {
                if (Config != null)
                {
                    return config.buzzerPort;
                }
                else
                {
                    return new ConfigMetaData().buzzerPort;
                }
            }
        }


        public bool ButtonIsConnected
        {
            set
            {
                if (Config != null)
                {
                    config.buttonIsConnected = value;
                }
            }
            get
            {
                if (Config != null)
                {
                    return config.buttonIsConnected;
                }
                else
                {
                    return new ConfigMetaData().buttonIsConnected;
                }
            }
        }

        public Pin ButtonPort
        {
            set
            {
                if (Config != null)
                {
                    config.buttonPort = value;
                }
            }
            get
            {
                if (Config != null)
                {
                    return config.buttonPort;
                }
                else
                {
                    return new ConfigMetaData().buttonPort;
                }
            }
        }

        public static void Init(bool force)
        {
            if (Config == null || force)
                Config = new ConfigHelper();
        }

        public static void Save()
        {
            if (Config != null)
                Config.SaveFile();
        }


        private ConfigHelper()
        {
            InitFile();
            LoadFile();
        }
        
        private void InitFile()
        {
            Task.Run(
                async () =>
                {
                    var localFolder = ApplicationData.Current.LocalFolder;
                    this.file = await localFolder.CreateFileAsync("App.config", CreationCollisionOption.OpenIfExists);
                }).Wait();
        }

        private void SaveFile()
        {
            var content = JsonConvert.SerializeObject(config);

            Task.Run(
                async () =>
                {
                    await Windows.Storage.FileIO.WriteTextAsync(this.file, content);
                }).Wait();
        }

        private void LoadFile()
        {
            var content = string.Empty;

            Task.Run(
                async () =>
                {
                    content = await Windows.Storage.FileIO.ReadTextAsync(this.file);
                }).Wait();

            if (content != string.Empty)
                config = JsonConvert.DeserializeObject<ConfigMetaData>(content);
            else
                config = new ConfigMetaData();
        }
    }
}
