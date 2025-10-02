using ChessProject.DL.Enums;
using System.ComponentModel.DataAnnotations;

namespace ChessProject.Models.Tournament
{
    public class TournamentListDto
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [MaxLength(200)]
        public string? Location { get; set; }

        [Range(2, 32)]
        public int MinPlayers { get; set; }

        [Range(2, 32)]
        public int MaxPlayers { get; set; }

        [Range(0, 3000)]
        public int? MinElo { get; set; }

        [Range(0, 3000)]
        public int? MaxElo { get; set; }

        public string? CategoriesDisplay { get; set; }
        public List<TournamentCategory> Categories { get; set; } = new();

        public TournamentStatus Status { get; set; }

        public int CurrentRound { get; set; }

        public bool WomenOnly { get; set; }

        [Required]
        public DateTime RegistrationDeadline { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
