using ChessProject.DL.Entities;
using ChessProject.Models.Tournament;

namespace ChessProject.Mappers
{
    public static class TournamentMapper
    {
        public static TournamentListDto ToTournamentListDto(this Tournament t)
        {
            return new TournamentListDto()
            {
                Id = t.Id,
                Status = t.Status,
                Categories = t.Categories,
                CategoriesDisplay = string.Join(", ", t.Categories.Select(c => c.ToString())),
                CreatedAt = t.CreatedAt,
                CurrentRound = t.CurrentRound,
                Location = t.Location,
                MaxElo = t.MaxElo,
                MinElo = t.MinElo,
                MaxPlayers = t.MaxPlayers,
                MinPlayers = t.MinPlayers,
                Name = t.Name,
                RegistrationDeadline = t.RegistrationDeadline,
                UpdatedAt = t.UpdatedAt,
                WomenOnly = t.WomenOnly,
            };
        }
    }
}
