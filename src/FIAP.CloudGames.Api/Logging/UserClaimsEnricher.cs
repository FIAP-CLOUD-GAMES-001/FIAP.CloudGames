using Serilog.Core;
using Serilog.Events;
using System.Security.Claims;

namespace FIAP.CloudGames.Api.Logging;
public class UserClaimsEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var httpContext = HttpContextAccessorWrapper.HttpContext;

        if (httpContext?.User?.Identity?.IsAuthenticated != true)
            return;

        var user = httpContext.User;

        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "anonymous";
        var role = user.FindFirst(ClaimTypes.Role)?.Value ?? "none";

        logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("UserId", userId));
        logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("Role", role));
    }
}