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

        public void StartTournament(int id)
        {
            Tournament? tournament = _tournamentRepository.GetOneById(id);


            if (tournament == null)
            {
                throw new Exception("This tournament doesn't exist");
            }
            if (tournament.CurrentPlayers < tournament.MinPlayers)
            {
                throw new Exception("Not enough players.");
            }
            _tournamentRepository.StartTournament(id);
            CreateEncounters(id);
        }

        public void CreateEncounters(int tournamentId)
        {
            // 1️ Get list of registered players
            List<int> userIds = _tournamentRepository.GetUserIdsForTournament(tournamentId);

            // 2️ Add a dummy player (-1) if odd number
            if (userIds.Count % 2 != 0)
                userIds.Add(-1);

            int nbPlayers = userIds.Count;
            int totalRounds = nbPlayers - 1;
            int encounterPerRound = nbPlayers / 2;

            // 3️ Copy for rotation
            List<int> rotation = userIds;

            // 4️ Generate all rounds
            for (int round = 0; round < totalRounds; round++)
            {
                for (int i = 0; i < encounterPerRound; i++)
                {
                    int white = rotation[i];
                    int black = rotation[nbPlayers - 1 - i];

                    // Skip bye matches
                    if (white == -1 || black == -1)
                        continue;

                    // Create the encounter in DB
                    _tournamentRepository.CreateEncounter(tournamentId, white, black, round + 1);
                }

                // 5 Rotate players (except the first)
                int lastPlayerOfRotation = rotation[rotation.Count - 1];
                rotation.RemoveAt(nbPlayers - 1);
                rotation.Insert(1, lastPlayerOfRotation);
            }

            // reversed order
            // Recreate rotation to start again from initial order
            rotation = userIds;

            for (int round = 0; round < totalRounds; round++)
            {
                for (int i = 0; i < encounterPerRound; i++)
                {
                    int white = rotation[i];
                    int black = rotation[nbPlayers - 1 - i];

                    if (white == -1 || black == -1)
                        continue;

                    // revenge match
                    _tournamentRepository.CreateEncounter(tournamentId, black, white, totalRounds + round + 1);
                }

                // Rotate players (except the first)
                int lastPlayer = rotation[rotation.Count - 1];
                rotation.RemoveAt(nbPlayers - 1);
                rotation.Insert(1, lastPlayer);
            }
        }
    }
}
