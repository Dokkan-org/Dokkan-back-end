using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Dokkan.Api.Presistence;

public class ApplicationDbContext:DbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
