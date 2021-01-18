using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            using (var db = new SQLiteContext())
            {
                db.Database.EnsureCreatedAsync();
                db.SaveChangesAsync();
            }

        }

        public IConfiguration Configuration { get; }

        // here we add services for later use in our API.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContext<Context>(opt => opt.UseSqlServer
            //(Configuration.GetConnectionString("APIconnection")));
            services.AddScoped<IPoczekalnia, SerwisPoczekalni>();
            services.AddDbContext<SQLiteContext>();
            services.AddDbContext<ConfigContext>();
            //services.AddDbContext<SQLiteContext>(opt => opt.UseSqlite
            //("Data Source=SQLitePlayerBase.db"));

            //($"Data Source={Configuration. .ApplicationBasePath}/data.db"));

            services.AddControllers().AddNewtonsoftJson
                (s => s.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver());

            services.AddAutoMapper
                (AppDomain.CurrentDomain.GetAssemblies());
            
            //services.AddScoped<Ireposit, SQLrepositorium>();
            services.AddScoped<ILiteRepo, SQLiteRepo>();
            services.AddRazorPages();
            //services.AddHttpContextAccessor();
        }

        // Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //here we add midleware (order maters) 
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //dataContext.Database.Migrate();
          //  app.UseForwardedHeaders(new ForwardedHeadersOptions
          //  {
          //      ForwardedHeaders = ForwardedHeaders.XForwardedFor |
          //ForwardedHeaders.XForwardedProto
          //  });
            
            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
        }
    }
}
