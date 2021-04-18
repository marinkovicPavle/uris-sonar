using LoggingClassLibrary;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TheSocialBaz.Data;
using TheSocialBaz.Data.PostMock;
using TheSocialBaz.FakeLoggerService;
//using TheSocialBaz.FakeLoggerService;

namespace TheSocialBaz
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
            services.AddControllers();

            services.AddScoped<ICommentingRepository, CommentingRepository>();
            services.AddScoped<IPostMockRepository, PostMockRepository>();

            services.AddSingleton<Logger, FakeLogger>();
            services.AddSingleton<ILogger, FakeLogger>();

            services.AddHttpContextAccessor();

            services.AddDbContext<DBContext>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddSwaggerGen(setupAction =>
            {
                setupAction.SwaggerDoc("CommentingApiSpecification",
                     new Microsoft.OpenApi.Models.OpenApiInfo()
                     {
                         Title = "Comments API",
                         Version = "1.0",
                         Description = "With this API you can list all coments, all coments on some post, one comment, add new comment, update and delete comments that exists",
                         Contact = new Microsoft.OpenApi.Models.OpenApiContact
                         {
                             Name = "Pavle Marinkovic",
                             Email = "pavle019@live.com",
                             Url = new Uri("https://pavlemarinkovic.com")
                         },
                         License = new Microsoft.OpenApi.Models.OpenApiLicense
                         {
                             Name = "FTN"
                         }
                     });

                var xmlComments = $"{Assembly.GetExecutingAssembly().GetName().Name }.xml";
                var xmlCommentsPath = Path.Combine(AppContext.BaseDirectory, xmlComments);

                setupAction.IncludeXmlComments(xmlCommentsPath); //da bi swagger mogao citati xml komenatare
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            } 
            else
            {
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("Unespected error, please try later!");
                    });
                });
            }

            app.UseSwagger();

            app.UseSwaggerUI(setupAction => {
                setupAction.SwaggerEndpoint("/swagger/CommentingApiSpecification/swagger.json", "Commenting API");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
