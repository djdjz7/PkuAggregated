using System.Text;
using Microsoft.Extensions.DependencyInjection;
using PkuAggregated.Interfaces;
using PkuAggregated.Models;
using PkuAggregated.SeachSources;
using PkuAggregated.SearchSources;

namespace PkuAggregated
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration.AddJsonFile(
                "usersettings.json",
                optional: false,
                reloadOnChange: true
            );
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

            Params.TokenGeneratorSource =
                builder.Configuration["TokenGeneratorSource"]
                ?? throw new Exception("No TokenGeneratorSource provided.");
            Params.Username =
                builder.Configuration["AccountId"] ?? throw new Exception("No AccountId provided.");
            Params.Password =
                builder.Configuration["Password"] ?? throw new Exception("No Password provided.");
            Params.BbsUsername = builder.Configuration["BbsUsername"];
            Params.BbsPassword = builder.Configuration["BbsPassword"];

            builder.Services.AddSingleton<Treehole>();
            if (
                !string.IsNullOrEmpty(Params.BbsUsername)
                && !string.IsNullOrEmpty(Params.BbsPassword)
            )
                builder.Services.AddSingleton<ISearchSource, Bbs>();
            // builder.Services.AddSingleton<PortalApps>();
            // builder.Services.AddSingleton<ISearchSource, PortalApps>(
            //     (provider) => provider.GetService<PortalApps>()!
            // );
            builder.Services.AddSingleton<ISearchSource, CourseReview>();
            builder.Services.AddSingleton<ISearchSource, PortalDepartmentNotices>();
            builder.Services.AddSingleton<ISearchSource, PortalSchoolNotices>();

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
                        bool result = true;
                        var receivedToken = context.Request.Headers["X-Private-Verification"];
                        var receivedRequestTime = context.Request.Headers[
                            "X-Private-Token-Gen-Time"
                        ];
                        if (
                            string.IsNullOrEmpty(receivedToken)
                            || string.IsNullOrEmpty(receivedRequestTime)
                        )
                            result = false;
                        else
                        {
                            try
                            {
                                var tokenGenTime = DateTime.Parse(receivedRequestTime!);
                                result = Utils.VerifyToken(receivedToken!, tokenGenTime);
                            }
                            catch
                            {
                                result = false;
                            }
                        }
                        if (!result)
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
