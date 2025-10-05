using ChessProject.DL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessProject.DL.Entities
{
    public class Tournament
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

        public int CurrentPlayers { get; set; }

        [Range(0, 3000)]
        public int? MinElo { get; set; }

        [Range(0, 3000)]
        public int? MaxElo { get; set; }

        public List<int>CategoryId { get; set; } = new List<int>();

        public List<Category> Categories { get; set; } = new();

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
