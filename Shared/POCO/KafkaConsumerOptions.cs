using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.POCO
{
    public class KafkaConsumerOptions
    {
        public string BootstrapServers { get; set; } = string.Empty;
        public string Topic { get; set; } = string.Empty;
        public string GroupId { get; set; } = string.Empty;
        public AutoOffsetReset AutoOffsetReset { get; set; } = AutoOffsetReset.Earliest;
    }
}
