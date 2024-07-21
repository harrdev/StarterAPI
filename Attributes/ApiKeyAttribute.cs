using Microsoft.AspNetCore.Mvc;
using StarterAPI.Filters;

namespace StarterAPI.Attributes
{
    public class ApiKeyAttribute : ServiceFilterAttribute
    {
        public ApiKeyAttribute() : base(typeof(ApiKeyAuthorizationFilter)) { }
    }
}
