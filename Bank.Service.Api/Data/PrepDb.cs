using Bank.Service.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Bank.Service.Api.Data;

public static class PrepDb
{
    public static void PrepPopulation(IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();

        SeedData(serviceScope.ServiceProvider.GetService<DataBaseContext>());
    }

    private static void SeedData(DataBaseContext context)
    {
        context.Database.Migrate();

        context.SaveChanges();
    }
}