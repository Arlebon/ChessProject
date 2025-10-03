using ChessProject.DL.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ChessProject.Models.User
{
    public class UserRegisterFormDTO
    {
        [Required(ErrorMessage = "Field required")]
        [MaxLength(50)]
        [DisplayName("Username")]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = "Field required")]
        [MaxLength(255)]
        [DisplayName("Email")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Field required")]
        [DisplayName("Date of Birth")]
        [DataType(DataType.Date)]
        public DateOnly? BirthDate { get; set; } = null;

        [DisplayName("Gender")]
        public Gender Gender { get; set; }

        [Range(0, 3000, ErrorMessage = "ELO must be between 0 and 3000")]
        [DisplayName("ELO Rank")]
        public int? ELO { get; set; }
    }
}
