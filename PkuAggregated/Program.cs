using System.Text;
using Microsoft.Extensions.DependencyInjection;
using PkuAggregated.Interfaces;
using PkuAggregated.Models;
using PkuAggregated.SearchSources;

namespace PkuAggregated
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var enableCors = bool.Parse(builder.Configuration["EnableCors"] ?? "false");

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHttpContextAccessor();

            if (enableCors)
                builder.Services.AddCors(options =>
                {
                    options.AddPolicy(
                        "_defaultCorsPolicy",
                        policy =>
                        {
                            policy
                                .WithOrigins(
                                    builder.Configuration["FrontendUrl"]
                                        ?? throw new Exception("No FrontendUrl provided.")
                                )
                                .AllowAnyMethod()
                                .AllowAnyHeader();
                        }
                    );
                });

            builder.Services.AddSingleton<Treehole>();
            builder.Services.AddSingleton<PortalApps>();
            builder.Services.AddSingleton<ISearchSource, PortalApps>(
                (provider) => provider.GetService<PortalApps>()!
            );
            builder.Services.AddSingleton<ISearchSource, CourseReview>();
            builder.Services.AddSingleton<ISearchSource, PortalDepartmentNotices>();
            builder.Services.AddSingleton<ISearchSource, PortalSchoolNotices>();

            Params.TokenGeneratorSource =
                builder.Configuration["TokenGeneratorSource"]
                ?? throw new Exception("No TokenGeneratorSource provided.");
            Params.Username =
                builder.Configuration["AccountId"] ?? throw new Exception("No Username provided.");
            Params.Password =
                builder.Configuration["Password"] ?? throw new Exception("No Password provided.");

            var app = builder.Build();

            if (enableCors)
                app.UseCors();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            if (enableCors)
                app.MapControllers().RequireCors("_defaultCorsPolicy");
            else
                app.MapControllers();

            if (!app.Environment.IsDevelopment())
                app.Use(
                    async (context, next) =>
                    {
                        var receivedToken = context.Request.Headers["X-Private-Verification"];
                        if (receivedToken != Utils.GenerateVerificationToken())
                        {
                            context.Response.StatusCode = 403;
                            await context.Response.Body.WriteAsync(
                                Encoding.UTF8.GetBytes(
                                    "X-Private-Verification token is invalid. Contact server administrator."
                                )
                            );
                            return;
                        }
                        await next();
                    }
                );

            app.Run();
        }
    }
}
