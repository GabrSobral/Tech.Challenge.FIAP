using Tech.Challenge.Middlewares;

namespace Tech.Challenge.Configuration;

public static class Middlewares
{
    public static IApplicationBuilder ConfigureMiddlewares(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlerMiddleware>();

        return app;
    }
}
