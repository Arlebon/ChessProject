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
        public TournamentService(TournamentRepository tournamentRepository)
        {
            _tournamentRepository = tournamentRepository;
        }

        public List<Tournament> GetAll()
        {
            return _tournamentRepository.GetAll();
        }

        public void Add(Tournament t)
        {
            _tournamentRepository.Add(t);
        }

        public bool DeleteTournamentById(int id)
        {
           return _tournamentRepository.DeleteTournamentById(id);
        }
    }
}
