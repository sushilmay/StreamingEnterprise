using Prometheus;

public static class MetricsService
{
    private static readonly Counter RequestCounter =
        Metrics.CreateCounter(
            "myapp_requests_total",
            "Total number of requests");

    private static readonly Histogram RequestDuration =
        Metrics.CreateHistogram(
            "myapp_request_duration_seconds",
            "Request duration in seconds");

    public static void IncrementRequest()
    {
        RequestCounter.Inc();
    }

    public static IDisposable MeasureDuration()
    {
        return RequestDuration.NewTimer();
    }
}