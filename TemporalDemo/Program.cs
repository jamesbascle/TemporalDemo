// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TemporalDemo.Activities;
using TemporalDemo.Workflows;
using Temporalio.Client;
using Temporalio.Extensions.Hosting;

using var tokenSource = new CancellationTokenSource();
Console.CancelKeyPress += (_, eventArgs) =>
{
    tokenSource.Cancel();
    eventArgs.Cancel = true;
};

var builder = Host.CreateApplicationBuilder();

builder.Services
    .AddHostedTemporalWorker("localhost:7233", 
        "default", "my-task-queue"
        )
    .AddScopedActivities<Activity1>()
    .AddWorkflow<Workflow1>();

var host = builder.Build();
host.Run();