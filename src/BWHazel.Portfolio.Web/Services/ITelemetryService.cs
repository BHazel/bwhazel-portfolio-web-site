using System.Threading.Tasks;

namespace BWHazel.Portfolio.Web.Services;

/// <summary>
/// Defines methods for sending telemetry data.
/// </summary>
public interface ITelemetryService
{
    /// <summary>
    /// Sends a telemetry event.
    /// </summary>
    /// <param name="eventName">The event name.</param>
    /// <param name="category">The event category.</param>
    /// <param name="pageUri">The source page URI.</param>
    /// <returns>A task representing any asynchronous operation.</returns>
    Task SendEvent(string eventName, string category, string pageUri);
    
    /// <summary>
    /// Sends a telemetry exception.
    /// </summary>
    /// <param name="exceptionName">The exception name.</param>
    /// <param name="exceptionMessage">The exception message.</param>
    /// <param name="pageUri">The source page URI.</param>
    /// <returns>A task representing any asynchronous operation.</returns>
    Task SendException(string exceptionName, string exceptionMessage, string pageUri);
}