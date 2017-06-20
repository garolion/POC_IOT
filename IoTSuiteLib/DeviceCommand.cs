using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IoTSuiteLib
{
    public class DeviceCommand
    {
        //private Dictionary<string, string> _parameterStore;
        private Dictionary<string, bool> _parameters;
        private List<DeviceCommandParameter> _commandParameters;

        public DeviceCommand()
        {
            //Parameters = new List<DeviceCommandParameter>();

            _parameters = new Dictionary<string, bool>();
            _commandParameters = new List<DeviceCommandParameter>();
        }

        public string Name { get; set; }
        public string MessageId { get; set; }
        public string CreatedTime { get; set; }

        //[JsonProperty(PropertyName="Parameters")]
        //public Dictionary<string, string> ParametersStore
        //{
        //    get
        //    {
        //        return _parameterStore;
        //    }
        //    set
        //    {
        //        if (value == _parameterStore)
        //            return;
        //        _parameterStore = value;
        //        if (_parameterStore.Count > 0)
        //        {
        //            var parameters = new List<DeviceCommandParameter>();
        //            foreach (var parameter in _parameterStore)
        //            {
        //                parameters.Add(new DeviceCommandParameter() { Name = parameter.Key, Value = parameter.Value });
        //            }
        //            Parameters.AddRange(parameters);
        //        }
        //    }
        //}
        //[JsonIgnore]
        //public List<DeviceCommandParameter> Parameters { get; set; }

        [JsonIgnore]
        public Dictionary<string, string> Parameters { get; set; }

        [JsonIgnore]
        public List<DeviceCommandParameter> CommandParameters 
        {
            get
            {
                return _commandParameters;
            }
        }      
        
        public void ProcessParameters(string commandString)
        {
            _commandParameters = new List<DeviceCommandParameter>();

            int index_start = commandString.IndexOf("Parameters") + 12;
            int index_stop = commandString.IndexOf("}", index_start) + 1;
            string paramString = commandString.Substring(index_start, index_stop - index_start);

            if (paramString.Length>=4 && !paramString.Substring(0,4).Equals("null"))
            {
                var parameters = JsonConvert.DeserializeObject<Dictionary<string, object>>(paramString);
                var commandParameters = new List<DeviceCommandParameter>();

                foreach (var parameter in parameters)
                {
                    commandParameters.Add(new DeviceCommandParameter() { Name = parameter.Key, Value = parameter.Value });
                }
                _commandParameters.AddRange(commandParameters);
            }
        }
    }
}
