using Temporalio.Activities;
using TemporalDemo.Models;
using Timer = System.Timers.Timer;

namespace TemporalDemo.Activities;

public class Activity1
{
    [Activity(nameof(Activity1))]
    public static async Task<Activity1Output> Run(Activity1Input input)
    {
        // Heartbeat every 2s
        using var timer = new Timer(2000)
        {
            AutoReset = true,
            Enabled = true
        };
        timer.Elapsed += (_, _) => ActivityExecutionContext.Current.Heartbeat();

        var delay = Random.Shared.Next(0, 1000);
        Console.WriteLine("Delay will be "+delay);
        await Task.Delay(delay);
        Console.WriteLine("Delayed "+delay);
        return new Activity1Output { Baz = "Done @ " + DateTimeOffset.UtcNow.ToString("O") };
    }
}