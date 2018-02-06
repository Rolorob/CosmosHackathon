using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CosmosHackathon.Domain
{
    public class DeviceReading
    {
        public DeviceReading(string deviceId, DateTime timestamp, decimal value, string channelId, string unit, IDictionary<string, string> properties = null)
        {
            DeviceId = deviceId;
            Timestamp = timestamp;
            Value = value;
            ChannelId = channelId;
            Unit = unit;
            Properties = properties ?? new Dictionary<string, string>();
        }

        [JsonConstructor]
        public DeviceReading()
        {
        }

        [JsonProperty]
        public string DeviceId { get; private set; }

        [JsonProperty]
        public DateTime Timestamp { get; private set; }

        [JsonProperty]
        public decimal Value { get; private set; }

        [JsonProperty]
        public string ChannelId { get; private set; }

        [JsonProperty]
        public string Unit { get; private set; }

        [JsonProperty]
        public IDictionary<string, string> Properties { get; private set; }

        public override string ToString()
        {
            var dictionaryString = string.Join(", ", Properties
                .Select(kv => string.Format("{0} = {1}", kv.Key, kv.Value)));

            return $"[{DeviceId}-{ChannelId}] {Timestamp}: {Value} {Unit} [{dictionaryString}]";
        }
    }
}
