namespace Tech.Challenge.Configuration;

public static class EndPoints
{
    public static IApplicationBuilder ConfigureEndPoints(this IApplicationBuilder app)
    {
        // app.UseHttpsRedirection();

        app.UseRouting();

        app.UseCors();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
        
        return app;
    }
}
