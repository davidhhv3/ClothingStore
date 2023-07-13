using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ClothingStore.Infrastructure
{
    public class AuthOperationFilter : IOperationFilter
    {
        private bool hasAuthorize;
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {    
            if(context.MethodInfo.DeclaringType != null) 
                hasAuthorize = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
                    .Union(context.MethodInfo.GetCustomAttributes(true))
                    .OfType<AuthorizeAttribute>().Any();            

            if (hasAuthorize)
            {
                operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new OpenApiSecurityRequirement
                    {
                        [
                            new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } }
                        ] = new string[] { }
                    }
                };
            }
        }
    }
}
