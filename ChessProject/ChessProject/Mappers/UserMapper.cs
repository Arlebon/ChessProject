using ChessProject.DL.Entities;
using ChessProject.Models.User;

namespace ChessProject.Mappers
{
    public static class UserMapper
    {
        public static User ToUser(this UserRegisterFormDTO form)
        {
            return new User()
            {
                UserName = form.UserName,
                Email = form.Email,
                BirthDate = form.BirthDate,
                ELO = form.ELO,
                Gender = form.Gender,
            };
        }
    }
}
