using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shared.Interfaces;
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
        private readonly IMessageConsumer _consumer;

        public EventConsumerJob(IMessageConsumer consumer, ILogger<EventConsumerJob> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(consumer));
            _consumer = consumer ?? throw new ArgumentNullException(nameof(consumer));
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"The {nameof(EventConsumerJob)} has started executing...");

            _consumer.Consume(stoppingToken);

            _logger.LogInformation($"The {nameof(EventConsumerJob)} has finished executing...");

            return Task.CompletedTask;
        }
    }
}
