
using CV_Handling_API.Data;
using System.Net.Http.Json;
using CV_Handling_API.DTOs;
using CV_Handling_API.EndPoints;
using CV_Handling_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;

namespace CV_Handling_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<CVHandlingDBContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            PersonEndpoints.ConfigurePersonEndpoints(app);

            WorkEndpoints.ConfigureWorkEndpoints(app);

            EducationEndpoints.ConfigureEducationEndpoints(app);

            app.MapGet("github/{username}/repos", async (string username) =>
            {
                using var httpClient = new HttpClient();
                
                string githubURL = $"https://api.github.com/users/{username}/repos";

                httpClient.DefaultRequestHeaders.Add("User-Agent", "MinimalAPI-Client");

                try
                {
                    var repos = await httpClient.GetFromJsonAsync<List<GitHubRepo>>(githubURL);

                    if (repos == null || repos.Count == 0)
                        return Results.NotFound("No public repos found");

                    var formattedRepos = repos.Select(repo => new
                    {
                        Name = repo.Name,
                        Language = string.IsNullOrEmpty(repo.Language) ? "Unknown" : repo.Language,
                        Description = string.IsNullOrEmpty(repo.Description) ? "No description" : repo.Description,
                        url = repo.HtmlUrl
                    });
                    return Results.Ok(formattedRepos);
                }
                catch (HttpRequestException ex)
                {
                    return Results.Problem($"Error with call to github API: {ex}");
                }
            });
          

            app.Run();
        }
    }
}
