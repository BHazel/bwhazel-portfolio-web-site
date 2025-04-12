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
    public async Task SendEvent_WithMissingEventName_ThrowsException()
    {
        IApplicationInsights? applicationInsights = Substitute.For<IApplicationInsights>();
        ApplicationInsightsTelemetryService telemetryService = new(applicationInsights);
        
        await Assert.ThrowsAsync<ArgumentNullException>(() => telemetryService.SendEvent(string.Empty, "TestCategory", "/Test/Page"));
    }
    
    /// <summary>
    /// Tests that the <see cref="ApplicationInsightsTelemetryService.SendEvent"/> method throws an exception when the category is missing.
    /// </summary>
    [Fact]
    public async Task SendEvent_WithMissingCategory_ThrowsException()
    {
        IApplicationInsights? applicationInsights = Substitute.For<IApplicationInsights>();
        ApplicationInsightsTelemetryService telemetryService = new(applicationInsights);
        
        await Assert.ThrowsAsync<ArgumentNullException>(() => telemetryService.SendEvent("TestEvent", string.Empty, "/Test/Page"));
    }
    
    /// <summary>
    /// Tests that the <see cref="ApplicationInsightsTelemetryService.SendEvent"/> method throws an exception when the page URI is missing.
    /// </summary>
    [Fact]
    public async Task SendEvent_WithMissingPageUri_ThrowsException()
    {
        IApplicationInsights? applicationInsights = Substitute.For<IApplicationInsights>();
        ApplicationInsightsTelemetryService telemetryService = new(applicationInsights);
        
        await Assert.ThrowsAsync<ArgumentNullException>(() => telemetryService.SendEvent("TestEvent", "TestCategory", string.Empty));
    }

    /// <summary>
    /// Tests that the <see cref="ApplicationInsightsTelemetryService.SendException"/> method sends an exception when all values are provided.
    /// </summary>
    [Fact]
    public async Task SendException_WithAllValuesProvided_SendsEvent()
    {
        IApplicationInsights? applicationInsights = Substitute.For<IApplicationInsights>();
        ApplicationInsightsTelemetryService telemetryService = new(applicationInsights);
        string exceptionName = "TestException";
        string exceptionMessage = "Test Exception Message";
        string pageUri = "/Test/Page";
        
        await telemetryService.SendException(exceptionName, exceptionMessage, pageUri);
        
        ExceptionTelemetry expectedException = new()
        {
            Exception = new()
            {
                Name = exceptionName,
                Message = exceptionMessage,
            },
            Properties = new()
            {
                ["PageUri"] = pageUri
            }
        };
        
        await applicationInsights.Received(1).TrackException(Arg.Is<ExceptionTelemetry>(actualException =>
            actualException.Exception!.Name == expectedException.Exception.Name &&
            actualException.Exception!.Message == expectedException.Exception.Message &&
            actualException.Properties!["PageUri"] == expectedException.Properties["PageUri"]));
    }
    
    /// <summary>
    /// Tests that the <see cref="ApplicationInsightsTelemetryService.SendException"/> method throws an exception when the exception name is missing.
    /// </summary>
    [Fact]
    public async Task SendException_WithMissingExceptionName_ThrowsException()
    {
        IApplicationInsights? applicationInsights = Substitute.For<IApplicationInsights>();
        ApplicationInsightsTelemetryService telemetryService = new(applicationInsights);
        
        await Assert.ThrowsAsync<ArgumentNullException>(() => telemetryService.SendException(string.Empty, "Test Exception Message", "/Test/Page"));
    }
    
    /// <summary>
    /// Tests that the <see cref="ApplicationInsightsTelemetryService.SendException"/> method throws an exception when the exception message is missing.
    /// </summary>
    [Fact]
    public async Task SendException_WithMissingExceptionMessage_ThrowsException()
    {
        IApplicationInsights? applicationInsights = Substitute.For<IApplicationInsights>();
        ApplicationInsightsTelemetryService telemetryService = new(applicationInsights);
        
        await Assert.ThrowsAsync<ArgumentNullException>(() => telemetryService.SendException("TestException", string.Empty, "/Test/Page"));
    }
    
    /// <summary>
    /// Tests that the <ApplicationInsightsTelemetryService.SendException/> method throws an exception when the page URI is missing.
    /// </summary>
    [Fact]
    public async Task SendException_WithMissingPageUri_ThrowsException()
    {
        IApplicationInsights? applicationInsights = Substitute.For<IApplicationInsights>();
        ApplicationInsightsTelemetryService telemetryService = new(applicationInsights);
        
        await Assert.ThrowsAsync<ArgumentNullException>(() => telemetryService.SendException("TestException", "Test Exception Message", string.Empty));
    }
}