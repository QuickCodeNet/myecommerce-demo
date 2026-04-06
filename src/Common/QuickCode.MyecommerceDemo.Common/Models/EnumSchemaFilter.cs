using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace QuickCode.MyecommerceDemo.Common.Models
{
    public class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type.IsEnum)
            {
                var enumNames = new OpenApiArray();
                var enumDescriptions = new OpenApiArray();
                
                var enumValues = Enum.GetValues(context.Type);
                var enumFields = context.Type.GetFields(BindingFlags.Public | BindingFlags.Static);
                
                foreach (var enumValue in enumValues)
                {
                    var enumName = Enum.GetName(context.Type, enumValue);
                    enumNames.Add(new OpenApiString(enumName));
                    
                    var fieldInfo = enumFields.FirstOrDefault(f => f.Name == enumName);
                    var descriptionAttribute = fieldInfo?.GetCustomAttribute<DescriptionAttribute>();
                    var description = descriptionAttribute?.Description ?? enumName;
                    enumDescriptions.Add(new OpenApiString(description));
                }

                schema.Extensions.Add("x-enumNames", enumNames);
                schema.Extensions.Add("x-enum-varnames", enumNames);
                schema.Extensions.Add("x-enumDescriptions", enumDescriptions);
            }
        }
    }
}

