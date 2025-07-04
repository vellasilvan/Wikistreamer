using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shared.DTOs;
using Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Producer.Jobs
{
    public class EventProducerJob : BackgroundService
    {

        private readonly ILogger<EventProducerJob> _logger;
        private readonly IWikiStreamService _wikiStreamService;
        public EventProducerJob(IWikiStreamService wikiStreamService, ILogger<EventProducerJob> logger)
        {
            _wikiStreamService = wikiStreamService ?? throw new ArgumentNullException(nameof(wikiStreamService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"The {nameof(EventProducerJob)} has started executing...");
            await _wikiStreamService.GetWikiStreamsAsync(stoppingToken);
            _logger.LogInformation($"The {nameof(EventProducerJob)} has finished executing...");

        }
    }
}
