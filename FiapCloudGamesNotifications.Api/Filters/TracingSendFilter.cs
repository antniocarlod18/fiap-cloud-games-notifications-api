using Elastic.Apm;
using MassTransit;
using System.Diagnostics;
using System.Threading.Channels;

namespace FiapCloudGamesNotifications.Api.Filters;

public class TracingSendFilter<T> : IFilter<SendContext<T>> where T : class
{
    public async Task Send(SendContext<T> context, IPipe<SendContext<T>> next)
    {
        var tracer = Agent.Tracer;
        var currentTransaction = tracer.CurrentTransaction;

        if (currentTransaction != null)
        {
            context.Headers.Set("trace-id", currentTransaction.TraceId);
        }

        await next.Send(context);
    }

    public void Probe(ProbeContext context)
    {
        context.CreateFilterScope("TracingSendFilter");
    }
}
