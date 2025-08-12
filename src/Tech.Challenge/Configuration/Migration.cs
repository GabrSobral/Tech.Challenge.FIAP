using Microsoft.EntityFrameworkCore;
using Tech.Challenge.Infra.Database.Contexts;

namespace Tech.Challenge.Configuration;

public static class Migration
{
    public static IApplicationBuilder ConfigureMigrations(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope();

        var context = serviceScope?.ServiceProvider.GetRequiredService<DataContext>();

        if (context != null)
        {
            try
            {
                var pendingMigrations = context.Database.GetPendingMigrations();
                if (pendingMigrations != null && pendingMigrations.Any())
                {
                    context.Database.Migrate();
                    
                    var seeder = new SeedData(context);

                    seeder.SeedProdutos();
                    seeder.SeedServicos();
                    seeder.SeedUsuarios();
                    seeder.SeedClientes();
                    seeder.SeedVeiculos();

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        return app;
    }
}
