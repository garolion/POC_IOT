using Microsoft.Azure.Devices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace C2DTest
{

    /// <summary>
    /// An internal call used to generate the JSON message
    /// </summary>
    internal class DeviceCommand
    {
        public string Name { get; set; }
        public string MessageId { get; set; }
        public string CreatedTime { get; set; }

        public Dictionary<string, string> Parameters { get; set; }
    }


    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }


        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await SendMessageAsync();
        }

        //To Be Replaced by the line bellow at the Azure Function
        //public static async Task Run(HttpRequestMessage req, TraceWriter log)
        private async Task SendMessageAsync()
        {
            string deviceId;

            #region section to uncomment in the Azure function
            //string jsonRequest = await req.Content.ReadAsStringAsync();
            //if (jsonRequest.Length == 0) { return; }

            //data = JsonConvert.DeserializeObject(jsonRequest);

            //if (data.DeviceId == null || data.DeviceId.Equals(string.Empty)) { return; }
            //deviceId = data.DeviceId.ToString();
            //log.Info($"JsonJata.DeviceId: " + data.DeviceId.ToString());
            #endregion

            #region section to remove in the Azure function
            deviceId = "Rpi3arfontai";
            #endregion

            string connectionString = "HostName=iotdemoarfontaicc5f7.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=0lGxEtYycs9DFO9ig1/TycT0EVM5xRU6dxSId9vzomI=";

            try
            {
                ServiceClient serviceClient = ServiceClient.CreateFromConnectionString(connectionString);

                //log.Info($"Connected to IoT Hub!");

                var message = new DeviceCommand();
                message.Name = "SendAlert";
                message.MessageId = Guid.NewGuid().ToString();
                message.CreatedTime = DateTime.Now.ToString();

                var commandMessage = new Microsoft.Azure.Devices.Message(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(message)));

                await serviceClient.SendAsync(deviceId, commandMessage);
            }
            catch (Exception ex)
            {
                //log.Info($"An Exception occured " + ex.StackTrace);
            }
        }
    }
}
