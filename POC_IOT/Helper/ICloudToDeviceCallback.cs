using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IoTSuiteLib;

namespace POC_IOT.Helper
{
    interface ICloudToDeviceCallback
    {
        void CloudToDeviceNotification(DeviceCommand command);

        void NotifyMessage(string message);
    }
}
