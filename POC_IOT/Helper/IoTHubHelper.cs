using System;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Common.Exceptions;
using Newtonsoft.Json;

using IoTSuiteLib;
using System.Collections.Generic;

namespace POC_IOT.Helper
{
    class IoTHubHelper
    {
        private DeviceClient deviceClient;
        private RegistryManager registryManager;
        private ICloudToDeviceCallback _callback;

        private bool isConnectedToAzureIoTHub = false;
        public bool IsConnectedToAzureIoTHub
        {
            get
            {
                return isConnectedToAzureIoTHub;
            }

        }

        public IoTHubHelper(ICloudToDeviceCallback callback)
        {
            this._callback = callback;
        }

        public async Task ConnectToIoTHub()
        {
            try
            {
                string connectionString = ConfigHelper.Config.IoTHubConnectionString;

                registryManager = RegistryManager.CreateFromConnectionString(connectionString);

                await AddDeviceAsync();

                deviceClient = DeviceClient.Create(
                    string.Format("{0}.azure-devices.net", ConfigHelper.Config.IoTHubName),
                    new DeviceAuthenticationWithRegistrySymmetricKey(ConfigHelper.Config.DeviceId, ConfigHelper.Config.Devicekey),
                    Microsoft.Azure.Devices.Client.TransportType.Amqp);

                await RegisterDeviceAsync();

                ReceiveC2dAsync();

                isConnectedToAzureIoTHub = true;
            }
            catch (Exception)
            {
            }
        }

        private async Task AddDeviceAsync()
        {
            Device device;

            try
            {
                string deviceName = ConfigHelper.Config.DeviceId;
                device = await registryManager.AddDeviceAsync(new Device(deviceName));
            }
            catch (DeviceAlreadyExistsException)
            {
                device = await registryManager.GetDeviceAsync(ConfigHelper.Config.DeviceId);
            }

            ConfigHelper.Config.Devicekey = device.Authentication.SymmetricKey.PrimaryKey;
        }

        private async Task RegisterDeviceAsync()
        {
            var data = new DeviceMetaData();
            data.Version = "1.0";
            data.IsSimulatedDevice = false;
            data.Properties.DeviceID = ConfigHelper.Config.DeviceId;
            data.Properties.FirmwareVersion = "1.42";
            data.Properties.HubEnabledState = true;
            data.Properties.Processor = "ARM";
            data.Properties.Platform = "UWP";
            data.Properties.SerialNumber = "123-456-7890";
            data.Properties.InstalledRAM = "1024 MB";
            data.Properties.ModelNumber = "Model 2-B";
            data.Properties.Manufacturer = "Raspberry";
            data.Properties.HubEnabledState = true;
            data.Properties.DeviceState = DeviceState.Normal;

            // à mi-chemin entre la localisation des devices 1 et 14 du scénario de base
            data.Properties.Latitude = 47.668159F;
            data.Properties.Longitude = -122.111515F;

            data.Commands.Add(new DeviceCommandDefinition("SendAlert"));

            data.Commands.Add(new DeviceCommandDefinition("SwitchLed"));

            data.Commands.Add(new DeviceCommandDefinition("ConfigLed",
                new DeviceCommandParameterDefinition[] {
                    new DeviceCommandParameterDefinition("GreenLed", DeviceCommandParameterType.Boolean),
                    new DeviceCommandParameterDefinition("RedLed", DeviceCommandParameterType.Boolean)
                }));

            var content = JsonConvert.SerializeObject(data);
            await deviceClient.SendEventAsync(new Microsoft.Azure.Devices.Client.Message(Encoding.UTF8.GetBytes(content)));
        }

        public async Task SendDeviceToCloudWeatherDataAsync(string temperature, string humidity)
        {
            var deviceId = ConfigHelper.Config.DeviceId;
            var telemetryDataPoint = new
            {
                deviceId = deviceId,
                temperature = float.Parse(temperature),
                humidity = float.Parse(humidity)
            };
            var messageString = JsonConvert.SerializeObject(telemetryDataPoint);

            var message = new Microsoft.Azure.Devices.Client.Message(Encoding.ASCII.GetBytes(messageString));
            await deviceClient.SendEventAsync(message);
        }

        private async Task ReceiveC2dAsync()
        {
            while (true)
            {
                Microsoft.Azure.Devices.Client.Message receivedMessage = await deviceClient.ReceiveAsync();
                if (receivedMessage == null) continue;

                string message = Encoding.ASCII.GetString(receivedMessage.GetBytes());

                _callback.NotifyMessage("Command received");

                try
                {
                    var command = JsonConvert.DeserializeObject<DeviceCommand>(message);

                    //workaround récupération des paramètres
                    command.ProcessParameters(message);

                    if (command != null)
                    {
                        _callback.NotifyMessage(string.Format("Command '{0}' received", command.Name));
                        _callback.CloudToDeviceNotification(command);
                    }
                }
                catch (Exception ex) { string mesg = ex.Message; }

                await deviceClient.CompleteAsync(receivedMessage);
            }
        }
    }
}
