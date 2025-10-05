using ChessProject.DAL.Repositories;
using ChessProject.DL.Entities;
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
        private readonly UserRepository _userRepository;
        public TournamentService(TournamentRepository tournamentRepository, UserRepository  userRepository)
        {
            _tournamentRepository = tournamentRepository;
            _userRepository = userRepository;
        }

        public List<Tournament> GetAll()
        {
            return _tournamentRepository.GetAll();
        }

        public void Add(Tournament t)
        {
            Tournament? tournament = _tournamentRepository.GetOneById(t.Id);

            if (tournament != null)
            {
                throw new Exception("Tournament with this ID already exists.");
            }
            _tournamentRepository.Add(t);
        }

        public bool DeleteTournamentById(int id)
        {
            Tournament? tournament = _tournamentRepository.GetOneById(id);

            if (tournament == null)
            {
                throw new Exception("This tournament doesn't exist");
            }
            return _tournamentRepository.DeleteTournamentById(id);
        }

        public Tournament GetOneById(int id)
        {
            Tournament? tournament = _tournamentRepository.GetOneById(id);

            if (tournament == null)
            {
                throw new Exception("This tournament doesn't exist");
            }

            return tournament;
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
