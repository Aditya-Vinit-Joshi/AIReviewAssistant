using AIReviewAssistant.Interfaces.AI;
using AIReviewAssistant.Interfaces.ApiService;
using AIReviewAssistant.Interfaces.DatabaseServices;
using AIReviewAssistant.Interfaces.ErrorLogging;
using AIReviewAssistant.Interfaces.GitHub;
using AIReviewAssistant.Services.AIServices;
using AIReviewAssistant.Services.APIServices;
using AIReviewAssistant.Services.DatabaseServices;
using AIReviewAssistant.Services.GitHub;
using AIReviewAssistant.Services.LoggingServices;
using AIReviewAssistant.Settings;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Driver;

namespace AIReviewAssistant
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            var mongoSettingsSection = builder.Configuration.GetSection("MongoDbSettings");
            builder.Services.Configure<MongoDbSettings>(mongoSettingsSection);

            builder.Services.AddSingleton<IMongoClient>(sp =>
            {
                var settings = mongoSettingsSection.Get<MongoDbSettings>();
                return new MongoClient(settings?.ConnectionString);
            });

            builder.Services.AddScoped<IMongoDbReviewService,MongoDbReviewService>();
            builder.Services.AddScoped<IErrorLoggingMongoService,ErrorLoggingMongoService>();
            builder.Services.AddScoped<IHttpHandler, HttpHandlerService>();
            builder.Services.AddScoped<IApiService, APIService>();
            builder.Services.AddScoped<IAIReviewService, GeminiReviewService>();
            builder.Services.AddScoped<IGithubService, GitHubService>();
            builder.Services.AddScoped<IBridgeService, BridgeService>();
            builder.Services.AddAutoMapper(typeof(Program));

            builder.Services.AddControllers();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    policy => policy.WithOrigins("http://your-frontend.com")
                                    .AllowAnyHeader()
                                    .AllowAnyMethod()
                                    .AllowCredentials());
            });

            var app = builder.Build();
            // Configure the HTTP request pipeline.
            app.UseCors("AllowSpecificOrigin");
            //app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            var client = app.Services.GetRequiredService<IMongoClient>();
            Console.WriteLine("Connected to MongoDB at: " + client.Settings.Server);

            app.Run();
        }
    }
}
