using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;

namespace Jazz.Covenant.ServiceHost.Configuration
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services = services.AddEndpointsApiExplorer()
                           .AddSwaggerGen(c =>
                           {
                               c.SchemaFilter<EnumSchemaFilter>();
                           });

            return services.Configure<JsonOptions>(options =>
            {
                options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
        }

        public static WebApplication UseSwagger(this WebApplication app,
                                                     string name)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", name);
            });
            return app;
        }
        public class EnumSchemaFilter : ISchemaFilter
        {
            public void Apply(OpenApiSchema model, SchemaFilterContext context)
            {
                if (context.Type.IsEnum)
                {
                    model.Enum.Clear();
                    foreach (string enumName in Enum.GetNames(context.Type))
                    {
                        System.Reflection.MemberInfo memberInfo = context.Type.GetMember(enumName).FirstOrDefault(m => m.DeclaringType == context.Type);
                        EnumMemberAttribute enumMemberAttribute = memberInfo == null
                         ? null
                         : memberInfo.GetCustomAttributes(typeof(EnumMemberAttribute), false).OfType<EnumMemberAttribute>().FirstOrDefault();
                        string label = enumMemberAttribute == null || string.IsNullOrWhiteSpace(enumMemberAttribute.Value)
                         ? enumName
                         : enumMemberAttribute.Value;
                        model.Enum.Add(new OpenApiString(label));
                    }
                }
            }
        }
    }
}
