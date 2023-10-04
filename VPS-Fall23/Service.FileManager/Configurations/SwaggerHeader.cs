using Microsoft.OpenApi.Models;
using VPS.Helper;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace VPS.MinIO.API.Configurations
{
    internal class SwaggerHeader : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters ??= new List<OpenApiParameter>();
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = Constant.MINIO_ACCESS_KEY_HEADER_NAME,
                In = ParameterLocation.Header,
                Required = true
            });
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = Constant.MINIO_SECRET_KEY_HEADER_NAME,
                In = ParameterLocation.Header,
                Required = true
            });


        }
    }
}
