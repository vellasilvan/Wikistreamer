using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.POCO
{
    public class KafkaOptions
    {
        public string BootstrapServers { get; set; } = string.Empty;
        public string Topic { get; set; } = string.Empty;
        public Acks Acks {  get; set; } = Acks.All;
        public bool AllowAutoCreateTopics { get; set; } = true;
    }
}
