using System.ComponentModel.DataAnnotations;

namespace CV_Handling_API.Models
{
    public class Person
    {
        [Key]
        public int PersonID { get; set; }

        [Required, StringLength(25)]
        public string FirstName { get; set; }

        [Required, StringLength(25)]
        public string LastName { get; set; }

        [Required, EmailAddress(ErrorMessage = "Invalid Email")]
        public string EmailAddress { get; set; }

        [Required, Phone(ErrorMessage = "Invalid Number")]
        public string PhoneNumber { get; set; }

        public virtual List<Education> Educations { get; set; } = new List<Education>();
        public virtual List<Experience> Experiences { get; set; } = new List<Experience>();

    }
}
