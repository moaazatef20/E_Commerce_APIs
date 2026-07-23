using E_Commerce.Application.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;

namespace E_Commerce.API.Attributes
{
    public class RedisCasheAttribute : ActionFilterAttribute
    {
        private readonly int _cacheDuration;

        public RedisCasheAttribute(int cacheDuration = 2)
        {
            _cacheDuration = cacheDuration;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var casheService = context.HttpContext.RequestServices.GetRequiredService<ICasheService>();
            var casheKey = GenerateCasheKey(context.HttpContext.Request);
            var data =  await casheService.GetDataAsync(casheKey);
            if(!string.IsNullOrEmpty(data))
            {
                context.Result = new ContentResult
                {
                    Content = data,
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK
                };
                return;
            }

            var actionResult = await next.Invoke();
            if(actionResult.Result is OkObjectResult { Value : not null} ok)
            {
                await casheService.SetDataAsync(casheKey, ok.Value, TimeSpan.FromDays(_cacheDuration));
            }
        }

       
        private static string GenerateCasheKey(HttpRequest request)
        {
            var keyBuilder = new StringBuilder();
            keyBuilder.Append($"{request.Path}");
            if(request.Query.Any())
            {
                foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
                {
                    keyBuilder.Append($"?{key}={value}&");
                }
            }
            return keyBuilder.ToString();
        }
}
}
