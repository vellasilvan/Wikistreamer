using Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Interfaces;
using Microsoft.Extensions.Logging;
using Confluent.Kafka;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Shared.POCO;

namespace Producer.KafkaProducer
{
    internal class KafkaProducer<T> : IMessageProducer<T>
    {

        private readonly IProducer<Null, string> _producer;
        private readonly string _topic;
        private readonly ILogger _logger;
        
        public KafkaProducer(ILogger<KafkaProducer<T>> logger, IOptions<KafkaProducerOptions> options)
        {

            var kafkaOptions = options.Value;
            _topic = kafkaOptions.Topic;

            var config = new ProducerConfig
            {
                BootstrapServers = kafkaOptions?.BootstrapServers ?? throw new ArgumentNullException(nameof(options)),
                AllowAutoCreateTopics = kafkaOptions.AllowAutoCreateTopics,
                Acks = kafkaOptions.Acks,
            };

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _producer = new ProducerBuilder<Null, string>(config).Build();
        }

        public async Task ProduceAsync(T message, CancellationToken cancellationToken = default)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message), "Cannot produce a null message.");

            try
            {
                string jsonMessage = JsonSerializer.Serialize(message);

                var deliveryResult = await _producer.ProduceAsync(topic: _topic,
                    new Message<Null, string>
                    {
                        Value = jsonMessage
                    },
                    cancellationToken);

                _logger.LogInformation($"Delivered message to {deliveryResult.Value}, Offset: {deliveryResult.Offset}");
            }
            catch (ProduceException<Null, string> e)
            {
                _logger.LogError($"Delivery failed: {e.Error.Reason}");
            }
        }

        public void Dispose()
        {
            _producer.Flush(TimeSpan.FromSeconds(10));
            _producer.Dispose();
        }
    }
}
