using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.System.Threading;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using GrovePi;
using GrovePi.I2CDevices;
using GrovePi.Sensors;
using POC_IOT.Helper;
using IoTSuiteLib;

using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Common.Exceptions;
using Newtonsoft.Json;

using POC_IOT.ViewModel;

namespace POC_IOT
{
    public sealed partial class AccueilUserControl : UserControl, ICloudToDeviceCallback
    {
        ThreadPoolTimer _timer;
        ThreadPoolTimer _alertingTimer;
        public AccueilViewModel ViewModel { get; set; }
        private IoTHubHelper _ioTHubHelper;

        private readonly IBuildGroveDevices _deviceFactory = DeviceFactory.Build;
        private ITemperatureSensor _temperatureSensor = null;
        private ITemperatureAndHumiditySensor _temperatureAndHumiditySensor = null;
        private ISoundSensor _soundSensor = null;
        private IRotaryAngleSensor _rotaryAngleSensor = null;
        private ILed _greenLed = null;
        private ILed _redLed= null;
        private IBuzzer _buzzer = null;
        private IButtonSensor _button = null;
        private IRgbLcdDisplay _rgbLcdDisplay = null;

        private bool isInAlertingMode = false;
        private SensorStatus alertingButtonStatus;

        public AccueilUserControl()
        {
            this.InitializeComponent();
            ViewModel = new AccueilViewModel();

            ConfigHelper.Init(false);
            _ioTHubHelper = new IoTHubHelper(this);

            InitializeRaspBerryPi();
        }

        internal void InitializeRaspBerryPi()
        {
            try
            {
                _rgbLcdDisplay = _deviceFactory.RgbLcdDisplay();
                _rgbLcdDisplay.SetText("App started").SetBacklightRgb(0, 255, 255);

                if (ConfigHelper.Config.TemperatureIsConnected)
                    _temperatureSensor = _deviceFactory.TemperatureSensor(ConfigHelper.Config.TemperaturePort);

                if (ConfigHelper.Config.TemperatureAndHumidityIsConnected)
                    _temperatureAndHumiditySensor = _deviceFactory.TemperatureAndHumiditySensor(ConfigHelper.Config.TemperatureAndHumidityPort, Model.Dht11);

                if (ConfigHelper.Config.HumidityIsConnected)
                    _rotaryAngleSensor = _deviceFactory.RotaryAngleSensor(ConfigHelper.Config.HumidityPort);

                if (ConfigHelper.Config.SoundIsConnected)
                    _soundSensor = _deviceFactory.SoundSensor(ConfigHelper.Config.SoundPort);

                if (ConfigHelper.Config.GreenLedIsConnected)
                    _greenLed = _deviceFactory.Led(ConfigHelper.Config.GreenLedPort).ChangeState(SensorStatus.On);

                if (ConfigHelper.Config.RedLedIsConnected)
                    _redLed = _deviceFactory.Led(ConfigHelper.Config.RedLedPort).ChangeState(SensorStatus.On);
                
                if (ConfigHelper.Config.BuzzerIsConnected)
                    _buzzer = _deviceFactory.Buzzer(ConfigHelper.Config.BuzzerPort).ChangeState(SensorStatus.Off);

                if (ConfigHelper.Config.ButtonIsConnected)
                    _button = _deviceFactory.ButtonSensor(ConfigHelper.Config.ButtonPort);
            }
            catch (Exception)  {   }

            _timer = ThreadPoolTimer.CreatePeriodicTimer(Timer_Tick, TimeSpan.FromMilliseconds(ConfigHelper.Config.PasDeTemps));
        }

        private async void Timer_Tick(ThreadPoolTimer _timer)
        {
            var tempStr = "N/A";
            var humStr = "N/A";
            var soundStr = "N/A";
            
            if (ConfigHelper.Config.TemperatureAndHumidityIsConnected)
            {
                try
                {
                    tempStr = _temperatureAndHumiditySensor.TemperatureInCelsius().ToString();

                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        ViewModel.Temperature = tempStr;
                    });
                }
                catch (Exception) { }
            }
            else if (ConfigHelper.Config.TemperatureIsConnected)
            {
                try
                {
                    tempStr = _temperatureSensor.TemperatureInCelsius().ToString();

                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        ViewModel.Temperature = tempStr;
                    });
                }
                catch (Exception) { }
            }

            if (ConfigHelper.Config.HumidityIsConnected)
            {
                try
                {
                    humStr = (_rotaryAngleSensor.Degrees()/3).ToString();

                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        ViewModel.Humidity = humStr;
                    });
                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                }
            }

            if (ConfigHelper.Config.SoundIsConnected)
            {
                try
                {
                    soundStr = _soundSensor.SensorValue().ToString();

                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        ViewModel.Sound = soundStr;
                    });
                }
                catch (Exception) { }
            }

            if (isInAlertingMode && ConfigHelper.Config.ButtonIsConnected)
            {
                if (_button.CurrentState != alertingButtonStatus)
                {
                    StopAlerting();
                }
            }

            try
            {
                string bufferStr = new string(' ', 7 - humStr.Length);
                _rgbLcdDisplay.SetText(string.Format("Humidity{0}:{1}Temperature:{2}", bufferStr, humStr, tempStr));
            }
            catch (Exception) { }

            if (_ioTHubHelper.IsConnectedToAzureIoTHub)
            {
                await _ioTHubHelper.SendDeviceToCloudWeatherDataAsync(tempStr, humStr);
            }
        }

        private async void IoTHubConnectionButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await _ioTHubHelper.ConnectToIoTHub();

                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    ViewModel.IotHubStatus = "Connected";
                });
            }
            catch (Exception) { }
        }

        public void CloudToDeviceNotification(DeviceCommand command)
        {
            if (command != null)
            {
                switch (command.Name)
                {
                    case "SendAlert":
                        SendAlert();
                        break;
                    case "SwitchLed":
                        SwitchLed();
                        break;
                    case "ConfigLed":
                        var parameters = command.CommandParameters;
                        bool greenLedState = true;
                        bool redLedState = true;

                        if (parameters != null && parameters.Count >= 2 )
                        {
                            if (parameters.Exists(x => x.Name.Equals("GreenLed")))
                                greenLedState = (bool)parameters.Find(x => x.Name.Equals("GreenLed")).Value;
                            if (parameters.Exists(x => x.Name.Equals("RedLed")))
                                redLedState = (bool)parameters.Find(x => x.Name.Equals("RedLed")).Value;
                        }

                        ConfigLed(greenLedState, redLedState);
                        break;
                    default:
                        break;
                }
            }
        }

        private async void Alerting_Tick(ThreadPoolTimer _timer)
        {
            if (_buzzer.CurrentState == SensorStatus.On)
                _buzzer.ChangeState(SensorStatus.Off);
            else
                _buzzer.ChangeState(SensorStatus.On);
        }

        private void StartAlerting()
        {
            if (!isInAlertingMode)
            {
                isInAlertingMode = true;

                // on met off par défaut, en cas de debug pas à pas, pour ne pas laisser le buzzer sonner
                //_buzzer.ChangeState(SensorStatus.On);
                _buzzer.ChangeState(SensorStatus.Off);
                _alertingTimer = ThreadPoolTimer.CreatePeriodicTimer(Alerting_Tick, TimeSpan.FromMilliseconds(500));
            }
        }

        private void StopAlerting()
        {
            _buzzer.ChangeState(SensorStatus.Off);

            if (_alertingTimer != null)
            {
                _alertingTimer.Cancel();
            }
            isInAlertingMode = false;
        }
        
        private void SendAlert()
        {
            alertingButtonStatus = _button.CurrentState;
            StartAlerting();
        }

        private void SwitchLed()
        {
            if (_greenLed != null && _greenLed.CurrentState.Equals(SensorStatus.On))
                 _greenLed.ChangeState(SensorStatus.Off);
            else
                _greenLed.ChangeState(SensorStatus.On);

            if (_redLed != null && _redLed.CurrentState.Equals(SensorStatus.On))
                _redLed.ChangeState(SensorStatus.Off);
            else
                _redLed.ChangeState(SensorStatus.On);
        }

        private void ConfigLed(bool greenLedOn, bool redLedOn)
        {
            SensorStatus greenLedStatus = greenLedOn ? SensorStatus.On : SensorStatus.Off;
            SensorStatus redLedStatus = redLedOn ? SensorStatus.On : SensorStatus.Off;

            if (_greenLed != null)
                _greenLed.ChangeState(greenLedStatus);

            if (_redLed != null)
                _redLed.ChangeState(redLedStatus);
        }

        public void NotifyMessage(string message)
        {
            FeedbackBlock.Text = message;
        }
    }
}
