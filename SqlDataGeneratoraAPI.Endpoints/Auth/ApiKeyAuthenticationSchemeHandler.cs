using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

public class ApiKeyAuthenticationSchemeHandler : AuthenticationHandler<ApiKeyAuthenticationSchemeOptions>
{
    public ApiKeyAuthenticationSchemeHandler(IOptionsMonitor<ApiKeyAuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // Extract the API key from the request header
        var apiKey = Context.Request.Headers["X-API-KEY"];

        

        if (apiKey != Options.ApiKey || string.IsNullOrEmpty(apiKey))
        {
            // Handle failed authentication and set response
            Context.Response.StatusCode = 401;
            await Context.Response.WriteAsJsonAsync(new { Message = "API Key is missing or invalid check our documentation for the instructions on how to use the API" });
            return AuthenticateResult.Fail("Invalid or missing X-API-KEY");
        }

        // If valid, create claims for the user
        var claims = new[] { new Claim(ClaimTypes.Name, "") };
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        // Return successful authentication
        return AuthenticateResult.Success(ticket);
    }
}
