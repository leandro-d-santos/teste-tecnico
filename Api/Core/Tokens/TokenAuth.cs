using Domain.Tokens.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Domain.Tokens.Core
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public sealed class TokenAuth : Attribute, IAuthorizationFilter
    {
        private readonly IServiceProvider _serviceProvider;

        public TokenAuth(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            var token = authorizationHeader.ToString().Split(" ")[1];
            if (string.IsNullOrEmpty(token))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            var tokenValidation = (ITokenValidationRepository?)_serviceProvider.GetService(typeof(ITokenValidationRepository));
            if (tokenValidation is null || !tokenValidation.IsValid(token))
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}