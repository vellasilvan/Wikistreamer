using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared.Interfaces;
using Shared.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static Confluent.Kafka.ConfigPropertyNames;

namespace Consumer.KafkaConsumer
{
    public class KafkaConsumer : IMessageConsumer
    {
        private readonly IConsumer<Null, string> _consumer;
        private readonly ILogger<KafkaConsumer> _logger;
        private readonly string _topic;
        
        public KafkaConsumer(IOptions<KafkaConsumerOptions> options, ILogger<KafkaConsumer> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            var consumerOptions = options.Value;
            var config = new ConsumerConfig
            {
                BootstrapServers = consumerOptions?.BootstrapServers ?? throw new ArgumentNullException(nameof(options)),
                GroupId = consumerOptions.GroupId,
                AutoOffsetReset = consumerOptions.AutoOffsetReset
            };
            _topic = consumerOptions.Topic ?? throw new ArgumentNullException(nameof(options));

            _consumer = new ConsumerBuilder<Null, string>(config).Build();
            
        }
        public void Consume(CancellationToken cancellationToken = default)
        {
            _consumer.Subscribe(_topic);
            _logger.LogInformation($"Subscribed to topic {_topic}");

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        var consumeResult = _consumer.Consume(cancellationToken);
                        if (consumeResult != null)
                        {
                            _logger.LogInformation($"Consumed message '{consumeResult.Message.Value}' at: '{consumeResult.Offset}'");
                        }
                    }
                    catch (ConsumeException e)
                    {
                        _logger.LogError($"Error occurred: {e.Error.Reason}");
                    }
                }
            }

            catch (OperationCanceledException)
            {
                // Ensure the consumer leaves the group cleanly and final offsets are committed.
                _consumer.Close();
                _logger.LogInformation("Consumer closed gracefully.");
            }
        }

        public void Dispose()
        {
            _consumer?.Dispose();
        }
    }
}
