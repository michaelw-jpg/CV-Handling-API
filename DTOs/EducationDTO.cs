using CV_Handling_API.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CV_Handling_API.DTOs
{
    public class EducationDTO
    {

        [Key]
        public int EducationID { get; set; }

        [Required, StringLength(100)]
        public string School { get; set; }

        [Required, StringLength(100)]
        public string Degree { get; set; }

        [Required, DataType(DataType.Date)]
        public DateOnly StartDate { get; set; }

        [Required, DataType(DataType.Date)]
        public DateOnly EndDate { get; set; }

        [MaxLength(280)]
        public string? EduDescription { get; set; }

        [Required]
        public int PersonID_FK { get; set; }
      
  
    }
}
