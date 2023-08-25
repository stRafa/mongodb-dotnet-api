using MongoDBAPI.Services;

namespace MongoDBAPI.Configuration
{
    public static class ServicesConfig
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddSingleton<BooksService>();
            services.AddScoped<UsersService>();
            services.AddHttpClient<UsersService>();
            services.AddScoped<YtConvertService>();
        }
    }
}
