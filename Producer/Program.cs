using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared.Extensions;
using Shared.Interfaces;
using Producer.WikiStreamService;
using Producer.KafkaProducer;
using Shared.POCO;
using Producer.Jobs;

using var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        // Bind KafkaOptions from config
        services.Configure<KafkaProducerOptions>(context.Configuration.GetSection("KafkaProducer"));
        // Register KafkaProducer as a singleton
        services.AddSingleton(typeof(IMessageProducer<>), typeof(KafkaProducer<>));
        // Other registered services
        services.AddSingleton<IWikiStreamService, WikiMediaStreamService>();
        services.AddHttpClient<IWikiStreamService, WikiMediaStreamService>(client =>
        {
            client.BaseAddress = new Uri("https://stream.wikimedia.org");
        }).AddPolicyHandler(RetryPolicies.GetRetryPolicy());

        // Register the background job
        services.AddHostedService<EventProducerJob>(); 

    })
    .Build();

await host.RunAsync();