using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Payoneer.DotnetCore.Domain;
using Payoneer.DotnetCore.Rds;
using Payoneer.DotnetCore.Repository;
using Payoneer.DotnetCore.Service.External;
using System.IO.Compression;

namespace Payoneer.DotnetCore.WebApi.External
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("ApplicationContext");
            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connectionString));

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc; ;
                });

            services.AddScoped<IDbFactory, DbFactory>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IRepository<Payment>, Repository<Payment>>();
            services.AddTransient<IPaymentServiceExternal, PaymentServiceExternal>();
            services.AddTransient<IPaymentValidator, PaymentValidator>();
            
            services.AddCors();

            services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);
            services.AddResponseCompression(options => options.Providers.Add<GzipCompressionProvider>());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseResponseCompression();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder =>
                builder.WithOrigins("http://localhost:5000"));

            app.UseMvc();

            app.ApplicationServices.CreateScope().ServiceProvider.GetService<ApplicationContext>().Database.EnsureCreated();
        }
    }
}
