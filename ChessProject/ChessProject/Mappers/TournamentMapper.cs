using ChessProject.BLL.Services;
using ChessProject.DL.Entities;
using ChessProject.DL.Enums;
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
                //CategoriesDisplay = string.Join(", ", t.Categories.Select(c => c.ToString())),
                CurrentRound = t.CurrentRound,
                Location = t.Location,
                MaxElo = t.MaxElo,
                MinElo = t.MinElo,
                MaxPlayers = t.MaxPlayers,
                MinPlayers = t.MinPlayers,
                CurrentPlayers = t.CurrentPlayers,
                Name = t.Name,
                RegistrationDeadline = t.RegistrationDeadline,
                WomenOnly = t.WomenOnly,
            };
        }

        public static Tournament ToTournament(this TournamentFormDto form)
        {
            return new Tournament()
            {
                Name=form.Name,
                CategoryId = form.Categories,
                Location =form.Location,
                MinElo=form.MinElo,
                MaxElo=form.MaxElo,
                MinPlayers=form.MinPlayers,
                MaxPlayers=form.MaxPlayers,
                RegistrationDeadline=form.RegistrationDeadline,
                WomenOnly=form.WomenOnly,

                //Default Values
                Status= TournamentStatus.WaitingForPlayers,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt= DateTime.UtcNow,
                CurrentRound = 0,
            };
        }

        public static TournamentDetailsDTO ToTournamentDetailsDTO(this Tournament t)
        {
            return new TournamentDetailsDTO()
            {
                Id = t.Id,
                Name = t.Name,
                Location = t.Location,
                MinPlayers = t.MinPlayers,
                MaxPlayers = t.MaxPlayers,
                MinElo = t.MinElo,
                MaxElo = t.MaxElo,
                Categories = t.Categories,
                Status = t.Status,
                CurrentRound = t.CurrentRound,
                WomenOnly = t.WomenOnly,
                RegistrationDeadline = t.RegistrationDeadline,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt,
            };
        }
    }
}
