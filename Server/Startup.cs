using Client.BuildingBlocks.AzureSpeechRecognition;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Server.BuildingBlocks;
using Server.Data;
using Server.Hubs;
using Server.Services;
using Server.Workers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Server
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
            services.AddScoped<MessageStreamingService>();
            services.AddScoped<DateProvider>();
            
            services.AddSingleton<AllianceLoaderService>();
            services.AddSingleton<DataContextContainer>();
            services.AddSingleton<SkipCountProvider>();

            services.Configure<AzureSpeechAnalysisOptions>(options =>
            {
                //options.Endpoint = Configuration["AzureSpeechAnalysisEndpoint"];
                options.APIKey = Configuration["azure"];
            });
            services.AddHttpClient<AzureSpeechAnalysisAPIClient>(options =>
            {
                options.BaseAddress = new Uri("https://switzerlandnorth.api.cognitive.microsoft.com/sts/v1.0/");
            });

            services.AddHostedService<BackgroundDataWorker>();

            services.AddAutoMapper(typeof(Startup).Assembly);

            services.AddControllers()
                .AddJsonOptions(options => 
                {
                    //options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
                    //options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                });
            services.AddRazorPages();
            services.AddSignalR();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Server", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Server v1"));

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseBlazorFrameworkFiles();

            app.UseRouting();      

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapHub<NotificationHub>("/notificationhub");
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
