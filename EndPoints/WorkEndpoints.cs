using CV_Handling_API.Data;
using CV_Handling_API.DTOs;
using CV_Handling_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;

namespace CV_Handling_API.EndPoints
{
    public class WorkEndpoints
    {

        public static void ConfigureWorkEndpoints(WebApplication app)
        {

            
            //updates a existing work experience
            app.MapPut("/persons/{pid}/work/{id}/update", async (HttpContext httpContext,CVHandlingDBContext context, int id, int pid) =>
            {
                var query = httpContext.Request.Query;
                var existingExperience = await context.Experiences
                .Where(e => e.ExperienceID == id && e.PersonID_FK == pid)
                .FirstOrDefaultAsync();

                if (existingExperience == null)
                return Results.BadRequest("Experience not found");

                if (!string.IsNullOrWhiteSpace(query["Title"]))
                    existingExperience.Title = query["Title"];

                if (!string.IsNullOrWhiteSpace(query["Company"]))
                    existingExperience.Company = query["Company"];

                if (!string.IsNullOrWhiteSpace(query["StartDate"]))
                {
                    if (!DateOnly.TryParse(query["StartDate"], out DateOnly parsedStartDate))
                        return Results.BadRequest("Invalid Startdate format, use YYYY-MM-DD");
                    
                    existingExperience.StartDate = parsedStartDate;
                }

                if (query.ContainsKey("EndDate"))  // checks if input includes endDate
                {
                    if (string.IsNullOrWhiteSpace(query["EndDate"]))
                    {
                        // if userinput empty string set enddate to null
                        existingExperience.EndDate = null;
                    }
                    else if (!DateOnly.TryParse(query["EndDate"], out DateOnly parsedEndDate))
                    {
                        return Results.BadRequest("Invalid EndDate format, use YYYY-MM-DD");
                    }
                    else
                    {
                        if (parsedEndDate > DateOnly.FromDateTime(DateTime.Today))
                            return Results.BadRequest("EndDate must be before today");

                        existingExperience.EndDate = parsedEndDate;
                    }
                }

                if (existingExperience.EndDate.HasValue && existingExperience.StartDate > existingExperience.EndDate)
                    return Results.BadRequest("EndDate must be after StartDate");

                if (!string.IsNullOrWhiteSpace(query["WorkDescription"]))
                    existingExperience.WorkDescription = query["WorkDescription"];


               
                await context.SaveChangesAsync();

                return Results.Ok(existingExperience);


            });
                
               
            
            //add a new work experience to a existing person
            app.MapPost("/persons/{id}/work/add", async (HttpContext httpContext, CVHandlingDBContext context, int id) =>
            {
                var query = httpContext.Request.Query;
                List<string> requiredKeys = new List<string> { "Title", "Company", "StartDate", };

                var personExists = await context.Persons.FindAsync(id);
                if (personExists == null)
                    return Results.BadRequest("Person does not exist");

                foreach (string key in requiredKeys)
                {
                    if (string.IsNullOrWhiteSpace(query[key]))
                    {
                        return Results.BadRequest($"You need to provide all required fields");
                    }
                }

                if (!DateOnly.TryParse(query["StartDate"], out DateOnly startDate))
                    return Results.BadRequest("Invalid StartDate format, use YYYY-MM-DD");

                //endDate is optional so if user not inputted enddate set to null if input exists do checks
                DateOnly? endDate = null;
                if (!string.IsNullOrWhiteSpace(query["EndDate"]))
                {
                    if (!DateOnly.TryParse(query["EndDate"], out DateOnly parsedEndDate))
                        return Results.BadRequest("Invalid EndDate format, use YYYY-MM-DD");

                    endDate = parsedEndDate;

                    if (startDate > endDate)
                        return Results.BadRequest("Startdate must be before enddate");

                    if (endDate > DateOnly.FromDateTime(DateTime.Today))
                        return Results.BadRequest("EndDate must be before today");
                }

                
                //create a DTO
                ExperienceDTO expDto = new ExperienceDTO
                {
                    Title = query["Title"],
                    Company = query["Company"],
                    WorkDescription = query["WorkDescription"],
                    StartDate = startDate,
                    EndDate = endDate,
                    PersonID_FK = id
                };

                var validationContext = new ValidationContext(expDto);
                var validationResult = new List<ValidationResult>();
                //validation check using data annotations
                bool isValid = Validator.TryValidateObject(expDto, validationContext, validationResult, true);

                if (!isValid)
                {
                    return Results.BadRequest(validationResult.Select(v => v.ErrorMessage));
                }

                //put data from DTO to new Experience class
                Experience newExp = new Experience
                {

                    Title = expDto.Title,
                    Company = expDto.Company,
                    WorkDescription = expDto.WorkDescription,
                    StartDate = expDto.StartDate,
                    EndDate = expDto.EndDate,
                    PersonID_FK = expDto.PersonID_FK
                };

                context.Add(newExp);
                await context.SaveChangesAsync();

                var locationUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/persons/{id}/work/{newExp.ExperienceID}";
                return Results.Created(locationUrl, newExp);
                
            });
                
            
            //finds work of specifik person id
            app.MapGet("/persons/{id}/work",async ( CVHandlingDBContext context, int id) =>
            {
                var person = await context.Persons
                .Where(p => p.PersonID == id)
                .Include(p => p.Experiences)
                .FirstOrDefaultAsync();

                if (person == null)
                    return Results.BadRequest("No person found");

                var experienceList = person.Experiences.ToList();
                if (!experienceList.Any())
                    return Results.BadRequest("Person has no experience");

                return Results.Ok(experienceList);
       
            });

            //deletes a work experience
            app.MapDelete("/persons/{pid}/work/{id}/remove", async(CVHandlingDBContext context, int id, int pid) =>
            {
                var existingExperience = await context.Experiences
                .Where(e => e.ExperienceID == id && e.PersonID_FK == pid)
                .FirstOrDefaultAsync();

                if (existingExperience == null)
                    return Results.NotFound("Work experience not found");

                context.Experiences.Remove(existingExperience);
                await context.SaveChangesAsync();
                return Results.Ok($"Work experience with id:{id} has been removed");
            });
        }
    }
}
