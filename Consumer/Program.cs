// See https://aka.ms/new-console-template for more information
using Consumer.Jobs;
using Consumer.KafkaConsumer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared.Extensions;
using Shared.Interfaces;
using Shared.POCO;


Console.WriteLine("Start consuming events ...");

using var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.Configure<KafkaConsumerOptions>(context.Configuration.GetSection("KafkaConsumer"));
        services.AddSingleton<IMessageConsumer, KafkaConsumer>();
        services.AddHostedService<EventConsumerJob>();
    })
    .Build();

await host.RunAsync();

