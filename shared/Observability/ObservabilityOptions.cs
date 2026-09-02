namespace Shared.Observability;

public sealed class ObservabilityOptions
{
    public string ServiceNamespace { get; set; } = "service-template";
    public string OtlpEndpoint { get; set; } = "http://localhost:4317";
    public string DefaultLogLevel { get; set; } = "Information";
    public string FrameworkLogLevel { get; set; } = "Warning";
    public bool EnableAspNetCoreTracing { get; set; } = true;
    public bool EnableHttpClientTracing { get; set; } = true;
    public bool EnableEntityFrameworkCoreTracing { get; set; } = true;
}

