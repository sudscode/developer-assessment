using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using TodoList.Api.Mappers;
using TodoList.Application;
using TodoList.Infrastructure;

namespace TodoList.Api
{
    public class Startup
    {

        public const string DbNameConfigKey = "DbName";
        public const string AllowOriginConfigKey = "AllowedOrigin";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var allowOriginConfig = Configuration.GetSection(AllowOriginConfigKey) ?? throw new ArgumentNullException($"Key not found - '{AllowOriginConfigKey}'");
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllHeaders",
                      builder =>
                      {
                          //Restrict the access to prevent potential attacks
                          
                          builder.WithOrigins(allowOriginConfig.Value) //AllowAnyOrigin()
                                 .AllowAnyHeader()
                                 .AllowAnyMethod();
                      });
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TodoList.Api", Version = "v1" });
            });

            //Alternatively  options pattern can be used for binding (overkill in this instance)

            var dbConfig = Configuration.GetSection(DbNameConfigKey) ?? throw new ArgumentNullException($"Key not found - '{DbNameConfigKey}'");

            services.AddDbContext<TodoContext>(opt => opt.UseInMemoryDatabase(dbConfig.Value));
            services.AddScoped<ITodoListRepository, TodoListRepository>();
            services.AddScoped<IMapper, Mapper>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TodoList.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("AllowAllHeaders");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
