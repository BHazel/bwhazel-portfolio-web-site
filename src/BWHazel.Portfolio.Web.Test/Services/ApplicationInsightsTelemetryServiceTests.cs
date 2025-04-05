using System;
using System.Threading.Tasks;
using BlazorApplicationInsights.Interfaces;
using BlazorApplicationInsights.Models;
using BWHazel.Portfolio.Web.Services;
using NSubstitute;
using Xunit;

namespace BWHazel.Portfolio.Web.Test.Services;

/// <summary>
/// Tests for the <see cref="ApplicationInsightsTelemetryService"/> class.
/// </summary>
public class ApplicationInsightsTelemetryServiceTests
{
    /// <summary>
    /// Tests that the <see cref="ApplicationInsightsTelemetryService.SendEvent"/> method sends an event when all values are provided.
    /// </summary>
    [Fact]
    public async Task SendEvent_WithAllValuesProvided_SendsEvent()
    {
        IApplicationInsights? applicationInsights = Substitute.For<IApplicationInsights>();
        ApplicationInsightsTelemetryService telemetryService = new(applicationInsights);
        string eventName = "TestEvent";
        string category = "TestCategory";
        string pageUri = "/Test/Page";

        await telemetryService.SendEvent(eventName, category, pageUri);

        EventTelemetry expectedEvent = new()
        {
            Name = eventName,
            Properties = new()
            {
                ["Category"] = category,
                ["PageUri"] = pageUri
            }
        };

        await applicationInsights.Received(1).TrackEvent(Arg.Is<EventTelemetry>(actualEvent =>
            actualEvent.Name == expectedEvent.Name &&
            actualEvent.Properties!["Category"] == expectedEvent.Properties["Category"] &&
            actualEvent.Properties!["PageUri"] == expectedEvent.Properties["PageUri"]));
    }
    
    /// <summary>
    /// Tests that the <see cref="ApplicationInsightsTelemetryService.SendEvent"/> method throws an exception when the event name is missing.
    /// </summary>
    [Fact]
    public async Task SendEvent_WithMissingEventName_ThrowsArgumentNullException()
    {
        IApplicationInsights? applicationInsights = Substitute.For<IApplicationInsights>();
        ApplicationInsightsTelemetryService telemetryService = new(applicationInsights);
        
        await Assert.ThrowsAsync<ArgumentNullException>(() => telemetryService.SendEvent(string.Empty, "TestCategory", "/Test/Page"));
    }
    
    /// <summary>
    /// Tests that the <see cref="ApplicationInsightsTelemetryService.SendEvent"/> method throws an exception when the category is missing.
    /// </summary>
    [Fact]
    public async Task SendEvent_WithMissingCategory_ThrowsArgumentNullException()
    {
        IApplicationInsights? applicationInsights = Substitute.For<IApplicationInsights>();
        ApplicationInsightsTelemetryService telemetryService = new(applicationInsights);
        
        await Assert.ThrowsAsync<ArgumentNullException>(() => telemetryService.SendEvent("TestEvent", string.Empty, "/Test/Page"));
    }
    
    /// <summary>
    /// Tests that the <see cref="ApplicationInsightsTelemetryService.SendEvent"/> method throws an exception when the page URI is missing.
    /// </summary>
    [Fact]
    public async Task SendEvent_WithMissingPageUri_ThrowsArgumentNullException()
    {
        IApplicationInsights? applicationInsights = Substitute.For<IApplicationInsights>();
        ApplicationInsightsTelemetryService telemetryService = new(applicationInsights);
        
        await Assert.ThrowsAsync<ArgumentNullException>(() => telemetryService.SendEvent("TestEvent", "TestCategory", string.Empty));
    }
}