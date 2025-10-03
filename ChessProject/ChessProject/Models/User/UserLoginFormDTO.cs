using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ChessProject.Models.User
{
    public class UserLoginFormDTO
    {
        [Required(ErrorMessage = "Field required")]
        [DisplayName("Email or username")]
        public string Login { get; set; } = null!;

        [Required(ErrorMessage = "Field required")]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string Password { get; set; } = null!;

    }
}
