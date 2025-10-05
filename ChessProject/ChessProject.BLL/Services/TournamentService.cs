using ChessProject.DAL.Repositories;
using ChessProject.DL.Entities;
using ChessProject.DL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessProject.BLL.Services
{
    public class TournamentService
    {
        private readonly TournamentRepository _tournamentRepository;
        private readonly UserService _userService;
        private readonly CategoryService _categoryService;

        public TournamentService(TournamentRepository tournamentRepository, UserService userService, CategoryService categoryService)
        {
            _tournamentRepository = tournamentRepository;
            _userService = userService;
            _categoryService = categoryService;
        }

        public List<Tournament> GetAll()
        {
            List<Tournament> tournaments = _tournamentRepository.GetAll();

            tournaments.ForEach(t => t.CurrentPlayers = _tournamentRepository.GetNumberOfUserInTournament(t.Id));

            return tournaments;
        }

        public void Add(Tournament t)
        {
            t.Categories = t.CategoryId.Select(id => _categoryService.GetCategoryById(id)).ToList();
            _tournamentRepository.Add(t);
        }

        public bool DeleteTournamentById(int id)
        {
           return _tournamentRepository.DeleteTournamentById(id);
        }

        public Tournament GetOneById(int id)
        {
            Tournament? tournament = _tournamentRepository.GetOneById(id);

            if (tournament == null)
            {
                throw new Exception("This tournament doesn't exist");
            }

            tournament.CurrentPlayers = _tournamentRepository.GetNumberOfUserInTournament(id);

            return tournament;
        }

        public bool AddUserToTournament(int tournamentId, int userId)
        {
            Tournament tournament = GetOneById(tournamentId);
            User user = _userService.GetById(userId);

            int userAgeAtRegistrationDeadLine = tournament.RegistrationDeadline.Year - user.BirthDate!.Value.Year;

            if (tournament.RegistrationDeadline.Month < user.BirthDate.Value.Month)
            {
                userAgeAtRegistrationDeadLine--;
            }
            bool catCheck = false;

            if (tournament.Status != TournamentStatus.WaitingForPlayers)
            {
                throw new Exception("Tournament already started or finished");
            }

            if (tournament.RegistrationDeadline < DateTime.UtcNow)
            {
                throw new Exception("Registrations are closed");
            }

            if (_tournamentRepository.UserExistInTournament(tournamentId, userId))
            {
                throw new Exception("User already registered for this tournament");
            }

            if (tournament.CurrentPlayers >= tournament.MaxPlayers)
            {
                throw new Exception("Tournament already full");
            }

            foreach (var cat in tournament.Categories)
            {
                if (userAgeAtRegistrationDeadLine >= cat.MinAge && userAgeAtRegistrationDeadLine <= cat.MaxAge)
                {
                    catCheck = true;
                }
            }

            if (!catCheck)
            {
                throw new Exception("User can't join these categories");
            }

            if (tournament.MinElo != null)
            {
                if (user.ELO < tournament.MinElo)
                {
                    throw new Exception("User doesn't have the required ELO rank");
                }
            }

            if (tournament.MaxElo != null)
            {
                if (user.ELO > tournament.MaxElo)
                {
                    throw new Exception("User rank is too hight");
                }
            }

            if (tournament.WomenOnly)
            {
                if (user.Gender != Gender.Female)
                {
                    throw new Exception("Tournament for women only");
                }
            }

            _tournamentRepository.AddUserToTournament(tournamentId, userId);

            return true;
        }
    }
}
