// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared.Extensions;
using Shared.Interfaces;
using Producer.WikiStreamService;
using Producer.KafkaProducer;
using Shared.POCO;

using var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        // Bind KafkaOptions from config
        services.Configure<KafkaOptions>(context.Configuration.GetSection("Kafka"));
        // Register KafkaProducer as a singleton
        services.AddSingleton(typeof(IMessageProducer<>), typeof(KafkaProducer<>));
        // Your other services...
        services.AddTransient<IWikiStreamService, WikiMediaStreamService>();
        services.AddHttpClient<IWikiStreamService, WikiMediaStreamService>(client =>
        {
            client.BaseAddress = new Uri("https://stream.wikimedia.org");
        }).AddPolicyHandler(RetryPolicies.GetRetryPolicy());
        
    })
    .Build();

// Get the service provider
var serviceProvider = host.Services;

// Get the required service
var wikiStreamService = serviceProvider.GetRequiredService<IWikiStreamService>();
await wikiStreamService.GetWikiStreamsAsync();




