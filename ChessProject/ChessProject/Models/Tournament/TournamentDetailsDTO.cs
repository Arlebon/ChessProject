using ChessProject.DL.Entities;
using ChessProject.DL.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ChessProject.Models.Tournament
{
    public class TournamentDetailsDTO
    {
        public int Id { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; } = null!;

        [DisplayName("Location")]
        public string? Location { get; set; }

        [DisplayName("Minimum number of players")]
        public int MinPlayers { get; set; }

        [DisplayName("Maximum number of players")]
        public int MaxPlayers { get; set; }

        [DisplayName("Minimum ELO rank")]
        public int? MinElo { get; set; }

        [DisplayName("Maximum ELO rank")]
        public int? MaxElo { get; set; }

        [DisplayName("Categories")]
        public List<Category> Categories { get; set; } = new();

        [DisplayName("Status")]
        public TournamentStatus Status { get; set; }

        [DisplayName("Current round")]
        public int CurrentRound { get; set; }

        [DisplayName("Women only ?")]
        public bool WomenOnly { get; set; }

        [DisplayName("Registration deadline")]
        public DateTime RegistrationDeadline { get; set; }

        [DisplayName("Created at")]
        public DateTime CreatedAt { get; set; }

        [DisplayName("Last update at")]
        public DateTime UpdatedAt { get; set; }
    }
}
