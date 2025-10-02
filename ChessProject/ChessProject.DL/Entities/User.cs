using ChessProject.DL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessProject.DL.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string UserName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public DateOnly? BirthDate { get; set; }

        public Gender Gender { get; set; }

        public int? ELO { get; set; }

        public Role Role { get; set; }
    }
}
