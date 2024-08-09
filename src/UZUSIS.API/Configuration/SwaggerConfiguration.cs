namespace UZUSIS.API.Configuration;

public static class SwaggerConfiguration
{
    public static void ConfigurarSwagger(this IServiceCollection services)
    {

        services.Configure<RouteOptions>(options =>
        {
            options.LowercaseUrls = true;
            options.LowercaseQueryStrings = true;
        });



    }

}