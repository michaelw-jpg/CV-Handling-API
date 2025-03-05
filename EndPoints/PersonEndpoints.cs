using CV_Handling_API.Data;
using CV_Handling_API.DTOs;
using CV_Handling_API.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CV_Handling_API.EndPoints
{
    public class PersonEndpoints
    {

        public static void ConfigurePersonEndpoints(WebApplication app)
        {
           
            //adds new person and does validation for input fields
            app.MapPost("/persons/new", async (CVHandlingDBContext context, HttpContext httpContext) =>
            {
                var query = httpContext.Request.Query;
                List<string> requiredKeys = new List<string> { "firstName", "lastName", "email", "phone" };

                foreach (string key in requiredKeys)
                {
                    if (string.IsNullOrWhiteSpace(query[key]))
                    {
                        return Results.BadRequest($"You need to provide all fields");
                    }
                }

                PersonDTO personDTO = new PersonDTO
                {
                    FirstName = query["firstName"],
                    LastName = query["lastName"],
                    EmailAddress = query["email"],
                    PhoneNumber = query["phone"]
                };

                var validationContext = new ValidationContext(personDTO);
                var validationResult = new List<ValidationResult>();

                bool isValid = Validator.TryValidateObject(personDTO, validationContext, validationResult, true);

                if (!isValid)
                {
                    return Results.BadRequest(validationResult.Select(v => v.ErrorMessage));
                }


                Person newPerson = new Person
                {
                    FirstName = personDTO.FirstName,
                    LastName = personDTO.LastName,
                    EmailAddress = personDTO.EmailAddress,
                    PhoneNumber = personDTO.PhoneNumber
                };


                context.Add(newPerson);
                await context.SaveChangesAsync();

                var locationUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/persons/{newPerson.PersonID}";

                return Results.Created(locationUrl, newPerson);

            });


            //finds all persons + data from related fields
            app.MapGet("/persons", async (int? id, CVHandlingDBContext context) =>
            {
                if (id != null)
                {
                    var PersonList = await context.Persons
                    .Where(p => p.PersonID == id)
                    .Include(p => p.Educations)
                    .Include(p => p.Experiences)
                    .ToListAsync();
                    return Results.Ok(PersonList);
                }

                var personList = await context.Persons
                .Include(p => p.Educations)
                .Include(p => p.Experiences)
                .ToListAsync();
                return Results.Ok(personList);
            });
        }
    }
}
