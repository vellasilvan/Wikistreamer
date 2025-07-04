using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consumer.Jobs
{
    internal class EventConsumerJob : BackgroundService
    {
        private readonly ILogger<EventConsumerJob> _logger;

        public EventConsumerJob(ILogger<EventConsumerJob> logger)
        {
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "test-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();

            consumer.Subscribe("WikiMedia");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = consumer.Consume(TimeSpan.FromSeconds(5));

                    if (consumeResult == null)
                    {
                        continue;
                    }

                    _logger.LogInformation($"Consumed message '{consumeResult.Message.Value}' at: '{consumeResult.Offset}'");
                }
                catch (Exception)
                {
                    // Ignore
                }
            }


            return Task.CompletedTask;
        }
    }
}
