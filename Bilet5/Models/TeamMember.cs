using System.ComponentModel.DataAnnotations;

namespace Bilet5.Models
{
    public class TeamMember
    {
        public int Id { get; set; }
        [Required,MaxLength(100)]
        public string Fullname { get; set; }
        [Required, MaxLength(255)]
        public string Information { get; set; }
        [Required, MaxLength(100)]
        public string JobDescription { get; set; }
        public bool IsDelected { get; set; } = default;
        [Required]
        public string ImagePath { get; set; }
    }
}
