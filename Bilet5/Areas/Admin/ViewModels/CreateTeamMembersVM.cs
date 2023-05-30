using System.ComponentModel.DataAnnotations;

namespace Bilet5.Areas.Admin.ViewModels
{
	public class CreateTeamMembersVM
	{
		[Required, MaxLength(100)]
		public string Fullname { get; set; }
		[Required, MaxLength(255)]
		public string Information { get; set; }
		[Required, MaxLength(100)]
		public string JobDescription { get; set; }
		[Required]
		public IFormFile Photo { get; set; }
	}
}
