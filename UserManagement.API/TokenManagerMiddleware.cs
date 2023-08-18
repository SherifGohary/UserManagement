using Microsoft.AspNetCore.Http;
using System.Net;
using System.Threading.Tasks;

namespace UserManagement.API
{
    public class TokenManagerMiddleware : IMiddleware
    {
        private readonly TokenManager _tokenManager;

        public TokenManagerMiddleware(TokenManager tokenManager)
        {
            _tokenManager = tokenManager;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            string authorizationHeader = context.Request.Headers["authorization"];

            if (string.IsNullOrEmpty(authorizationHeader) || await _tokenManager.IsCurrentActiveToken())
            {
                await next(context);

                return;
            }
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }
    }
}
