using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace TeamChat.MiddleWares
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token =
                context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()
                ?? context.Request.Query["access_token"];

            if (token == null)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Not authorized!");
                return;
            }

            try
            {
                HttpClient httpClient = new HttpClient();
                string apiUrl = _configuration["JWK_URL"] ?? "";

                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                string jwksJson = await response.Content.ReadAsStringAsync();

                var handler = new JwtSecurityTokenHandler();

                var jwks = new JsonWebKeySet(jwksJson);
                var jwk = jwks.Keys.First();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = jwk,
                    ValidAlgorithms = new[] { "RS256" },
                    ValidateLifetime = false,
                    ValidateAudience = false,
                    ValidateIssuer = false
                };
                var claimsPrincipal = handler.ValidateToken(
                    token,
                    validationParameters,
                    out var validatedToken
                );

                string emailClaimValue =
                    claimsPrincipal
                        .Claims.FirstOrDefault(
                            c =>
                                c.Type
                                == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"
                        )
                        ?.Value ?? "";

                context.Items["token"] = token;
                context.Items["email"] = emailClaimValue;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsync("Not authorized!");
                return;
            }

            await _next(context);
        }
    }
}
