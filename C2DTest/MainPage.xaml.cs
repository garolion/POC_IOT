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

using Microsoft.Azure.Devices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

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

        private async Task SendMessageAsync()
        {
        string connectionString = "HostName=iotdemoarfontaicc5f7.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=0lGxEtYycs9DFO9ig1/TycT0EVM5xRU6dxSId9vzomI=";

            ServiceClient serviceClient = ServiceClient.CreateFromConnectionString(connectionString);

            try
            {
                var message = new DeviceCommand();
                message.Name = "SwitchLed";
                message.MessageId = new Guid().ToString();
                message.CreatedTime = DateTime.Now.ToString();




                var commandMessage = new Microsoft.Azure.Devices.Message(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(message)));

                await serviceClient.SendAsync("Rpi3arfontai", commandMessage);
                                
            }
            // Raised by Azure IoT Hub if you mentionned an unknown DeviceId
            catch (Exception ex)
            { }
        }
    }
}
