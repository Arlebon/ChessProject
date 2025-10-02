using ChessProject.DL.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ChessProject.Models.User
{
    public class UserRegisterFormDTO
    {
        public string UserName { get; set; } = null!;

        public string Email { get; set; } = null!;

        [Required]
        [DisplayName("Date of Birth")]
        [DataType(DataType.Date)]
        public DateOnly? BirthDate { get; set; } = null;

        public Gender Gender { get; set; }

        public int? ELO { get; set; }
    }
}
