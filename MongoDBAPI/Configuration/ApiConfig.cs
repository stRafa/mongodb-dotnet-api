using Microsoft.AspNetCore.Mvc;

namespace MongoDBAPI.Configuration
{
    public static class ApiConfig
    {
        public static void AddAPIConfig(this IServiceCollection services) 
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            services.AddControllers();
        }
    }
}
