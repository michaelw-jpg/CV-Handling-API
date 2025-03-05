using CV_Handling_API.Data;
using CV_Handling_API.DTOs;
using CV_Handling_API.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CV_Handling_API.EndPoints
{
    public class EducationEndpoints
    {

        public static void ConfigureEducationEndpoints(WebApplication app)
        {
           //finds all educations of specifik person
            app.MapGet("/persons/{id}/educations", async (CVHandlingDBContext context, int id) =>
            {
                var person = await context.Persons
                .Where(p => p.PersonID == id)
                .Include(p => p.Educations)
                .FirstOrDefaultAsync();

                if (person == null)
                    return Results.BadRequest("No person found");

                var educationList = person.Educations.ToList();
                if (!educationList.Any())
                    return Results.BadRequest("Person has no educations listed");

                return Results.Ok(educationList);

            });

            //updates a specifik education
            app.MapPut("/persons/{pid}/educations/{id}/update", async (HttpContext httpContext, CVHandlingDBContext context, int id, int pid) =>
            {
                var query = httpContext.Request.Query;
                var existingEducation = await context.Educations
                .Where(e => e.EducationID == id && e.PersonID_FK == pid)
                .FirstOrDefaultAsync();

                if (existingEducation == null)
                    return Results.BadRequest("Education not found");

                if (!string.IsNullOrWhiteSpace(query["School"]))
                    existingEducation.School = query["School"];

                if (!string.IsNullOrWhiteSpace(query["Degree"]))
                    existingEducation.Degree = query["Degree"];

                if (!string.IsNullOrWhiteSpace(query["StartDate"]))
                {
                    if (!DateOnly.TryParse(query["StartDate"], out DateOnly parsedStartDate))
                        return Results.BadRequest("Invalid Startdate format, use YYYY-MM-DD");

                    existingEducation.StartDate = parsedStartDate;
                }

                if (!string.IsNullOrWhiteSpace(query["EndDate"]))
                {
                    if (!DateOnly.TryParse(query["StartDate"], out DateOnly parsedEndDate))
                        return Results.BadRequest("Invalid Startdate format, use YYYY-MM-DD");

                    existingEducation.EndDate = parsedEndDate;
                }

                if (existingEducation.StartDate > existingEducation.EndDate)
                    return Results.BadRequest("EndDate must be after StartDate");

                if (!string.IsNullOrWhiteSpace(query["EduDescription"]))
                    existingEducation.EduDescription = query["EduDescription"];



                await context.SaveChangesAsync();

                return Results.Ok(existingEducation);


            });

            app.MapPost("/persons/{id}/educations/new", async (HttpContext httpContext, CVHandlingDBContext context, int id) => 
            {
                var query = httpContext.Request.Query;
                List<string> requiredKeys = new List<string> { "School", "Degree", "StartDate", "EndDate" };

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

                
                if (!DateOnly.TryParse(query["EndDate"], out DateOnly endDate))
                        return Results.BadRequest("Invalid EndDate format, use YYYY-MM-DD");

                if (startDate > endDate)
                        return Results.BadRequest("Startdate must be before enddate");

                //create EduDTO
                EducationDTO eduDTO = new EducationDTO
                {
                    School = query["School"],
                    Degree = query["Degree"],
                    EduDescription = query["EduDescription"],
                    StartDate = startDate,
                    EndDate = endDate,
                    PersonID_FK = id
                };

                var validationContext = new ValidationContext(eduDTO);
                var validationResult = new List<ValidationResult>();
                //validation check using data annotations
                bool isValid = Validator.TryValidateObject(eduDTO, validationContext, validationResult, true);

                if (!isValid)
                {
                    return Results.BadRequest(validationResult.Select(v => v.ErrorMessage));
                }

                Education newEdu = new Education
                {
                    School = eduDTO.School,
                    Degree = eduDTO.Degree,
                    EduDescription = eduDTO.EduDescription,
                    StartDate = eduDTO.StartDate,
                    EndDate = eduDTO.EndDate,
                    PersonID_FK = eduDTO.PersonID_FK
                };

                context.Add(newEdu);
                await context.SaveChangesAsync();

                var locationUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/persons/{id}/edu/{newEdu.EducationID}";
                return Results.Created(locationUrl, newEdu);
            });

            app.MapDelete("/persons/{pid}/educations/{id}/remove", async (CVHandlingDBContext context, int id, int pid) =>
            {
                var existingEducation = await context.Educations
                .Where(e => e.EducationID == id && e.PersonID_FK == pid)
                .FirstOrDefaultAsync();

                if (existingEducation == null)
                    return Results.NotFound("Education was not found");

                context.Educations.Remove(existingEducation);
                await context.SaveChangesAsync();
                return Results.Ok($"Education with {id} has been removed");
            });
        }
    }
}
