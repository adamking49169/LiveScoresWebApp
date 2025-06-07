using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.RateLimiting;

public class RateLimitedHandler : DelegatingHandler
{
    private readonly RateLimiter _limiter;

    public RateLimitedHandler(int permitLimit, TimeSpan window)
    {
        _limiter = new TokenBucketRateLimiter(new TokenBucketRateLimiterOptions
        {
            TokenLimit = permitLimit,
            TokensPerPeriod = permitLimit,
            ReplenishmentPeriod = window,
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            QueueLimit = int.MaxValue,
            AutoReplenishment = true
        });
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        using var lease = await _limiter.AcquireAsync(1, cancellationToken);
        if (!lease.IsAcquired)
        {
            throw new HttpRequestException("Rate limit exceeded");
        }

        return await base.SendAsync(request, cancellationToken);
    }
}