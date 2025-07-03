// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared.Extensions;
using Shared.Interfaces;
using Shared.WikiStreamService;


var builder = Host.CreateApplicationBuilder();

builder.Services.AddTransient<IWikiStreamService, WikiMediaStreamService>();
builder.Services.AddHttpClient<IWikiStreamService, WikiMediaStreamService>(client =>
{
    client.BaseAddress = new Uri("https://stream.wikimedia.org");
}).AddPolicyHandler(RetryPolicies.GetRetryPolicy());

// Build the host
var host = builder.Build();

// Get the service provider
var serviceProvider = host.Services;

// Get the required service
var wikiStreamService = serviceProvider.GetRequiredService<IWikiStreamService>();
var test = await wikiStreamService.GetWikiStreamsAsync();




