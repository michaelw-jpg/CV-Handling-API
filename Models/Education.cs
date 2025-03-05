using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CV_Handling_API.Models
{
    public class Education
    {
        [Key]
        public int EducationID { get; set; }

        [Required, StringLength(100)]
        public string School { get; set; }

        [Required, StringLength(100)]
        public string Degree { get; set; }

        [Required,DataType(DataType.Date)]
        [Column(TypeName = "date")]
        public DateOnly StartDate { get; set; }

        [Required, DataType(DataType.Date)]
        [Column(TypeName = "date")]
        public DateOnly EndDate { get; set; }

        [MaxLength(280)]
        public string? EduDescription { get; set; }

        [Required]
        [ForeignKey("Person")]
        public int PersonID_FK { get; set; }
        [JsonIgnore]
        public virtual Person Person { get; set; }
    }
}
