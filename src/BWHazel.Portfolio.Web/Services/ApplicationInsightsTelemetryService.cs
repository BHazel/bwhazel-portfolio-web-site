using System;
using System.Threading.Tasks;
using BlazorApplicationInsights.Interfaces;
using BlazorApplicationInsights.Models;

namespace BWHazel.Portfolio.Web.Services;

/// <summary>
/// Sends telemetry data to Azure Application Insights.
/// </summary>
/// <param name="applicationInsights">The Application Insights service.</param>
public class ApplicationInsightsTelemetryService(IApplicationInsights applicationInsights)
    : ITelemetryService
{
    private readonly IApplicationInsights applicationInsights = applicationInsights;

    /// <summary>
    /// Sends a telemetry event to Azure Application Insights.
    /// </summary>
    /// <param name="eventName">The event name.</param>
    /// <param name="category">The event category.</param>
    /// <param name="pageUri">The source page URI.</param>
    /// <returns>A task representing any asynchronous operation.</returns>
    public async Task SendEvent(string eventName, string category, string pageUri)
    {
        if (string.IsNullOrWhiteSpace(eventName))
        {
            throw new ArgumentNullException(nameof(eventName));
        }
        
        if (string.IsNullOrWhiteSpace(category))
        {
            throw new ArgumentNullException(nameof(category));
        }
        
        if (string.IsNullOrWhiteSpace(pageUri))
        {
            throw new ArgumentNullException(nameof(pageUri));
        }
        
        EventTelemetry eventTelemetry = new()
        {
            Name = eventName,
            Properties = new()
            {
                ["Category"] = category,
                ["PageUri"] = pageUri
            }
        };
        
        await this.applicationInsights.TrackEvent(eventTelemetry);
    }
}