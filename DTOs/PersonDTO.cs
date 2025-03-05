using System.ComponentModel.DataAnnotations;

namespace CV_Handling_API.DTOs
{
    public class PersonDTO
    {

        [Required, StringLength(25)]
        public string FirstName { get; set; }

        [Required, StringLength(25)]
        public string LastName { get; set; }

        [Required, EmailAddress(ErrorMessage = "Invalid Email")]
        public string EmailAddress { get; set; }

        [Required, Phone(ErrorMessage = "Invalid Number")]
        public string PhoneNumber { get; set; }


        public PersonDTO()
        {

        }

        public PersonDTO(string firstName, string lastName, string emailAddress, string phoneNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            EmailAddress = emailAddress;
            PhoneNumber = phoneNumber;
        }
    }


}
