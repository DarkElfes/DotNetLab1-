using Library.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Primitives;
using System.IdentityModel.Tokens.Jwt;

namespace Bank.Handler;

public class RequestHandler(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue("Authorization", out StringValues authHeader))
        {
            JwtSecurityToken jwt = new(authHeader.ToString().Replace("Bearer ", ""));

            int userCardId = int.Parse(jwt.Claims.First(c => c.Type == "card_id").Value);

            context.Items["card_id"] = userCardId;
        }
        else if (context.GetRouteValue("controller")?.ToString() == "Card")
            return;

        await _next(context);
    }
}
