using System;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Microsoft.Azure.Devices;


private class DeviceCommand
{
    public string Name { get; set; }
    public string MessageId { get; set; }
    public string CreatedTime { get; set; }

    public Dictionary<string, string> Parameters { get; set; }
}

public static async Task Run(HttpRequestMessage req, TraceWriter log)
{
    string deviceId;

    string jsonRequest = await req.Content.ReadAsStringAsync();
    if (jsonRequest.Length == 0) { return; }

    dynamic data = JsonConvert.DeserializeObject(jsonRequest);

    if (data.DeviceId == null || data.DeviceId.Equals(string.Empty)) { return; }
    deviceId = data.DeviceId.ToString();
    log.Info($"JsonJata.DeviceId: " + data.DeviceId.ToString());
    
    string connectionString = "Paste_your_connectionString_to_IoT_Hub";

    try
    {
        ServiceClient serviceClient = ServiceClient.CreateFromConnectionString(connectionString);

        log.Info($"Connected to IoT Hub!");

        var message = new DeviceCommand();
        message.Name = "SendAlert";
        message.MessageId = Guid.NewGuid().ToString();
        message.CreatedTime = DateTime.Now.ToString();

        var commandMessage = new Microsoft.Azure.Devices.Message(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(message)));

        await serviceClient.SendAsync(deviceId, commandMessage);
    }
    catch (Exception ex)
    {
        log.Info($"An Exception occured " + ex.StackTrace);
    }
}
