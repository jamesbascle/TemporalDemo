using TemporalDemo.Models;
using Temporalio.Exceptions;
using Temporalio.Workflows;

namespace TemporalDemo.Workflows;

[Workflow(nameof(Workflow1))]
public class Workflow1
{
    [WorkflowRun]
    public async Task<Workflow1Output> Run(Workflow1Input input)
    {
        var activity1  =
            Workflow.ExecuteActivityAsync<Activity1Output>(
                "Activity1",
                    new object?[] { new Activity1Input { Qux = $"Qux Starting w/ input {input.Foo} @ " + DateTimeOffset.UtcNow.ToString("O") } },
                    new ActivityOptions{StartToCloseTimeout = TimeSpan.FromSeconds(60)});
        
        var timeout = Workflow.DelayAsync(TimeSpan.FromMilliseconds(1000));
        
        await Workflow.WhenAnyAsync(activity1, timeout);
        if (!activity1.IsCompletedSuccessfully)
        {
            throw new ApplicationFailureException("Timeout Occurred when Executing Activity1");
        }
        
        return new Workflow1Output { Bar = "Done @ " + DateTimeOffset.UtcNow.ToString("O") };
    }
}