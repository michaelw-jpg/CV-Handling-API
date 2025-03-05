using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CV_Handling_API.Models
{
    public class Experience
    {
        [Key]
        public int ExperienceID { get; set; }

        [Required, StringLength(50)]
        public string Title { get; set; }

        [Required, StringLength(50)]
        public string Company { get; set; }

        [StringLength(280)]
        public string? WorkDescription { get; set; }

        [Required, DataType(DataType.Date)]
        [Column(TypeName = "date")]
        public DateOnly StartDate { get; set; }

        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        public DateOnly? EndDate { get; set; }


        [ForeignKey("Person")]
        [Required]
        public int PersonID_FK { get; set; }
        [JsonIgnore]
        public virtual Person Person { get; set; }

    }
}
