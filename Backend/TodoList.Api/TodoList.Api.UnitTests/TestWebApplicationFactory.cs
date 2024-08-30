using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TodoList.Infrastructure;

namespace TodoList.Api.UnitTests
{
    public class TestWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<TodoContext>));
                services.AddDbContext<TodoContext>(opt => opt.UseInMemoryDatabase("TestDB"));

                var serviceProvider = services.BuildServiceProvider();
                var scope = serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<TodoContext>();
                
                //seed data 2 records

                dbContext.TodoItems.AddRange(TestHelper.SeedData());
                dbContext.SaveChanges();

            });
        }
    }
}
