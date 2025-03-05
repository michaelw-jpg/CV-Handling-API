using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CV_Handling_API.DTOs
{
    public class ExperienceDTO
    {

        [Required, StringLength(50)]
        public string Title { get; set; }

        [Required, StringLength(50)]
        public string Company { get; set; }

        [StringLength(280)]
        public string? WorkDescription { get; set; }

        [Required, DataType(DataType.Date)]
        public DateOnly StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateOnly? EndDate { get; set; }

        [Required]
        public int PersonID_FK { get; set; }
    }
}
