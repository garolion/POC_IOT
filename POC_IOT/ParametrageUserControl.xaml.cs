using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using GrovePi;

using POC_IOT.Helper;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace POC_IOT
{
    public sealed partial class ParametrageUserControl : UserControl
    {
        private AccueilUserControl accueilUserControl;

        public ParametrageUserControl()
        {
            this.InitializeComponent();
            ConfigHelper.Init(false);

            InitUI();
        }

        internal void RegisterAccueilPane(AccueilUserControl control)
        {
            accueilUserControl = control;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            ConfigHelper.Config.DeviceId = ObjectIdTextBox.Text;
            ConfigHelper.Config.IoTHubName = IoTHubNameTextBox.Text;
            ConfigHelper.Config.IoTHubKey = IoTHubKeyTextBox.Text;

            long pasDeTemps;
            bool isLong = long.TryParse(IntervalTextBox.Text, out pasDeTemps);
            if (isLong)
                ConfigHelper.Config.PasDeTemps = pasDeTemps;

            ConfigHelper.Config.TemperatureIsConnected = PortTemperatureToggleSwitch.IsOn;
            if (PortTemperatureToggleSwitch.IsOn)
                ConfigHelper.Config.TemperaturePort = (Pin)PortTemperatureComboBox.SelectedItem;

            ConfigHelper.Config.TemperatureAndHumidityIsConnected = PortTemperatureAndHumidityToggleSwitch.IsOn;
            if (PortTemperatureAndHumidityToggleSwitch.IsOn)
                ConfigHelper.Config.TemperatureAndHumidityPort = (Pin)PortTemperatureAndHumidityComboBox.SelectedItem;

            ConfigHelper.Config.HumidityIsConnected = PortHumidityToggleSwitch.IsOn;
            if (PortHumidityToggleSwitch.IsOn)
                ConfigHelper.Config.HumidityPort = (Pin)PortHumidityComboBox.SelectedItem;

            ConfigHelper.Config.SoundIsConnected = PortSoundToggleSwitch.IsOn;
            if (PortSoundToggleSwitch.IsOn)
                ConfigHelper.Config.SoundPort = (Pin)PortSoundComboBox.SelectedItem;

            ConfigHelper.Config.GreenLedIsConnected = PortGreenLedToggleSwitch.IsOn;
            if (PortGreenLedToggleSwitch.IsOn)
                ConfigHelper.Config.GreenLedPort = (Pin)PortGreenLedComboBox.SelectedItem;

            ConfigHelper.Config.RedLedIsConnected = PortRedLedToggleSwitch.IsOn;
            if (PortRedLedToggleSwitch.IsOn)
                ConfigHelper.Config.RedLedPort = (Pin)PortRedLedComboBox.SelectedItem;

            ConfigHelper.Config.BuzzerIsConnected = PortBuzzerToggleSwitch.IsOn;
            if (PortBuzzerToggleSwitch.IsOn)
                ConfigHelper.Config.BuzzerPort = (Pin)PortBuzzerComboBox.SelectedItem;

            ConfigHelper.Config.ButtonIsConnected = PortButtonToggleSwitch.IsOn;
            if (PortButtonToggleSwitch.IsOn)
                ConfigHelper.Config.ButtonPort = (Pin)PortButtonComboBox.SelectedItem;

            try
            {
                ConfigHelper.Save();
                accueilUserControl.InitializeRaspBerryPi();
                FeedbackTextBlock.Visibility = Visibility.Visible;
                FeedbackTextBlock.Text = "Settings successfully saved";
            }
            catch(Exception ex)
            {
                FeedbackTextBlock.Visibility = Visibility.Visible;
                FeedbackTextBlock.Text = "An error occured";
            }
        }

        private void ReinitButton_Click(object sender, RoutedEventArgs e)
        {
            ConfigHelper.Init(true);
            InitUI();
        }

        private void InitUI()
        {
            ObjectIdTextBox.Text = ConfigHelper.Config.DeviceId;
            IoTHubNameTextBox.Text = ConfigHelper.Config.IoTHubName;
            IoTHubKeyTextBox.Text = ConfigHelper.Config.IoTHubKey;

            IntervalTextBox.Text = ConfigHelper.Config.PasDeTemps.ToString();

            PortTemperatureToggleSwitch.IsOn = ConfigHelper.Config.TemperatureIsConnected;
            PortTemperatureComboBox.ItemsSource = Enum.GetValues(typeof(GrovePi.Pin));
            PortTemperatureComboBox.SelectedItem = ConfigHelper.Config.TemperaturePort;

            PortTemperatureAndHumidityToggleSwitch.IsOn = ConfigHelper.Config.TemperatureAndHumidityIsConnected;
            PortTemperatureAndHumidityComboBox.ItemsSource = Enum.GetValues(typeof(GrovePi.Pin));
            PortTemperatureAndHumidityComboBox.SelectedItem = ConfigHelper.Config.TemperatureAndHumidityPort;

            PortHumidityToggleSwitch.IsOn = ConfigHelper.Config.HumidityIsConnected;
            PortHumidityComboBox.ItemsSource = Enum.GetValues(typeof(GrovePi.Pin));
            PortHumidityComboBox.SelectedItem = ConfigHelper.Config.HumidityPort;

            PortSoundToggleSwitch.IsOn = ConfigHelper.Config.SoundIsConnected;
            PortSoundComboBox.ItemsSource = Enum.GetValues(typeof(GrovePi.Pin));
            PortSoundComboBox.SelectedItem = ConfigHelper.Config.SoundPort;

            PortGreenLedToggleSwitch.IsOn = ConfigHelper.Config.GreenLedIsConnected;
            PortGreenLedComboBox.ItemsSource = Enum.GetValues(typeof(GrovePi.Pin));
            PortGreenLedComboBox.SelectedItem = ConfigHelper.Config.GreenLedPort;

            PortRedLedToggleSwitch.IsOn = ConfigHelper.Config.RedLedIsConnected;
            PortRedLedComboBox.ItemsSource = Enum.GetValues(typeof(GrovePi.Pin));
            PortRedLedComboBox.SelectedItem = ConfigHelper.Config.RedLedPort;

            PortBuzzerToggleSwitch.IsOn = ConfigHelper.Config.BuzzerIsConnected;
            PortBuzzerComboBox.ItemsSource = Enum.GetValues(typeof(GrovePi.Pin));
            PortBuzzerComboBox.SelectedItem = ConfigHelper.Config.BuzzerPort;

            PortButtonToggleSwitch.IsOn = ConfigHelper.Config.ButtonIsConnected;
            PortButtonComboBox.ItemsSource = Enum.GetValues(typeof(GrovePi.Pin));
            PortButtonComboBox.SelectedItem = ConfigHelper.Config.ButtonPort;

            FeedbackTextBlock.Visibility = Visibility.Collapsed;
        }
    }
}
